using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class MaskController : MonoBehaviour
{
    [SerializeField] private Button[] maskButtons; // Buttons for each mask
    [SerializeField] private Material[] maskMaterials; // Array of mask materials
    private ARFaceManager arFaceManager;

    void Start()
    {
        arFaceManager = FindObjectOfType<ARFaceManager>();

        // Assign button click events
        for (int i = 0; i < maskButtons.Length; i++)
        {
            int index = i; // Capture index for lambda
            maskButtons[i].onClick.AddListener(() => ChangeMask(index));
        }
    }

    void ChangeMask(int index)
    {
        foreach (ARFace face in arFaceManager.trackables)
        {
            // Change material on the face mesh renderer
            var meshRenderer = face.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = maskMaterials[index];
            }
        }
    }
}