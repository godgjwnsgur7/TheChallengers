using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;

public class SceneMgr
{
    public BaseScene CurrentScene
    { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public ENUM_SCENE_TYPE CurrSceneType
    {
        private set;
        get;
    }

    public void LoadScene(ENUM_SCENE_TYPE sceneType)
    {
        Managers.Sound.Stop_BGM();
        Managers.UI.popupCanvas.Play_FadeOutEffect();

        CoroutineHelper.StartCoroutine(IDelaySceneLoad(sceneType));
    }

    public void Sync_LoadScene(ENUM_SCENE_TYPE sceneType)
    {
        bool a = PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember(sceneType);
    
        if(!a)
            Debug.LogError($"TrySceneLoadWithRoomMember Return False : {sceneType}");
    }

    public void Get_CurrSceneType(ENUM_SCENE_TYPE sceneType)
    {
        CurrSceneType = sceneType;
    }


    protected IEnumerator IDelaySceneLoad(ENUM_SCENE_TYPE sceneType)
    {
        yield return new WaitUntil(() => Managers.UI.popupCanvas.Get_FadeState());

        string nextScene = System.Enum.GetName(typeof(ENUM_SCENE_TYPE), sceneType);

        PhotonLogicHandler.Instance.LoadScene(sceneType);
    }
}
