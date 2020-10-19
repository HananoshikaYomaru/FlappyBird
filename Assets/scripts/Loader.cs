using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    // the names have to be the same as your Scene name
    public enum Scene
    {
        GameScene,
        Loading,
        MainMenu,
        SettingMenu,
    }

    private static Scene targetScene;

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(Scene.Loading.ToString());
        targetScene = scene;
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
