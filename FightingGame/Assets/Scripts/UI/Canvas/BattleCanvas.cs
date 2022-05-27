using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] TestText testText;
    
    public override void Open<T>()
    {
        if (typeof(T) == typeof(TestText)) testText.Open();
        else Debug.Log("범위 벗어남");
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(TestText)) testText.Close();
        else Debug.Log("범위 벗어남");
    }
}