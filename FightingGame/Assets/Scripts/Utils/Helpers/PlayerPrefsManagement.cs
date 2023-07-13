using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class KeySettingData
{
    public List<KeySettingDataElement> keySettingDataList = new List<KeySettingDataElement>();
    public float opacity;
    
    public KeySettingData(List<KeySettingDataElement> _keySettingDataList, float _opacity)
    {
        keySettingDataList = _keySettingDataList;
        opacity = _opacity;
    }
}

public class KeySettingDataElement
{
    public int key; // ENUM_KEYSETTING_NAME의 번호 (키 값)
    public float scaleSize;
    public float rectTrX;
    public float rectTrY;

    public KeySettingDataElement(int _key, float scaleSize, float _rectTrX, float _rectTrY)
    {
        this.key = _key;
        this.scaleSize = scaleSize;
        this.rectTrX = _rectTrX;
        this.rectTrY = _rectTrY;
    }
}

public class VolumeData
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public bool isBgmMute;
    public bool isSfxMute;

    public VolumeData(float _masterVolume, float _bgmVolume, float _sfxVolume
        , bool _isBgmMute, bool _isSfxMute)
    {
        masterVolume = _masterVolume;
        bgmVolume = _bgmVolume;
        sfxVolume = _sfxVolume;
        isBgmMute = _isBgmMute;
        isSfxMute = _isSfxMute;
    }
}

public class PlayerPrefsManagement : MonoBehaviour
{
    #region PlayerPrefs_SetGet
    // PlayerPrefs Set 계열 함수들. keyName_keyType의 이름으로 저장함
    private static void Set_Float(float value, string keyName, string keyType)
        => PlayerPrefs.SetFloat($"{keyName}_{keyType}", value);
    private static void Set_String(string value, string keyName, string keyType)
        => PlayerPrefs.SetString($"{keyName}_{keyType}", value);
    private static void Set_Int(int value, string keyName, string keyType)
        => PlayerPrefs.SetInt($"{keyName}_{keyType}", value);

    // PlayerPrefs Get 계열 함수들. keyName_keyType의 이름으로 불러옴
    private static float Get_Float(string keyName, string keyType)
        => PlayerPrefs.GetFloat($"{keyName}_{keyType}");
    private static string Get_String(string keyName, string keyType)
        => PlayerPrefs.GetString($"{keyName}_{keyType}");
    private static int Get_Int(string keyName, string keyType)
        => PlayerPrefs.GetInt($"{keyName}_{keyType}");

    // PlayerPrefs Delect Key 함수. 저장된 keyName_keyType의 이름으로 삭제 
    private static void Delete_Key(string keyName, string keyType)
    {
        PlayerPrefs.DeleteKey($"{keyName}_{keyType}");
    }
    #endregion

    public static void DeleteDataAll()
    {
        PlayerPrefs.DeleteAll();
    }

    #region KeySettingData
    /// <summary>
    /// KeySettingData가 null이면 저장하지 않고, false를 리턴
    /// </summary>
    public static bool Save_KeySettingData(KeySettingData _keySettingData)
    {
        if (_keySettingData == null || _keySettingData.keySettingDataList.Count != (int)ENUM_INPUTKEY_NAME.Max)
        {
            Debug.Log("keySettingDatas가 Null이거나 키 전체가 넘어오지 않았습니다.");
            return false;
        }

        for (int i = 0; i < _keySettingData.keySettingDataList.Count; i++)
        {
            string keyName = Enum.GetName(typeof(ENUM_INPUTKEY_NAME), _keySettingData.keySettingDataList[i].key);
            if (keyName == null)
            {
                Debug.Log($"keySettingDatas의 {i}번째 인자의 key Name가 없습니다.");
                return false;
            }

            Set_Float(_keySettingData.keySettingDataList[i].scaleSize, keyName, nameof(KeySettingDataElement.scaleSize));
            Set_Float(_keySettingData.keySettingDataList[i].rectTrX, keyName, nameof(KeySettingDataElement.rectTrX));
            Set_Float(_keySettingData.keySettingDataList[i].rectTrY, keyName, nameof(KeySettingDataElement.rectTrY));
        }

        Set_Float(_keySettingData.opacity, "InputKey", "Opacity");

        PlayerPrefs.Save();
        return true;
    }

    /// <summary>
    /// 만약 저장된 값이 없다면 null을 리턴
    /// </summary>
    public static KeySettingData Load_KeySettingData()
    {
        List<KeySettingDataElement> keySettingDataList = new List<KeySettingDataElement>();
        float _opacity = Get_Float("InputKey", "Opacity");

        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            string inputKeyName = Enum.GetName(typeof(ENUM_INPUTKEY_NAME), i);
            if(inputKeyName == null || !PlayerPrefs.HasKey($"{inputKeyName}_{nameof(KeySettingDataElement.scaleSize)}"))
            {
                Debug.Log($"inputKeyName이 NUll이거나 저장된 {i}번째 키가 없습니다.");
                return null;
            }

            float _scaleSize = Get_Float(inputKeyName, nameof(KeySettingDataElement.scaleSize));
            float _rectTrX = Get_Float(inputKeyName, nameof(KeySettingDataElement.rectTrX));
            float _rectTrY = Get_Float(inputKeyName, nameof(KeySettingDataElement.rectTrY));

            KeySettingDataElement keySettingDataElement = new KeySettingDataElement(i, _scaleSize, _rectTrX, _rectTrY);
            keySettingDataList.Add(keySettingDataElement);
        }

        KeySettingData keySettingData = new KeySettingData(keySettingDataList, _opacity); ;

        return keySettingData;
     }

    public static void Delete_KetSettingData()
    {
        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            string inputKeyName = Enum.GetName(typeof(ENUM_INPUTKEY_NAME), i);
            if (inputKeyName == null || !PlayerPrefs.HasKey($"{inputKeyName}_{nameof(KeySettingDataElement.scaleSize)}"))
            {
                Debug.Log($"inputKeyName이 NUll이거나 저장된 {i}번째 키가 없습니다.");
                return ;
            }

            Delete_Key(inputKeyName, nameof(KeySettingDataElement.scaleSize));
            Delete_Key(inputKeyName, nameof(KeySettingDataElement.rectTrX));
            Delete_Key(inputKeyName, nameof(KeySettingDataElement.rectTrY));
        }
        Delete_Key("InputKey", nameof(KeySettingData.opacity));
    }
    #endregion

    public static bool Save_VolumeData(VolumeData volumeData)
    {
        if(volumeData == null)
        {
            Debug.Log("soundDatas Null입니다.");
            return false;
        }

        Set_Float(volumeData.masterVolume, "Volume", "Master");
        Set_Float(volumeData.masterVolume, "Volume", "Bgm");
        Set_Float(volumeData.masterVolume, "Volume", "Sfx");
        Set_Int(volumeData.isBgmMute ? 1 : 0, "Mute", "Bgm");
        Set_Int(volumeData.isSfxMute ? 1 : 0, "Mute", "Sfx");

        PlayerPrefs.Save();
        Managers.Sound.Update_VolumeData(volumeData);
        return true;
    }

    public static VolumeData Load_VolumeData()
    {
        // 만약, 저장된 데이터가 없다면 기본 값으로 저장 후 리턴
        if(!PlayerPrefs.HasKey("Volume_Master"))
        {
            VolumeData tempVolumeData = new VolumeData(1.0f, 0.5f, 1.0f, false, false); // 기본 값
            Save_VolumeData(tempVolumeData); 
            return tempVolumeData;
        }

        float _wholeVolume = Get_Float("Volume", "Master");
        float _bgmVolume = Get_Float("Volume", "Bgm");
        float _sfxVolume = Get_Float("Volume", "Sfx");
        
        bool _isBgmMute = Get_Int("Mute", "Bgm") == 1;
        bool _isSfxMute = Get_Int("Mute", "Sfx") == 1;

        VolumeData volumeData = new VolumeData(_wholeVolume, _bgmVolume, _sfxVolume, _isBgmMute, _isSfxMute);

        return volumeData;
    }
}
