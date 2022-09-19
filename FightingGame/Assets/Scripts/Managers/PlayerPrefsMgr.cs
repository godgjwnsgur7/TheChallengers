using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class SubPrefsType
{
    public int exist;
    public float size;
    public float opacity;
    public float resetSize;
    public float resetOpacity;
    public float transX;
    public float transY;
    public float resettransX;
    public float resettransY;
    public bool isInit;

    public void Init(int exist)
    {
        this.exist = exist;
        this.size = 50;
        this.opacity = 100;
        this.resetSize = 50;
        this.resetOpacity = 100;
        this.transX = 0;
        this.transY = 0;
        this.resettransX = 0;
        this.resettransY = 0;
        this.isInit = false;
    }

    // Set Value
    public void SetExist(int value)
    {
        this.exist = value;
    }

    public void SetSize(float value)
    {
        this.size = value;
    }

    public void SetOpacity(float value)
    {
        this.opacity = value;
    }

    public void SetResetSize(float value)
    {
        this.resetSize = value;
    }

    public void SetResetOpacity(float value)
    {
        this.resetOpacity = value;
    }

    public void SetTransX(float value)
    {
        this.transX = value;
    }

    public void SetTransY(float value)
    {
        this.transY = value;
    }

    public void SetResetTransX(float value)
    {
        this.resettransX = value;
    }

    public void SetResetTransY(float value)
    {
        this.resettransY = value;
    }

    public void SetIsInit(bool value)
    {
        this.isInit = value;
    }

    // Get Value
    public int GetExist()
    {
        return this.exist;
    }

    public float GetSize()
    {
        return this.size;
    }

    public float GetOpacity()
    {
        return this.opacity;
    }

    public float GetResetSize()
    {
        return this.resetSize;
    }

    public float GetResetOpacity()
    {
        return this.resetOpacity;
    }

    public float GetTransX()
    {
        return this.transX;
    }

    public float GetTransY()
    {
        return this.transY;
    }

    public float GetResetTransX()
    {
        return this.resettransX;
    }

    public float GetResetTransY()
    {
        return this.resettransY;
    }

    public bool GetIsInit()
    {
        return this.isInit;
    }
}

public class PlayerPrefsMgr
{
    ENUM_BTNPREFS_TYPE prefsType;
    SubPrefsType[] subPrefsList = new SubPrefsType[(int)ENUM_BTNPREFS_TYPE.Max];
    // 초기 값
    public void Init()
    {
        for (int i = 0; i < (int)ENUM_BTNPREFS_TYPE.Max; i++)
        {
            prefsType = (ENUM_BTNPREFS_TYPE)i;

            // 해당 prefs가 없을 경우 초기 값으로 저장
            if (!HasKey(prefsType, 0))
            {
                subPrefsList[i].Init(i);
            }
            else // 있으면 기존의 것을 불러옴
            {
                subPrefsList[i] = new SubPrefsType();

                subPrefsList[i].SetExist((int)GetButtonPrefs(prefsType, 0));
                subPrefsList[i].SetSize((float)GetButtonPrefs(prefsType, 1));
                subPrefsList[i].SetOpacity((float)GetButtonPrefs(prefsType, 2));
                subPrefsList[i].SetResetSize((float)GetButtonPrefs(prefsType, 3));
                subPrefsList[i].SetResetOpacity((float)GetButtonPrefs(prefsType, 4));
                subPrefsList[i].SetTransX((float)GetButtonPrefs(prefsType, 5));
                subPrefsList[i].SetTransY((float)GetButtonPrefs(prefsType, 6));
                subPrefsList[i].SetResetTransX((float)GetButtonPrefs(prefsType, 7));
                subPrefsList[i].SetResetTransY((float)GetButtonPrefs(prefsType, 8));
                subPrefsList[i].SetIsInit(true);
            }
        }

        SaveButtonPrefs();
    }

    // 현재 저장되어 있는 prefsList들을 저장
    public void SaveButtonPrefs()
    {
        for (int i = 0; i < (int)ENUM_BTNPREFS_TYPE.Max; i++)
        {
            prefsType = (ENUM_BTNPREFS_TYPE)i;

            PlayerPrefs.SetInt(prefsType + ".Exist", subPrefsList[i].GetExist());
            PlayerPrefs.SetFloat(prefsType + ".Size", subPrefsList[i].GetSize());
            PlayerPrefs.SetFloat(prefsType + ".Opacity", subPrefsList[i].GetOpacity());
            PlayerPrefs.SetFloat(prefsType + ".ResetSize", subPrefsList[i].GetResetSize());
            PlayerPrefs.SetFloat(prefsType + ".ResetOpacity", subPrefsList[i].GetResetOpacity());
            PlayerPrefs.SetFloat(prefsType + ".TransX", subPrefsList[i].GetTransX());
            PlayerPrefs.SetFloat(prefsType + ".TransY", subPrefsList[i].GetTransY());
            PlayerPrefs.SetFloat(prefsType + ".ResetTransX", subPrefsList[i].GetResetTransX());
            PlayerPrefs.SetFloat(prefsType + ".ResetTransY", subPrefsList[i].GetResetTransY());
        }

        PlayerPrefs.Save();
    }

    public object GetButtonPrefs(ENUM_BTNPREFS_TYPE prefsType, int subPrefsType)
    {
        switch (subPrefsType)
        {
            case 0:
                return PlayerPrefs.GetInt(prefsType + ".Exist");
            case 1:
                return PlayerPrefs.GetFloat(prefsType + ".Size");
            case 2:
                return PlayerPrefs.GetFloat(prefsType + ".Opacity");
            case 3:
                return PlayerPrefs.GetFloat(prefsType + ".ResetSize");
            case 4:
                return PlayerPrefs.GetFloat(prefsType + ".ResetOpacity");
            case 5:
                return PlayerPrefs.GetFloat(prefsType + ".TransX");
            case 6:
                return PlayerPrefs.GetFloat(prefsType + ".TransY");
            case 7:
                return PlayerPrefs.GetFloat(prefsType + ".ResetTransX");
            case 8:
                return PlayerPrefs.GetFloat(prefsType + ".ResetTransY");
            default:
                Debug.Log("범위 벗어남");
                return null;
        }
    }

    public SubPrefsType GetSubPrefsList(int prefsType)
    {
        return subPrefsList[prefsType];
    }

    public void SetSubPrefsList(SubPrefsType subPrefsList)
    {
        this.subPrefsList[subPrefsList.GetExist()] = subPrefsList;
    }

    // 해당 Prefs가 있는지 확인
    public bool HasKey(ENUM_BTNPREFS_TYPE ept, int subPrefsType)
    {
        switch (subPrefsType)
        {
            case 0:
                if (PlayerPrefs.HasKey(ept + ".Exist"))
                    return true;
                else
                    return false;
            case 1:
                if (PlayerPrefs.HasKey(ept + ".Size"))
                    return true;
                else
                    return false;
            case 2:
                if (PlayerPrefs.HasKey(ept + ".Opacity"))
                    return true;
                else
                    return false;
            case 3:
                if (PlayerPrefs.HasKey(ept + ".ResetSize"))
                    return true;
                else
                    return false;
            case 4:
                if (PlayerPrefs.HasKey(ept + ".ResetOpacity"))
                    return true;
                else
                    return false;
            case 5:
                if (PlayerPrefs.HasKey(ept + ".TransX"))
                    return true;
                else
                    return false;
            case 6:
                if (PlayerPrefs.HasKey(ept + ".TransY"))
                    return true;
                else
                    return false;
            case 7:
                if (PlayerPrefs.HasKey(ept + ".ResetTransX"))
                    return true;
                else
                    return false;
            case 8:
                if (PlayerPrefs.HasKey(ept + ".ResetTransY"))
                    return true;
                else
                    return false;
            default:
                Debug.Log("범위 벗어남");
                return false;
        }
    }
}
