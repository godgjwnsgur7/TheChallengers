using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null) component = go.AddComponent<T>();
        return component;
    }
}

/// <summary>
/// 확장 메서드를 위한 스태틱 클래스
/// </summary>
public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }
}