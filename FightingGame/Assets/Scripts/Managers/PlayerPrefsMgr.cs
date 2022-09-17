using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class PlayerPrefsMgr
{
    ENUM_BTNPREFS_TYPE prefsType;
    float[][] prefsList = new float[(int)ENUM_BTNPREFS_TYPE.Max][];

    public void init()
    {
        for (int i = 0; i < (int)ENUM_BTNPREFS_TYPE.Max; i++)
        {
            prefsType = (ENUM_BTNPREFS_TYPE)i;

            if (!HasKey(prefsType, ENUM_BTNSUBPREFS_TYPE.Exist))
            {
                prefsList[i] = new float[(int)ENUM_BTNSUBPREFS_TYPE.Max] { i, 50, 100, 50, 100, 0, 0 };

                SetButtonPrefs(prefsType, prefsList[i]);
            }
            else
            {
                prefsList[i] = new float[(int)ENUM_BTNSUBPREFS_TYPE.Max];

                for (int j = 0; j < (int)ENUM_BTNSUBPREFS_TYPE.Max; j++)
                    prefsList[i][j] = GetButtonPrefs(i, j);
            }
        }
    }

    public bool HasKey(ENUM_BTNPREFS_TYPE ept, ENUM_BTNSUBPREFS_TYPE subPrefsType)
    {
        if (PlayerPrefs.HasKey(ept + "." + subPrefsType))
            return true;
        else
            return false;
    }

    public void SetButtonPrefs(ENUM_BTNPREFS_TYPE prefsType, ENUM_BTNSUBPREFS_TYPE subPrefsType, float value)
    {
        prefsList[(int)prefsType][(int)subPrefsType] = value;
    }

    public void SetButtonPrefs(ENUM_BTNPREFS_TYPE prefsType, float[] prefsList)
    {
        this.prefsList[(int)prefsType] = prefsList;
    }

    public void SaveButtonPrefs()
    {
        for(int i = 0; i < (int)ENUM_BTNPREFS_TYPE.Max; i++)
            for(int j = 0; j < (int)ENUM_BTNSUBPREFS_TYPE.Max; j++)
                PlayerPrefs.SetFloat((ENUM_BTNPREFS_TYPE)i + "." + (ENUM_BTNSUBPREFS_TYPE)j, prefsList[i][j]);

        PlayerPrefs.Save();
    }

    public float[] GetButtonPrefs(ENUM_BTNPREFS_TYPE prefsType)
    {
        return prefsList[(int)prefsType];
    }

    public float GetButtonPrefs(int prefsType, int subPrefsType)
    {
        return PlayerPrefs.GetFloat((ENUM_BTNPREFS_TYPE)prefsType + "." + (ENUM_BTNSUBPREFS_TYPE)subPrefsType);
    }
}
