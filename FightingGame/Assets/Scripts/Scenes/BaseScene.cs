using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FGDefine;

public abstract class BaseScene : MonoBehaviour
{
    public ENUM_SCENE_TYPE SceneType { get; protected set; } = ENUM_SCENE_TYPE.Unknown;

    protected virtual IEnumerator Start()
    {
        yield return null;
        Init();
    }

    private void OnDisable()
    {
        Managers.Clear();
    }

    public virtual void Init()
    {
        Managers.UI.popupCanvas.Play_FadeInEffect();
        Managers.Scene.Get_CurrSceneType(SceneType);
        Play_BGM();
    }
    
    public virtual void Clear()
    {
        
    }

    public virtual void Play_BGM()
    {
        Managers.Sound.Play_BGM(ENUM_BGM_TYPE.Unknown);
    }
}
 