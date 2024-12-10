using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro
using System.Collections.Generic; // Required for Dictionary

public class HorizontalScrollMaskManager : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect; // Reference to the horizontal ScrollRect
    [SerializeField] private Button likeButton; // Reference to the like/dislike button
    [SerializeField] private TextMeshProUGUI buttonText; // Reference to the TextMeshPro text
    [SerializeField] private RectTransform[] maskItems; // Array of masks (content items in ScrollRect)
    private Image buttonImage; // Reference to the Button's Image component
    private bool isLiked = false; // Track whether the current mask is liked

    private int currentMaskIndex = 0; // Index of the currently selected mask
    private static Dictionary<int, bool> maskStates = new Dictionary<int, bool>(); // Store like/dislike states for masks

    void Start()
    {
        // Get the Image component from the Button
        buttonImage = likeButton.GetComponent<Image>();

        // Add a listener for the like button
        likeButton.onClick.AddListener(ToggleLike);

        // Initialize the first mask's state and appearance
        UpdateCurrentMask();
    }

    void Update()
    {
        // Dynamically detect which mask is currently in focus based on scroll position
        UpdateMaskIndexBasedOnScroll();
    }

    void ToggleLike()
    {
        // Toggle the "liked" state for the current mask
        isLiked = !isLiked;

        // Save the state for this mask
        SaveState();

        // Update the button's appearance
        UpdateButtonAppearance();
    }

    void UpdateButtonAppearance()
    {
        // Change the button's image color based on the "liked" state
        buttonImage.color = isLiked ? Color.green : Color.red;

        // Set the text color to white (always)
        buttonText.color = Color.white;

        // Update the text to reflect the current state
        buttonText.text = isLiked ? "Dislike" : "Like";
    }

    void SaveState()
    {
        // Save the "liked" state for this mask in the dictionary
        maskStates[currentMaskIndex] = isLiked;
    }

    void LoadState()
    {
        // Load the "liked" state for this mask from the dictionary, defaulting to false (disliked)
        if (maskStates.ContainsKey(currentMaskIndex))
        {
            isLiked = maskStates[currentMaskIndex];
        }
        else
        {
            isLiked = false; // Default state if no record exists
        }
    }

    void UpdateCurrentMask()
    {
        // Load and update appearance for the current mask
        LoadState();
        UpdateButtonAppearance();
    }

    void UpdateMaskIndexBasedOnScroll()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        // Determine which mask item is closest to the center of the viewport
        for (int i = 0; i < maskItems.Length; i++)
        {
            float distance = Mathf.Abs(scrollRect.content.localPosition.x + maskItems[i].localPosition.x);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        // If a new mask is detected, update its state and appearance
        if (closestIndex != currentMaskIndex)
        {
            currentMaskIndex = closestIndex;
            UpdateCurrentMask();
        }
    }
}