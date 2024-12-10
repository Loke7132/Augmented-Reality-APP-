using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HumanPrefab : MonoBehaviour
{
    [SerializeField] private GameObject humanPrefab;
    [SerializeField] private Vector3 prefabOffset;
    
    private GameObject human;
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
        human = Instantiate(humanPrefab, image.transform);
        human.transform.position += prefabOffset;

        var controller = human.GetComponent<HumanController>();
        
        if (controller != null)
        {
            var animator = human.GetComponent<Animator>();
            
            if (animator != null)
            {
               // controller.InitializeAnimator(animator);
                Debug.Log("Human prefab instantiated and Animator initialized.");
            }
            else
            {
                Debug.LogError("Animator component missing on Human prefab!");
            }
        }
        else
        {
            Debug.LogError("HumanController not found on instantiated prefab!");
        }
    }
}
}
