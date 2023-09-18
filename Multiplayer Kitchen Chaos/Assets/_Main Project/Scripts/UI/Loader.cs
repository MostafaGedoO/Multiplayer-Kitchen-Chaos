using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader 
{
    private static Scene targetSceneIndex;
    private static int loadingSceneIndex = 1;

    public enum Scene { MainGameScene, MainMenu, LoadingScene, Lobby, CharcterSelect }

    public static void LoadScene(Scene _scene)
    {
        targetSceneIndex = _scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadNetworkScene(Scene _scene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(_scene.ToString(), loadSceneMode:LoadSceneMode.Single);
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

