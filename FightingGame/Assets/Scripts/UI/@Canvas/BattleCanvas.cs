using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] InteractableUI interactableUI;
    
    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(InteractableUI)) interactableUI.Open(param);
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(InteractableUI)) interactableUI.Close();
        else Debug.Log("범위 벗어남");
    }
}