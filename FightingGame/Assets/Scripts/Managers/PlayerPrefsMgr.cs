using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;


// 얘는 수정할 예정
[Serializable]
public enum ENUM_BTNPREFS_TYPE
{
    LeftMoveBtn = 0,
    RightMoveBtn = 1,
    AttackBtn = 2,
    JumpBtn = 3,
    SkillBtn1 = 4,
    SkillBtn2 = 5,
    SkillBtn3 = 6,

    Max
}

public enum ENUM_PLAYERPREFS_TYPE
{
    // 이 데이터를 어떻게 받을지에 따라 틀림
}

public class SubPrefsType // PlayerPrefsData 같은걸로 변경 원합니다.
{
    public float size;
    public float opacity;
    public float transX;
    public float transY;

    // 얘도 이름 변경
    public SubPrefsType(float _size, float _opacity, Vector2 _transformPosition)
    {
        size = _size;
        opacity = _opacity;
        transX = _transformPosition.x;
        transY = _transformPosition.y;
    }

    /*
    public void Init(int exist) // 바꾸시고
    {
        this.exist = exist;
        this.size = 50;
        this.opacity = 100;
        this.resetSize = 50;
        this.resetOpacity = 100;
        this.isInit = false;
    }
    */

}

// 1번째, 모두다 묶어서 받고 묶어서 리턴시킬지,
// 2번째, 하나씩 묶어서 받고 ( 최소 단위 : SubPrefsType )

public class PlayerPrefsManagement
{

    // 타입을 받아서 스트링으로 변환해서 정보를 저장, 받아오기를 할 아이

   

    /*
    ENUM_BTNPREFS_TYPE prefsType;
    SubPrefsType[] subPrefsList = new SubPrefsType[(int)ENUM_BTNPREFS_TYPE.Max];
    // 초기 값
    public void Init()
    {
        for (int i = 0; i < (int)ENUM_BTNPREFS_TYPE.Max; i++)
        {
            prefsType = (ENUM_BTNPREFS_TYPE)i;
            subPrefsList[i] = new SubPrefsType();

            // 해당 prefs가 없을 경우 초기 값으로 저장
            if (!HasKey(prefsType, 0))
            {
                subPrefsList[i].Init(i);
            }
            else // 있으면 기존의 것을 불러옴
            {
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

        SaveButtonPrefsAll();
    }

    // 현재 저장되어 있는 prefsList들을 저장
    public void SaveButtonPrefsAll()
    {
        for (int i = 0; i < (int)ENUM_BTNPREFS_TYPE.Max; i++)
        {
            SaveButtonPrefs(i);
        }

        PlayerPrefs.Save();
    }

    public void SaveButtonPrefs(int prefsType)
    {
        this.prefsType = (ENUM_BTNPREFS_TYPE)prefsType;

        PlayerPrefs.SetInt(prefsType + ".Exist", subPrefsList[prefsType].GetExist());
        PlayerPrefs.SetFloat(prefsType + ".Size", subPrefsList[prefsType].GetSize());
        PlayerPrefs.SetFloat(prefsType + ".Opacity", subPrefsList[prefsType].GetOpacity());
        PlayerPrefs.SetFloat(prefsType + ".ResetSize", subPrefsList[prefsType].GetResetSize());
        PlayerPrefs.SetFloat(prefsType + ".ResetOpacity", subPrefsList[prefsType].GetResetOpacity());
        PlayerPrefs.SetFloat(prefsType + ".TransX", subPrefsList[prefsType].GetTransX());
        PlayerPrefs.SetFloat(prefsType + ".TransY", subPrefsList[prefsType].GetTransY());
        PlayerPrefs.SetFloat(prefsType + ".ResetTransX", subPrefsList[prefsType].GetResetTransX());
        PlayerPrefs.SetFloat(prefsType + ".ResetTransY", subPrefsList[prefsType].GetResetTransY());
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
    */
}
