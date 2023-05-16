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
        Managers.Scene.Get_CurrSceneType(SceneType);
        Managers.UI.popupCanvas.Play_FadeInEffect(Play_BGM);
    }
    
    public virtual void Clear()
    {
        
    }

    /// <summary>
    /// 재정의해서 사용. 재정의되지 않았을 경우 BGM을 끔
    /// </summary>
    public virtual void Play_BGM()
    {
        if(Managers.Scene.CurrSceneType != ENUM_SCENE_TYPE.Battle)
            Managers.Sound.Stop_BGM();
    }
}
 