using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RaycastPlacement : MonoBehaviour
{
    public GameObject objectToPlace; // Assign your 3D object here
    private ARRaycastManager raycastManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Perform raycast from touch position
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    // Get hit point on detected plane
                    Pose hitPose = hits[0].pose;

                    // Place object at hit point
                    objectToPlace.transform.position = hitPose.position;
                    objectToPlace.transform.rotation = hitPose.rotation;
                }
            }
        }
    }
}