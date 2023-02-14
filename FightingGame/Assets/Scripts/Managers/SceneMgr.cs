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


    bool loadSceneLock = false;

    public void LoadScene(ENUM_SCENE_TYPE sceneType)
    {
        loadSceneLock = true;
        Managers.UI.popupCanvas.Play_FadeOutEffect(Unlocking_loadSceneLock);

        CoroutineHelper.StartCoroutine(IDelaySceneLoad(sceneType));
    }

    /// <summary>
    /// Managers.UI.PopupCanvas.Play_FadeInEffect(Action CallBack); 
    /// 콜백함수로 해당 함수를 호출할 것.
    /// </summary>
    public void Sync_LoadScene(ENUM_SCENE_TYPE sceneType)
    {
        if (!PhotonLogicHandler.IsMasterClient)
        {
            Debug.LogError("마스터 클라이언트가 아닌데, 동기화 씬 이동을 시도했습니다.");
            return;
        }

        PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember(sceneType);
    }

    public void Get_CurrSceneType(ENUM_SCENE_TYPE sceneType)
    {
        CurrSceneType = sceneType;
    }

    public void Unlocking_loadSceneLock()
    {
        loadSceneLock = false;
    }

    protected IEnumerator IDelaySceneLoad(ENUM_SCENE_TYPE sceneType)
    {
        yield return new WaitUntil(() => loadSceneLock == false);

        string nextScene = System.Enum.GetName(typeof(ENUM_SCENE_TYPE), sceneType);

        SceneManager.LoadScene(nextScene);
    }
}
