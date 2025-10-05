using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


//this script changes the behaviour of the Play button in unity
//such that it loads the playFromScenePath scene before playing
//Required as we have a bootstrap scene
//However: if editing a playable scene, you must save before playing or changes will be lost
[InitializeOnLoad]
public static class PlayFromScene
{
    private const string playFromScenePath = "Assets/Scenes/LoadingScene.unity";
    private static string previousScenePath;

    static PlayFromScene()
    {
        EditorApplication.playModeStateChanged += LoadStartScene;
    }

    private static void LoadStartScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // Save current scene to restore later
            previousScenePath = SceneManager.GetActiveScene().path;

            // Open the desired start scene
            if (System.IO.File.Exists(playFromScenePath))
            {
                EditorSceneManager.OpenScene(playFromScenePath);
            }
            else
            {
                Debug.LogError("PlayFromScene: Scene not found at " + playFromScenePath);
            }
        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            // Restore previous scene when leaving Play mode
            // this doesn't work for some reason idk
            if (!string.IsNullOrEmpty(previousScenePath))
            {
                EditorSceneManager.OpenScene(previousScenePath);
            }
        }
    }
}
