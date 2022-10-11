using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class SettingPanel : UIElement
{
    private int areaNum;
    private InputPanel inputPanel;
    private InputKey inputKey;
    InputKeyManagement inputKeyManagement;

    [SerializeField] Button upbtn;
    [SerializeField] Button downbtn;
    [SerializeField] Button leftbtn;
    [SerializeField] Button rightbtn;

    public override void Close()
    {
        base.Close();
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    // 임시 테스트용
    public void OnClick_SetInputKey(int _InputNum)
    {
        this.inputKeyManagement = this.transform.root.Find("InputKeyManagement").GetComponent<InputKeyManagement>();
        this.inputPanel = this.inputKeyManagement.transform.Find("InputPanel").GetComponent<InputPanel>();
        this.inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_InputNum);
        this.areaNum = _InputNum;
    }

    // 임시 테스트용
    public void OnClick_MovePos(string direction)
    {
        Vector2 movePos = inputKey.transform.position;

        switch (direction)
        {
            case "Up":
                inputKeyManagement.Set_InputKeyTransForm(movePos.x, movePos.y + 10, areaNum);
                break;
            case "Down":
                inputKeyManagement.Set_InputKeyTransForm(movePos.x, movePos.y - 10, areaNum);
                break;
            case "Left":
                inputKeyManagement.Set_InputKeyTransForm(movePos.x - 10, movePos.y, areaNum);
                break;
            case "Right":
                inputKeyManagement.Set_InputKeyTransForm(movePos.x + 10, movePos.y, areaNum);
                break;
            default:
                Debug.Log("범위 벗어남");
                break;
        }
    }
}
