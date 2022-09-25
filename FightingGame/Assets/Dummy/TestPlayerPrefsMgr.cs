/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TestPlayerPrefsMgr
{
    ENUM_BTNPREFS_TYPE prefsType;
    float[][] prefsList = new float[(int)ENUM_BTNPREFS_TYPE.Max][];

    // 초기 값
    public void Init()
    {
        for (int i = 0; i < (int)ENUM_BTNPREFS_TYPE.Max; i++)
        {
            prefsType = (ENUM_BTNPREFS_TYPE)i;

            // 해당 prefs가 없을 경우 초기 값으로 저장
            if (!HasKey(prefsType, ENUM_BTNSUBPREFS_TYPE.Exist))
            {
                prefsList[i] = new float[(int)ENUM_BTNSUBPREFS_TYPE.Max] { i, 50, 100, 50, 100, 0, 0, 0, 0, 0 };

                SetPrefsList(prefsType, prefsList[i]);
            }
            else // 있으면 기존의 것을 불러옴
            {
                prefsList[i] = new float[(int)ENUM_BTNSUBPREFS_TYPE.Max];

                for (int j = 0; j < (int)ENUM_BTNSUBPREFS_TYPE.Max; j++)
                    prefsList[i][j] = GetButtonPrefs(i, j);
            }
        }

        SaveButtonPrefs();
    }

    // 해당 Prefs가 있는지 확인
    public bool HasKey(ENUM_BTNPREFS_TYPE ept, ENUM_BTNSUBPREFS_TYPE subPrefsType)
    {
        if (PlayerPrefs.HasKey(ept + "." + subPrefsType))
            return true;
        else
            return false;
    }

    // 특정 Prefs값을 담기
    public void SetButtonPrefs(ENUM_BTNPREFS_TYPE prefsType, ENUM_BTNSUBPREFS_TYPE subPrefsType, float value)
    {
        prefsList[(int)prefsType][(int)subPrefsType] = value;
    }
    public void SetButtonPrefs(int prefsType, int subPrefsType, float value)
    {
        prefsList[prefsType][subPrefsType] = value;
    }

    // 받은 리스트를 prefsList에 담기
    public void SetPrefsList(ENUM_BTNPREFS_TYPE prefsType, float[] prefsList)
    {
        this.prefsList[(int)prefsType] = prefsList;
    }

    // 현재 저장되어 있는 prefsList들을 저장
    public void SaveButtonPrefs()
    {
        for (int i = 0; i < (int)ENUM_BTNPREFS_TYPE.Max; i++)
            for (int j = 0; j < (int)ENUM_BTNSUBPREFS_TYPE.Max; j++)
                PlayerPrefs.SetFloat((ENUM_BTNPREFS_TYPE)i + "." + (ENUM_BTNSUBPREFS_TYPE)j, prefsList[i][j]);

        PlayerPrefs.Save();
    }

    // prefsList 호출
    public float[] GetPrefsList(ENUM_BTNPREFS_TYPE prefsType)
    {
        return prefsList[(int)prefsType];
    }

    // 특정 Prefs값 호출
    public float GetButtonPrefs(int prefsType, int subPrefsType)
    {
        return PlayerPrefs.GetFloat((ENUM_BTNPREFS_TYPE)prefsType + "." + (ENUM_BTNSUBPREFS_TYPE)subPrefsType);
    }
    public float GetButtonPrefs(ENUM_BTNPREFS_TYPE prefsType, ENUM_BTNSUBPREFS_TYPE subPrefsType)
    {
        return PlayerPrefs.GetFloat(prefsType + "." + subPrefsType);
    }
}*/