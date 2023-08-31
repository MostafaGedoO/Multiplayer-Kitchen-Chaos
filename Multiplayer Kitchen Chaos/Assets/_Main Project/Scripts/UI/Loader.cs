using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    private static Scene targetSceneIndex;
    private static int loadingSceneIndex = 1;

    public enum Scene { MainGameScene, MainMenu, LoadingScene }

    public static void LoadScene(Scene _scene)
    {
        targetSceneIndex = _scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadSceneAsync(targetSceneIndex.ToString());
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(Scene.MainMenu.ToString());
    }

    public static void RestartPlaying()
    {
        SceneManager.LoadSceneAsync(Scene.MainGameScene.ToString());
    }
}

