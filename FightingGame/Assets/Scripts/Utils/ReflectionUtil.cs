using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectionUtil
{
    public static List<Type> GetTypesInEnum<T>() where T : Enum
    {
        List<Type> enumTypes = new List<Type>();

        Type enumType = typeof(T);

        string[] enumMemberNames = Enum.GetNames(enumType);
        
        foreach(string name in enumMemberNames)
        {
            Type t = Type.GetType(name);
            
            if(enumType != null)
            {
                enumTypes.Add(t);
            }
        }

        return enumTypes;
    }

    public static T ConvertToEnum<T>(string name) where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), name))
            Debug.LogError($"{name}이 {typeof(T)}에현재 정의되어 있지 않음");

        object obj = Enum.Parse(typeof(T), name);

        return (T)obj;
    }

    public static T ConvertToEnum<T>(Type type) where T : Enum
    {
        return ConvertToEnum<T>(type.ToString());
    }

    public static string ConvertToString<T>(T state) where T : Enum
    {
        return Enum.GetName(typeof(T), state);
    }
}
