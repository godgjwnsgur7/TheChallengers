using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FGDefine;

public abstract class BaseScene : MonoBehaviour
{
    private ENUM_SCENE_TYPE sceneType;
    public ENUM_SCENE_TYPE SceneType { get; protected set; } = ENUM_SCENE_TYPE.Unknown;

    protected IEnumerator Start()
    {
        Debug.Log("확인");
        yield return null;
        Init();
    }

    public virtual void Init()
    {
       Managers.Sound.Change_ObserverScene(GetComponent<BaseScene>());

       Managers.UI.popupCanvas.Play_FadeOutEffect();
    }

    public abstract void Clear();
}
 