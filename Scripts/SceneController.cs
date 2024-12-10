using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class SceneController : MonoBehaviour
{
    // Method to switch to an AR scene
    public void SwitchToARScene(string sceneName)
    {
        Debug.Log("Button clicked, attempting to load scene: " + sceneName);
        StartCoroutine(LoadScene(sceneName));
    }

    // Method to switch back to the home screen
    public void SwitchToHomeScreen(string homeSceneName)
    {
        Debug.Log("Back button clicked, returning to home screen: " + homeSceneName);
        StartCoroutine(LoadScene(homeSceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        // Reset AR session if needed before switching scenes
        var arSession = FindObjectOfType<ARSession>();
        if (arSession != null)
        {
            arSession.Reset();
            yield return null; // Wait a frame after resetting
        }

        // Load the new scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the new scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene loaded: " + sceneName);

        // Initialize AR components in the new scene if it's an AR scene
        if (sceneName != "HomeScreen") // Replace "HomeScreen" with your actual home screen name
        {
            InitializeARComponents();
        }
    }

    private void InitializeARComponents()
    {
        // Find and enable the ARSession component in the new scene
        var arSession = FindObjectOfType<ARSession>();

        if (arSession != null)
        {
            StartCoroutine(StartARSession(arSession));
        }
    }

    private IEnumerator StartARSession(ARSession arSession)
    {
        // Check availability of AR on this device
        yield return ARSession.CheckAvailability();

        if (ARSession.state == ARSessionState.Ready || 
            ARSession.state == ARSessionState.SessionInitializing || 
            ARSession.state == ARSessionState.SessionTracking)
        {
            arSession.enabled = true;
            Debug.Log("AR session started.");
            // Additional setup for other AR components can be added here
        }
        else
        {
            Debug.LogError("AR is not supported on this device.");
            // Handle unsupported devices here
        }
    }
}