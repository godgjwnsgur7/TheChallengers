using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneMgr
{
    public BaseScene CurrentScene
    {
        get { return GameObject.FindObjectOfType<BaseScene>(); }
    }

    public void LoadScene(ENUM_SCENE_TYPE type)
    {
        CurrentScene.Clear();

        // 래핑할까 말까 고민중 (임시)
        string name = System.Enum.GetName(typeof(ENUM_SCENE_TYPE), type);
        SceneManager.LoadScene(name);
    }

}
