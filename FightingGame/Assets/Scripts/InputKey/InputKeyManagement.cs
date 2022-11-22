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
    public InputKey currInputKey = null;

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

    public void OnPoint_DownCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        // 만약 코루틴 변수 안이 null이 아닌데 클릭을 했다면 여기서 리턴시켜버리셈

        isValueChange = true;

        currInputKey = inputPanel.Get_InputKey(_inputKeyName);

        // 주의! 무조건 currInputKey를 이용해서만 값을 변경해야 함!
        // currInputKey를 이용해서 코루틴 돌려서 현재 마우스포인터좌표위치로 이동시킴
        // 이때, 코루틴 변수 안에 담으면서 ㄱㄱ
        
        
    }

    public void OnPoint_UpCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        // 담겨있는 코루틴 변수에 스탑코루틴 하고 후처리할거 하셈
        // 후처리 내용 - 겹치는 오브젝트 확인?
        
    }

    /// <summary>
    /// 저장 성공 시 true, 실패 시 false
    /// </summary>
    public bool Save_InputKeyDatas()
    {
        InputKey[] inputKeys = inputPanel.Get_InputKeys();

        if(InputKey_OverlapCheckAll())
        {
            // 겹치는 영역이 있는 것
            return false;
        }

        List<KeySettingData> keySettingDatas = new List<KeySettingData>();

        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            KeySettingData keySettingData = new KeySettingData(i,
                inputKeys[i].rectTr.localScale.x, inputKeys[i].slotImage.color.a, // opacity 저장방식 변경하셈 ㅇㅇ
                inputKeys[i].rectTr.position.x, inputKeys[i].rectTr.position.y);
            
            keySettingDatas.Add(keySettingData);
        }

        PlayerPrefsManagement.Save_KeySettingData(keySettingDatas);

        return true;
    }

    public bool InputKey_OverlapCheck()
    {

        // 코루틴 안에 들어갈 아이로 현재 제어중인 currInputKey가 겹치는지 확인한다.

        return true; // 임시로 해놈
    }

    /// <summary>
    /// 겹치는 UI가 있는지 확인 (겹칠 경우 true)
    /// </summary>
    public bool InputKey_OverlapCheckAll()
    {
        InputKey[] inputKeys = inputPanel.Get_InputKeys();

        List<InputKey> inputKeysList = new List<InputKey>();

        // 각 인풋키의 끝 좌표를 받아오는데, 아마 크기로만 받아오면 안될거임
        // 사이즈도 감안해서 수식세우고 꼭짓점 4곳에 레이캐스트를 쏴서 확인.
        // 만약 겹치는 아이가 있다면 해당하는 이름을 확인하여 자신만 컬러변경
        // 겹치는 아이가 없다면 영역표시 끄고


        return true; // 그냥 임시로 해놈 수정바람
    }
}
