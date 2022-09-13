using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class PlayerPrefsMgr
{
    // Default, Size, Opacity, resetSize, resetOpacity, transX, transY
    public object[] initValue;

    public void SetInitValue(object[] value)
    {
        if (value.Length > (int)ENUM_PLAYERPREFS_TYPE.Max)
            return;

        initValue = new object[(int)ENUM_PLAYERPREFS_TYPE.Max];

        for (int i = 0; i < value.Length; i++)
            initValue[i] = value[i];
    }

    public void SetPrefsAll(string name = "")
    {
        if (initValue == null)
            return;

        for(int i = 1; i < (int)ENUM_PLAYERPREFS_TYPE.Max; i++)
        {
            if (!Managers.Prefs.HasKey((ENUM_PLAYERPREFS_TYPE)i, name))
                Managers.Prefs.SetPlayerPrefs<object>(initValue[i], (ENUM_PLAYERPREFS_TYPE)i, name);
        }
    }

    public void SetPlayerPrefs<T>(T value, ENUM_PLAYERPREFS_TYPE ept, string name = "")
    {
        if (value.GetType() == typeof(string))
            PlayerPrefs.SetString($"{ept + name}", value.ToString());
        else if (value.GetType() == typeof(int))
            PlayerPrefs.SetInt($"{ept + name}", int.Parse(value.ToString()));
        else if (value.GetType() == typeof(float))
        {
            Debug.Log($"{ept + name}");
            PlayerPrefs.SetFloat($"{ept + name}", float.Parse(value.ToString()));
        }
        else
            Debug.Log("값 범위 벗어남");
    }

    public float GetPrefsFloat(ENUM_PLAYERPREFS_TYPE ept, string name = "")
    {
        if (!PlayerPrefs.HasKey($"{ept + name}"))
        {
            Debug.Log($"{ept + name} : Prefs가 없음");
        }

        return PlayerPrefs.GetFloat($"{ept + name}");
    }

    public string GetPrefsString(ENUM_PLAYERPREFS_TYPE ept, string name = "")
    {
        if (!PlayerPrefs.HasKey($"{ept + name}"))
        {
            Debug.Log($"{ept + name} : Prefs가 없음");
        }

        return PlayerPrefs.GetString($"{ept + name}");
    }

    public int GetPrefsInt(ENUM_PLAYERPREFS_TYPE ept, string name = "")
    {
        if (!PlayerPrefs.HasKey($"{ept + name}"))
        {
            Debug.Log($"{ept + name} : Prefs가 없음");
        }

        return PlayerPrefs.GetInt($"{ept + name}");
    }

    public bool HasKey(ENUM_PLAYERPREFS_TYPE ept, string name = "")
    {
        if (PlayerPrefs.HasKey($"{ept + name}"))
            return true;
        else
            return false;
    }
}
