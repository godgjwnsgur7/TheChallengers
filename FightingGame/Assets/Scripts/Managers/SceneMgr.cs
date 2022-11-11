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
