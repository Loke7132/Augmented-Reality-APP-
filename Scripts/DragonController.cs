using System.Collections;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private FixedJoystick fixedJoystick;
    private Rigidbody rigidBody;
    private Animator animator;

    private bool isButtonAnimationPlaying = false; // Flag to prevent joystick movement during button animations

    void OnEnable()
    {
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned or found. Please ensure the GameObject has an Animator component.");
        }
    }

    void FixedUpdate()
    {
        if (isButtonAnimationPlaying) return;

        float xVal = fixedJoystick.Horizontal;
        float yVal = fixedJoystick.Vertical;
        
        Vector3 movement = new Vector3(xVal, 0, yVal);
        rigidBody.velocity = movement * speed;

        if (movement.magnitude > 0.1f)
        {
            if (animator.HasState(0, Animator.StringToHash("Fly")))
            {
               
                animator.Play("Fly");
            }
            else
            {
                Debug.LogWarning("Animation state 'Fly' not found");
            }

         
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                Mathf.Atan2(xVal, yVal) * Mathf.Rad2Deg,
                transform.eulerAngles.z
            );
            
        }
        else
        {
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

    public void PlayAnimation(string animationName)
    {
        Debug.Log($"{animationName} Button Pressed");
        if (animator == null || !animator.isActiveAndEnabled) return;

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
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        
        yield return new WaitForSeconds(animationLength);
        
        ResetButtonAnimation();
    }

    private void ResetButtonAnimation()
    {
        isButtonAnimationPlaying = false; // Allow joystick control again
    }
}