using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class InputKeyAreaPanel : MonoBehaviour
{
    public InputKeyArea[] inputKeyAreas = new InputKeyArea[(int)ENUM_INPUTKEY_NAME.Max];

    bool isInit = false;

    public void Init(Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        if (isInit)
            return;
        isInit = true;

        KeySettingData keySettingData = Load_InputKeyData();

        for(int i = 0; i < inputKeyAreas.Length; i++)
        {
            inputKeyAreas[i] = gameObject.transform.Find(Enum.GetName(typeof(ENUM_INPUTKEY_NAME), i)).GetComponent<InputKeyArea>();
            
            if(inputKeyAreas[i] == null)
            {
                Debug.LogError($"{Enum.GetName(typeof(ENUM_INPUTKEY_NAME), i)} 를 찾지 못했습니다.");
                return;
            }
            
            inputKeyAreas[i].Init(_OnPointDownCallBack, _OnPointUpCallBack);

            if (keySettingData != null)
                Set_InputKeyArea(inputKeyAreas[i], keySettingData.keySettingDataList[i], keySettingData.opacity);
        }

        Set_ChangeIcon(ENUM_CHARACTER_TYPE.Knight);
    }

    public void Clear()
    {
        for(int i = 0; i < inputKeyAreas.Length; i++)
        {
            inputKeyAreas[i].Deselect_AreaImage();
        }
    }

    private void Set_InputKeyArea(InputKeyArea inputKeyArea, KeySettingDataElement keySettingDataElement, float _opacity)
    {
        inputKeyArea.rectTr.localScale = new Vector3(keySettingDataElement.scaleSize, keySettingDataElement.scaleSize, 1f);

        inputKeyArea.Set_Transparency(_opacity);

        inputKeyArea.rectTr.position = new Vector2(keySettingDataElement.rectTrX, keySettingDataElement.rectTrY);
    }

    public void Set_ChangeIcon(ENUM_CHARACTER_TYPE charType)
    {
        inputKeyAreas[(int)ENUM_INPUTKEY_NAME.Skill1].ChangeSet_IconImage($"Icon_{charType}Skill1");
        inputKeyAreas[(int)ENUM_INPUTKEY_NAME.Skill2].ChangeSet_IconImage($"Icon_{charType}Skill2");
        inputKeyAreas[(int)ENUM_INPUTKEY_NAME.Skill3].ChangeSet_IconImage($"Icon_{charType}Skill3");
        inputKeyAreas[(int)ENUM_INPUTKEY_NAME.Skill4].ChangeSet_IconImage($"Icon_{charType}Skill4");
    }

    public void Set_OpacityValueAll(float _value)
    {
        for(int i = 0; i < inputKeyAreas.Length; i++)
            inputKeyAreas[i].Set_Transparency(_value / 100f);
    }

    public void Reset_InputKeyData()
    {
        PlayerPrefsManagement.Delete_KetSettingData();
        KeySettingData keySettingData = Load_InputKeyData();
        for(int i = 0; i < inputKeyAreas.Length; i++)
            Set_InputKeyArea(inputKeyAreas[i], keySettingData.keySettingDataList[i], keySettingData.opacity);
        Set_ChangeIcon(ENUM_CHARACTER_TYPE.Knight);
    }

    public bool Save_InputKeyData()
    {
        // 겹치는 영역 체크
        for(int i = 0; i < inputKeyAreas.Length; i++)
        {
            if(inputKeyAreas[i].Get_CollisionCheck())
            {
                Managers.UI.popupCanvas.Open_NotifyPopup("저장에 실패했습니다.\n( 겹치는 영역 존재 )");
                return false;
            }            
        }

        List<KeySettingDataElement> keySettingDataList = new List<KeySettingDataElement>();
        float opacity = inputKeyAreas[0].Get_Transparency();

        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            KeySettingDataElement keySettingDataElement = new KeySettingDataElement(i,
                inputKeyAreas[i].rectTr.localScale.x,
                inputKeyAreas[i].rectTr.position.x, inputKeyAreas[i].rectTr.position.y);

            keySettingDataList.Add(keySettingDataElement);
        }

        KeySettingData keySettingData = new KeySettingData(keySettingDataList, opacity);

        PlayerPrefsManagement.Save_KeySettingData(keySettingData);

        return true;
    }

    public KeySettingData Load_InputKeyData()
    {
        KeySettingData keySettingData = PlayerPrefsManagement.Load_KeySettingData();

        if(keySettingData == null)
        {
            InputPanel inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.gameObject.SetActive(false);
            inputPanel.Init(null, null);
            InputKey[] inputKeys = inputPanel.Get_InputKeys();

            List<KeySettingDataElement> keySettingDataList = new List<KeySettingDataElement>();
            float opacity = inputKeyAreas[0].Get_Transparency();

            for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
            {
                KeySettingDataElement keySettingDataElement = new KeySettingDataElement(i,
                    inputKeys[i].rectTr.localScale.x,
                    inputKeys[i].rectTr.position.x, inputKeys[i].rectTr.position.y);

                keySettingDataList.Add(keySettingDataElement);
            }

            keySettingData = new KeySettingData(keySettingDataList, opacity);
            Managers.Resource.Destroy(inputPanel.gameObject);
        }

        return keySettingData;
    }

    public InputKeyArea Get_InputKeyArea(int index)
    {
        return inputKeyAreas[index];
    }
}
