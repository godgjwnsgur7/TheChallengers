using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneMgr : MonoBehaviour
{
    public BaseScene CurrentScene
    { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(ENUM_SCENE_TYPE type)
    {
        string nextScene = System.Enum.GetName(typeof(ENUM_SCENE_TYPE), type);
        LoadingSceneManagement.LoadScene(nextScene);
    }
}
