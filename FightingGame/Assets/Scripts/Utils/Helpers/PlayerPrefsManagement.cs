using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class KeySettingData
{
    public int key; // ENUM_KEYSETTING_NAME의 번호 (키 값)
    public float size;
    public float opacity;
    public float rectTrX;
    public float rectTrY;

    public KeySettingData(int _key, float _size, float _opacity, float _rectTrX, float _rectTrY)
    {
        key = _key;
        size = _size;
        opacity = _opacity;
        rectTrX = _rectTrX;
        rectTrY = _rectTrY;
    }
}

public class PlayerPrefsManagement : MonoBehaviour
{
    #region KeySettingData (가급적 건들지 마시오!)

    // PlayerPrefs Set 계열 함수들. keyName_keyType 을 키값으로 저장함
    private static void Set_Float(float value, string keyName, string keyType)
        => PlayerPrefs.SetFloat($"{keyName}_{keyType}", value);
    private static void Set_String(string value, string keyName, string keyType)
        => PlayerPrefs.SetString($"{keyName}_{keyType}", value);
    private static void Set_Int(int value, string keyName, string keyType)
        => PlayerPrefs.SetInt($"{keyName}_{keyType}", value);

    // PlayerPrefs Get 계열 함수들. keyName_keyType 을 키값으로 불러옴
    private static float Get_Float(string keyName, string keyType)
    { return PlayerPrefs.GetFloat($"{keyName}_{keyType}"); }
    private static string Get_String(string keyName, string keyType)
    { return PlayerPrefs.GetString($"{keyName}_{keyType}"); }
    private static int Get_Int(string keyName, string keyType)
    { return PlayerPrefs.GetInt($"{keyName}_{keyType}"); }

    #endregion

    /// <summary>
    /// KeySettingData가 null이면 저장하지 않고, false를 리턴
    /// </summary>
    public static bool Save_KeySettingData(List<KeySettingData> keySettingDatas)
    {
        if (keySettingDatas == null || keySettingDatas.Count != Enum.GetValues(typeof(ENUM_INPUTKEY_NAME)).Length)
        {
            Debug.Log("keySettingDatas가 Null이거나 키 전체가 넘어오지 않았습니다.");
            return false;
        }

        for (int i = 0; i < keySettingDatas.Count; i++)
        {
            string keyName = Enum.GetName(typeof(ENUM_INPUTKEY_NAME), keySettingDatas[i].key);
            if (keyName == null)
            {
                Debug.Log($"keySettingDatas의 {i}번째 인자의 key Name가 없습니다.");
                return false;
            }

            Set_Float(keySettingDatas[i].size, nameof(KeySettingData.size), keyName);
            Set_Float(keySettingDatas[i].opacity, nameof(KeySettingData.opacity), keyName);
            Set_Float(keySettingDatas[i].rectTrX, nameof(KeySettingData.rectTrX), keyName);
            Set_Float(keySettingDatas[i].rectTrY, nameof(KeySettingData.rectTrY), keyName);
        }

        PlayerPrefs.Save();
        return true;
    }

    /// <summary>
    /// 만약 저장된 값이 없다면 null을 리턴
    /// </summary>
    public static List<KeySettingData> Load_KeySettingData()
    {
        List<KeySettingData> keySettingDatas = new List<KeySettingData>();

        for (int i = 0; i < Enum.GetValues(typeof(ENUM_INPUTKEY_NAME)).Length; i++)
        {
            string inputKeyName = Enum.GetName(typeof(ENUM_INPUTKEY_NAME), i);
            if(inputKeyName == null || !PlayerPrefs.HasKey($"{inputKeyName}_{nameof(KeySettingData.size)}"))
            {
                Debug.Log($"inputKeyName이 NUll이거나 저장된 {i}번째 키가 없습니다.");
                return null;
            }

            float _size = Get_Float(inputKeyName, nameof(KeySettingData.size));
            float _opacity = Get_Float(inputKeyName, nameof(KeySettingData.opacity));
            float _rectTrX = Get_Float(inputKeyName, nameof(KeySettingData.rectTrX));
            float _rectTrY = Get_Float(inputKeyName, nameof(KeySettingData.rectTrY));

            keySettingDatas.Add(new KeySettingData(i, _size, _opacity, _rectTrX, _rectTrY));
        }

        return keySettingDatas;
     }
}
