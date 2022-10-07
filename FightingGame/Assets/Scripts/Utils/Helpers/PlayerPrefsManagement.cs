using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

// 얘는 수정할 예정
[Serializable]
public enum ENUM_KEYSETTING_NAME
{
    LeftArrow = 0,
    RightArrow = 1,
    Attack = 2,
    Jump = 3,
    Skill1 = 4,
    Skill2 = 5,
    Skill3 = 6,
    
    Max
}

public class KeySettingData
{
    public float size;
    public float opacity;
    public float rectTrX;
    public float rectTrY;

    public KeySettingData(float _size, float _opacity, float _rectTrX, float _rectTrY)
    {
        size = _size;
        opacity = _opacity;
        rectTrX = _rectTrX;
        rectTrY = _rectTrY;
    }
}

 public class PlayerPrefsManagement : MonoBehaviour
{
    // PlayerPrefs Set 계열 함수들. keyName_keyType 을 키값으로 저장함
    private void Set_Float(float value, string keyName, string keyType)
        => PlayerPrefs.SetFloat($"{keyName}_{keyType}", value);
    private void Set_String(string value, string keyName, string keyType)
        => PlayerPrefs.SetString($"{keyName}_{keyType}", value);
    private void Set_Int(int value, string keyName, string keyType)
        => PlayerPrefs.SetInt($"{keyName}_{keyType}", value);

    // PlayerPrefs Get 계열 함수들. keyName_keyType 을 키값으로 불러옴
    private float Get_Float(string keyName, string keyType)
    { return PlayerPrefs.GetFloat($"{keyName}_{keyType}"); }
    private string Get_String(string keyName, string keyType)
    { return PlayerPrefs.GetString($"{keyName}_{keyType}"); }
    private int Get_Int(string keyName, string keyType)
    { return PlayerPrefs.GetInt($"{keyName}_{keyType}"); }


    #region KeySettingData

    /// <summary>
    /// KeySettingData가 null이면 저장하지 않고, false를 리턴
    /// </summary>
    public bool Set_KeySettingData(KeySettingData keySettingData, ENUM_KEYSETTING_NAME keyName)
    {
        if (keySettingData == null)
            return false;

        Set_Float(keySettingData.size, nameof(keySettingData.size), keyName.ToString());
        Set_Float(keySettingData.opacity, nameof(keySettingData.opacity), keyName.ToString());
        Set_Float(keySettingData.rectTrX, nameof(keySettingData.rectTrX), keyName.ToString());
        Set_Float(keySettingData.rectTrY, nameof(keySettingData.rectTrY), keyName.ToString());

        return true;
    }

    /// <summary>
    /// 만약 저장된 값이 없다면 null을 리턴
    /// </summary>
    public KeySettingData Get_KeySettingData(ENUM_KEYSETTING_NAME keyName)
    {
        if(PlayerPrefs.HasKey($"{keyName}_{nameof(KeySettingData.size)}"))
            return null;

        float _size = Get_Float(keyName.ToString(), nameof(KeySettingData.size));
        float _opacity = Get_Float(keyName.ToString(), nameof(KeySettingData.opacity));
        float _rectTrX = Get_Float(keyName.ToString(), nameof(KeySettingData.rectTrX));
        float _rectTrY = Get_Float(keyName.ToString(), nameof(KeySettingData.rectTrY));

        return new KeySettingData(_size, _opacity, _rectTrX, _rectTrY);
    }

    #endregion

    public void Save_KeySettingData()
    {
        PlayerPrefs.Save();
    }
}
