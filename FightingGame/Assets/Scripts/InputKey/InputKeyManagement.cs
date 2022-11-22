using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputKeyManagement : MonoBehaviour
{
    public bool isMove = false;
    public bool isValueChange = false;

    [SerializeField] SettingPanel settingPanel;
    public InputPanel inputPanel = null;
    private InputKey currInputKey = null;

    public void Init()
    {
        if (inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(OnPoint_DownCallBack, OnPoint_UpCallBack);
            inputPanel.transform.SetAsFirstSibling();
        }

        if(!settingPanel.isInit)
            settingPanel.Init();
    }

    public void OnPoint_DownCallBack(InputKey _inputKey)
    {
        if (currInputKey != null) // 사용 중
            return;

        isValueChange = true;
        currInputKey = _inputKey;

        // currInputKey를 이용해서 코루틴 돌려서 현재 마우스포인터좌표위치로 이동시킴
        // 이때, 코루틴 변수 안에 담으면서 ㄱㄱ
    }

    public void OnPoint_UpCallBack(InputKey _inputKey)
    {
        // 담겨있는 코루틴 변수에 스탑코루틴 하고 후처리할거 하셈

        currInputKey = null;
    }

    public void Save_InputKeyDatas()
    {
        InputKey[] inputKeys = inputPanel.Get_InputKeys();

        for(int i = 0; i < inputKeys.Length; i++)
        {
            
        }
    }
}
