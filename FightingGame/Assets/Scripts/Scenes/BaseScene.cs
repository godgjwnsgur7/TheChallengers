using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FGDefine;

public abstract class BaseScene : MonoBehaviour
{
    public ENUM_SCENE_TYPE SceneType { get; protected set; } = ENUM_SCENE_TYPE.Unknown;

    protected IEnumerator Start()
    {
        if(PhotonLogicHandler.IsFullRoom) // 매칭상태로 씬이동을 함
        {
            PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC);
            yield return new WaitUntil(Managers.Network.Get_SceneSyncAllState);
        }

        yield return null;
        Init();
    }

    public virtual void Init()
    {
        Managers.Sound.Play((ENUM_BGM_TYPE)SceneType, ENUM_SOUND_TYPE.BGM);
        Managers.UI.popupCanvas.Play_FadeInEffect();
        Managers.Scene.Get_CurrSceneType(SceneType);
    }
    
    public virtual void Clear()
    {
    }
}
 