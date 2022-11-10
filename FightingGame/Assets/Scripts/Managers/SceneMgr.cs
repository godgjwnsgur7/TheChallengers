using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;

public class SceneMgr
{
    public BaseScene CurrentScene
    { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    bool loadSceneLock;

    public void LoadScene(ENUM_SCENE_TYPE sceneType)
    {
        Managers.Clear();

        loadSceneLock = true;
        Managers.UI.popupCanvas.Play_FadeInEffect(Unlocking_loadSceneLock);

        CoroutineHelper.StartCoroutine(IDelaySceneLoad(sceneType));
    }

    public void Unlocking_loadSceneLock()
    {
        loadSceneLock = false;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

    protected IEnumerator IDelaySceneLoad(ENUM_SCENE_TYPE sceneType)
    {
        yield return new WaitUntil(() => loadSceneLock == false);

        string nextScene = System.Enum.GetName(typeof(ENUM_SCENE_TYPE), sceneType);

        SceneManager.LoadScene(nextScene);
    }

}
