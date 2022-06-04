using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ENUM_SCENE_TYPE
{
    Unknown,
    Login,
    Lobby,
    Selection, // 안쓸듯? UI로 쓸 거 같음 일단... ㅋ (임시)
    Battle,
    Loading,
}

public abstract class BaseScene : MonoBehaviour
{
    private ENUM_SCENE_TYPE sceneType;
    public ENUM_SCENE_TYPE SceneType { get; protected set; } = ENUM_SCENE_TYPE.Unknown;

    public virtual void Init()
    {
        Managers.UI.Init(); // 임시
    }

    public abstract void Clear();
}
