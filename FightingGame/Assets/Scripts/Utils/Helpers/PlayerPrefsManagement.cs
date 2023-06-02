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

public class VolumeData
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public bool isBgmMute;
    public bool isSfxMute;

    public bool isVibration;

    public VolumeData(float _masterVolume, float _bgmVolume, float _sfxVolume
        , bool _isBgmMute, bool _isSfxMute, bool _isVibration)
    {
        masterVolume = _masterVolume;
        bgmVolume = _bgmVolume;
        sfxVolume = _sfxVolume;
        isBgmMute = _isBgmMute;
        isSfxMute = _isSfxMute;
        isVibration = _isVibration;
    }
}

public class PlayerPrefsManagement : MonoBehaviour
{
    #region PlayerPrefs_SetGet (가급적 건들지 마시오!)
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

    #region KeySettingData
    /// <summary>
    /// KeySettingData가 null이면 저장하지 않고, false를 리턴
    /// </summary>
    public static bool Save_KeySettingData(List<KeySettingData> keySettingDatas)
    {
        if (keySettingDatas == null || keySettingDatas.Count != (int)ENUM_INPUTKEY_NAME.Max)
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

            Set_Float(keySettingDatas[i].size, keyName, nameof(KeySettingData.size));
            Set_Float(keySettingDatas[i].opacity, keyName, nameof(KeySettingData.opacity));
            Set_Float(keySettingDatas[i].rectTrX, keyName, nameof(KeySettingData.rectTrX));
            Set_Float(keySettingDatas[i].rectTrY, keyName, nameof(KeySettingData.rectTrY));
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

        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
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

    public static void Delete_KetSettingData()
    {
        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            string inputKeyName = Enum.GetName(typeof(ENUM_INPUTKEY_NAME), i);
            if (inputKeyName == null || !PlayerPrefs.HasKey($"{inputKeyName}_{nameof(KeySettingData.size)}"))
            {
                Debug.Log($"inputKeyName이 NUll이거나 저장된 {i}번째 키가 없습니다.");
                return ;
            }

            PlayerPrefs.DeleteKey($"{inputKeyName}_{nameof(KeySettingData.size)}");
            PlayerPrefs.DeleteKey($"{inputKeyName}_{nameof(KeySettingData.opacity)}");
            PlayerPrefs.DeleteKey($"{inputKeyName}_{nameof(KeySettingData.rectTrX)}");
            PlayerPrefs.DeleteKey($"{inputKeyName}_{nameof(KeySettingData.rectTrY)}");
        }
    }
    #endregion

    public static bool Save_VolumeData(VolumeData volumeData)
    {
        if(volumeData == null)
        {
            Debug.Log("soundDatas Null입니다.");
            return false;
        }

        PlayerPrefs.SetFloat("Master_Volume", volumeData.masterVolume);
        PlayerPrefs.SetFloat("BGM_Volume", volumeData.bgmVolume);
        PlayerPrefs.SetFloat("SFX_Volume", volumeData.sfxVolume);
        PlayerPrefs.SetInt("BGM_Mute", volumeData.isBgmMute ? 1 : 0);
        PlayerPrefs.SetInt("SFX_Mute", volumeData.isSfxMute ? 1 : 0);
        PlayerPrefs.SetInt("Is_Vibration", volumeData.isVibration ? 1 : 0);

        PlayerPrefs.Save();
        Managers.Sound.Update_VolumeData(volumeData);
        return true;
    }

    public static VolumeData Load_VolumeData()
    {
        // 만약, 저장된 데이터가 없다면 기본 값으로 저장 후 리턴
        if(!PlayerPrefs.HasKey("Master_Volume"))
        {
            VolumeData tempVolumeData = new VolumeData(0.5f, 0.5f, 1.0f, false, false, true); // 기본 값
            Save_VolumeData(tempVolumeData); 
            return tempVolumeData;
        }

        float _wholeVolume = PlayerPrefs.GetFloat("Master_Volume");
        float _bgmVolume = PlayerPrefs.GetFloat("BGM_Volume");
        float _sfxVolume = PlayerPrefs.GetFloat("SFX_Volume");
        bool _isBgmMute = PlayerPrefs.GetInt("BGM_Mute") == 1;
        bool _isSfxMute = PlayerPrefs.GetInt("SFX_Mute") == 1;
        bool _isVibration = PlayerPrefs.GetInt("Is_Vibration") == 1;

        VolumeData volumeData = new VolumeData(_wholeVolume, _bgmVolume, _sfxVolume, _isBgmMute, _isSfxMute, _isVibration);

        return volumeData;
    }
}
