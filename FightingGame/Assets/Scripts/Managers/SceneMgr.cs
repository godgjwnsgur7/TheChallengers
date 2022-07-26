using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr
{
    public BaseScene CurrentScene
    { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(ENUM_SCENE_TYPE type)
    {
        Managers.Clear();

        string nextScene = System.Enum.GetName(typeof(ENUM_SCENE_TYPE), type);
        LoadingSceneManagement.LoadScene(nextScene);
    }

    public void FadeLoadScene(ENUM_SCENE_TYPE type) 
    {
        Managers.UI.OpenUI<BlackOutPopup>();
        Managers.Clear();

        string nextScene = System.Enum.GetName(typeof(ENUM_SCENE_TYPE), type);
        SceneManager.LoadScene(nextScene);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
