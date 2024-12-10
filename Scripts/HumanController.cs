
using UnityEngine;
using System.Collections;

public class HumanController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private FixedJoystick fixedJoystick;
    private Animator animator;
    private Vector3 originalPosition;
    private float groundY;

    private bool isButtonAnimationPlaying = false; // Flag to prevent joystick movement during button animations

    public void InitializeAnimator(Animator newAnimator)
    {
        animator = newAnimator;
    }

    void Start()
{
    fixedJoystick = FindObjectOfType<FixedJoystick>();
    animator = GetComponent<Animator>();
    if (animator == null)
    {
        Debug.LogError("Animator component is not assigned or found. Please ensure the GameObject has an Animator component.");
    }
    else
    {
        Debug.Log("Animator component successfully found and assigned.");
    }
    originalPosition = transform.position;
    groundY = transform.position.y;
}


    void Update()
    {
        // Ensure GameObject and Animator are active
        if (!gameObject.activeInHierarchy || animator == null || !animator.isActiveAndEnabled)
        {
            return;
        }

        // If a button-triggered animation is playing, skip joystick control
        if (isButtonAnimationPlaying) return;

        float xVal = fixedJoystick.Horizontal;
        float yVal = fixedJoystick.Vertical;

        Vector3 movement = new Vector3(xVal, 0, yVal);

        // Only move if there's joystick input
        if (movement.magnitude > 0.1f)
        {
            // Calculate movement in world space
            Vector3 moveDirection = Camera.main.transform.TransformDirection(movement);
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized;

            // Move while maintaining Y position
            Vector3 newPosition = transform.position + moveDirection * speed * Time.deltaTime;
            newPosition.y = groundY; // Keep at ground level
            transform.position = newPosition;

            // Rotate to face movement direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10f * Time.deltaTime);
            }

            // Play walking animation
            if (animator.HasState(0, Animator.StringToHash("forward")))
            {
                animator.Play("forward");
            }
            else
            {
                Debug.LogWarning("Animation state 'forward' not found");
            }
        }
        else
        {
            // Stay at current position but ensure Y position
            Vector3 currentPos = transform.position;
            currentPos.y = groundY;
            transform.position = currentPos;

            // Play idle animation
            if (animator.HasState(0, Animator.StringToHash("Idle")))
            {
                animator.Play("Idle");
            }
            else
            {
                Debug.LogWarning("Animation state 'Idle' not found");
            }
        }
    }

    public void UpdateGroundPosition()
    {
        groundY = transform.position.y;
        originalPosition = transform.position;
    }

    // Method to play animation via button
    public void PlayAnimation(string animationName)
{
    Debug.Log($"{animationName} Button Pressed");
    if (animator == null)
    {
        Debug.LogError("Animator is null. Ensure it is assigned correctly.");
        return;
    }
    if (!animator.isActiveAndEnabled)
    {
        Debug.LogError("Animator is not active or enabled.");
        return;
    }
    if (!animator.HasState(0, Animator.StringToHash(animationName)))
    {
        Debug.LogWarning($"Animation state '{animationName}' not found in the Animator.");
        return;
    }

    if (!isButtonAnimationPlaying)
    {
        isButtonAnimationPlaying = true; // Block joystick control
        animator.Play(animationName);
        StartCoroutine(WaitForAnimation(animationName));
    }
}


    private IEnumerator WaitForAnimation(string animationName)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is null. Ensure it is assigned correctly.");
            yield break;
        }
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log($"Animation {animationName} length: {animationLength}");
        yield return new WaitForSeconds(animationLength);
        ResetButtonAnimation();
    }

    // Reset joystick control after button-triggered animation ends
    private void ResetButtonAnimation()
    {
        isButtonAnimationPlaying = false; // Allow joystick control again
    }
}




