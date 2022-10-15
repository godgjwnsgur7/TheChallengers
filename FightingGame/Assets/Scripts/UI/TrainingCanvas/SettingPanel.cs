using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class SettingPanel : UIElement
{   
    private float moveSpeed = 1;
    private bool isMove = false;

    private int inputNum;
    private InputPanel inputPanel;
    private InputKey inputKey;
    private InputKeyManagement inputKeyManagement;
    private KeySettingData keySettingData;

    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider opacitySlider;
    [SerializeField] Text sizeText;
    [SerializeField] Text opacityText;

    public override void Close()
    {
        base.Close();
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public void Init()
    {
        this.inputKeyManagement = this.transform.root.Find("InputKeyManagement").GetComponent<InputKeyManagement>();
        this.inputPanel = inputKeyManagement.inputPanel;
    }

    // 클릭 InputKey, Slider 세팅
    public void OnClick_SetInputKey(int _inputNum)
    {
        // InputKey세팅
        this.inputNum = _inputNum;
        this.inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)this.inputNum);
        this.keySettingData = inputKeyManagement.Get_KeySettingData(this.inputNum);

        // Slider 세팅
        this.sizeSlider.value = this.keySettingData.size;
        this.opacitySlider.value = this.keySettingData.opacity;
        this.sizeText.text = (int)sizeSlider.value + "%";
        this.opacityText.text = (int)opacitySlider.value + "%";
    }

    // SizeSlider 값 변경
    public void OnValueChanged_SetSizeSlider()
    {
        int sizeValue = (int)this.sizeSlider.value;

        this.sizeText.text = $"{sizeValue}%";
        this.inputKeyManagement.Set_InputKeySize(sizeValue, this.inputNum);
    }

    // Opacity 값 변경
    public void OnValueChanged_SetOpacitySlider()
    {
        int opacityValue = (int)this.opacitySlider.value;

        this.opacityText.text = $"{opacityValue}%";
        this.inputKeyManagement.Set_InputKeyOpacity(opacityValue, this.inputNum);
    }

    // InputKet 이동
    public void OnPointerDown_MovePos(string _direction)
    {
        Vector2 movePos = this.inputKey.transform.position;
        isMove = true;
        StartCoroutine(MovePosCoroutine(movePos, _direction));
    }

    // InputKey 이동 중지
    public void OnPointerUp_MovePos()
    {
        isMove = false;
        this.moveSpeed = 1;
    }

    IEnumerator MovePosCoroutine(Vector2 _movePos, string _direction)
    {
        while (isMove)
        {
            switch (_direction)
            {
                case "Up":
                    _movePos.y += moveSpeed;
                    break;
                case "Down":
                    _movePos.y -= moveSpeed;
                    break;
                case "Left":
                    _movePos.x -= moveSpeed;
                    break;
                case "Right":
                    _movePos.x += moveSpeed;
                    break;
                default:
                    Debug.Log("범위 벗어남");
                    break;
            }

            moveSpeed += 1 * Time.deltaTime;
            this.inputKeyManagement.Set_InputKeyTransForm(_movePos.x, _movePos.y, this.inputNum);
            yield return null;
        }
    }

    public void OnClick_PopupOpen(string buttonName)
    {
        switch (buttonName)
        {
            case "Close":
                Managers.UI.popupCanvas.Open_SelectPopup(Close, null, "버튼 설정을 종료하시겠습니까?");
                break;
            case "Reset":
                Managers.UI.popupCanvas.Open_SelectPopup(Reset_InputKeyValue, null, "버튼 설정을 초기화하시겠습니까?");
                break;
            case "Save":
                Managers.UI.popupCanvas.Open_SelectPopup(inputKeyManagement.Save_KeySettingData, null, "버튼 설정을 저장하시겠습니까?");
                break;
            default:
                Debug.Log("범위벗어남");
                break;

        }
    }

    public void Reset_InputKeyValue()
    {
        inputKeyManagement.Reset_InputKeyValue();

        sizeSlider.value = inputKeyManagement.Get_KeySettingData(this.inputNum).size;
        opacitySlider.value = inputKeyManagement.Get_KeySettingData(this.inputNum).opacity;
    }
}
