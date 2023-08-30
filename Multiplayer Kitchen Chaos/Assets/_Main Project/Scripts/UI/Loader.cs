using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    private static int targetSceneIndex;
    private static int loadingSceneIndex = 1;

    public static void LoadScene(int _sceneIndex)
    {
        targetSceneIndex = _sceneIndex;
        SceneManager.LoadScene(loadingSceneIndex);
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadSceneAsync(targetSceneIndex);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public static void RestartPlaying()
    {
        SceneManager.LoadSceneAsync(2);
    }
}

