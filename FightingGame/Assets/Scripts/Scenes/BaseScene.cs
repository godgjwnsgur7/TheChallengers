using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ENUM_SCENE_TYPE
{
    Unknown,
    Login,
    Lobby,
    Battle,
    Loading,
    Main,
    CustomRoom,
}

public abstract class BaseScene : MonoBehaviour
{
    private ENUM_SCENE_TYPE sceneType;
    public ENUM_SCENE_TYPE SceneType { get; protected set; } = ENUM_SCENE_TYPE.Unknown;

    private void Start()
    {
        Init();    
    }

    public virtual void Init()
    {
        Managers.UI.Init(); // 임시
    }

    public abstract void Clear();
}
 