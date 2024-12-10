


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PuppyPrefab : MonoBehaviour
{
    [SerializeField] private GameObject puppyPrefab;
    [SerializeField] private Vector3 prefabOffset;
    
    private GameObject puppy;
    private ARTrackedImageManager arTrackedImageManager;

    void OnEnable()
    {
        arTrackedImageManager = gameObject.GetComponent<ARTrackedImageManager>();
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }




void OnImageChanged(ARTrackedImagesChangedEventArgs args)
{
    foreach (ARTrackedImage image in args.added)
    {
        puppy = Instantiate(puppyPrefab, image.transform);
        puppy.transform.position += prefabOffset;

        var controller = puppy.GetComponent<PuppyController>();
        
        if (controller != null)
        {
            var animator = puppy.GetComponent<Animator>();
            
            if (animator != null)
            {
               // controller.InitializeAnimator(animator);
                Debug.Log("puppy prefab instantiated and Animator initialized.");
            }
            else
            {
                Debug.LogError("Animator component missing on puppyprefab!");
            }
        }
        else
        {
            Debug.LogError("PuppyController not found on instantiated prefab!");
        }
    }
}


}