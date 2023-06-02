using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class InputPanel : MonoBehaviour
{
    InputKey[] inputKeys = new InputKey[(int)ENUM_INPUTKEY_NAME.Max];

    public void Init(Action<ENUM_INPUTKEY_NAME> OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> OnPointUpCallBack)
    {
        List<KeySettingData> keySettingDatas = PlayerPrefsManagement.Load_KeySettingData();

        for (int index = 0; index < inputKeys.Length; index++)
        {
            inputKeys[index] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)).GetComponent<InputKey>();
            
            if(inputKeys[index] == null)
            {
                Debug.LogError($"{Enum.GetName(typeof(ENUM_INPUTKEY_NAME), index)} 를 찾지 못했습니다.");
                return;
            }

            inputKeys[index].Init(OnPointDownCallBack, OnPointUpCallBack);

            if (keySettingDatas != null)
                Set_InputKey(inputKeys[index], keySettingDatas[index]);
        }
    }

    private void Set_InputKey(InputKey inputKey, KeySettingData keySettingData)
    {
        inputKey.rectTr.localScale = new Vector3(keySettingData.size, keySettingData.size, 1f);

        inputKey.Set_Transparency(keySettingData.opacity);

        inputKey.rectTr.position = new Vector2(keySettingData.rectTrX, keySettingData.rectTrY);
    }
    
    public void Set_InputSkillKeys(ENUM_CHARACTER_TYPE charType)
    {
        Managers.Data.CharInfoDict.TryGetValue((int)charType, out CharacterInfo charInfo);

        InputSkillKey inputSkillKey;

        inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Dash] as InputSkillKey;
        inputSkillKey.Set_SkillInfo(charInfo.skillCoolTime_Dash, charType, 0);

        inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Skill1] as InputSkillKey;
        inputSkillKey.Set_SkillInfo(charInfo.skillCoolTime_1, charType, 1);

        inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Skill2] as InputSkillKey;
        inputSkillKey.Set_SkillInfo(charInfo.skillCoolTime_2, charType, 2);

        inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Skill3] as InputSkillKey;
        inputSkillKey.Set_SkillInfo(charInfo.skillCoolTime_3, charType, 3);

        inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Skill4] as InputSkillKey;
        inputSkillKey.Set_SkillInfo(charInfo.skillCoolTime_4, charType, 4);
    }

    public void Notify_UseSkill(int skillNum)
    {
        InputSkillKey inputSkillKey = null;

        switch (skillNum)
        {
            case 0:
                inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Dash] as InputSkillKey;
                break;
            case 1:
                inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Skill1] as InputSkillKey;
                break;
            case 2:
                inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Skill2] as InputSkillKey;
                break;
            case 3:
                inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Skill3] as InputSkillKey;
                break;
            case 4:
                inputSkillKey = inputKeys[(int)ENUM_INPUTKEY_NAME.Skill4] as InputSkillKey;
                break;
        }

        if (inputSkillKey != null)
            inputSkillKey.Use_Skill();
        else
            Debug.Log("inputSkillKey is Null!");
    }

    public InputKey[] Get_InputKeys()
    {
        return inputKeys;
    }

    public InputKey Get_InputKey(ENUM_INPUTKEY_NAME inputKeyName)
    {
        return inputKeys[(int)inputKeyName];
    }
}
