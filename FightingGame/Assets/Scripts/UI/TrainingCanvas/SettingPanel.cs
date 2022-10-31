using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class SettingPanel : UIElement
{
    private float moveSpeed = 1;
    private bool isMove = false;
    private bool isHide = false;
    private Coroutine runningCoroutine = null;

    private int inputNum;
    private InputPanel inputPanel = null;
    private InputKey inputKey = null;
    private InputKeyManagement inputKeyManagement = null;
    private KeySettingData keySettingData = null;

    private AreaPanel areaPanel = null;
    private RectTransform thisRect;

    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider opacitySlider;
    [SerializeField] Text sizeText;
    [SerializeField] Text opacityText;
    [SerializeField] Text HideButtonText;

    public override void Close()
    {
        inputKeyManagement.Set_PanelActive(false);
        Reset_SettingPanel();
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
        this.areaPanel = inputKeyManagement.areaPanel;
        this.thisRect = GetComponent<RectTransform>();
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
        if (inputKey = null)
            return;

        int sizeValue = (int)this.sizeSlider.value;

        this.sizeText.text = $"{sizeValue}%";
        this.inputKeyManagement.Set_InputKeySize(sizeValue, this.inputNum);
    }

    // Opacity 값 변경
    public void OnValueChanged_SetOpacitySlider()
    {
        if (inputKey = null)
            return;

        int opacityValue = (int)this.opacitySlider.value;

        this.opacityText.text = $"{opacityValue}%";
        this.inputKeyManagement.Set_InputKeyOpacity(opacityValue, this.inputNum);
    }

    // InputKet 이동
    public void OnPointerDown_MovePos(string _direction)
    {
        Vector2 movePos = this.inputKey.GetComponent<RectTransform>().anchoredPosition;
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

    public void OnClick_CloseBtn() => Managers.UI.popupCanvas.Open_SelectPopup(Close, null, "버튼 설정을 종료하시겠습니까?");
    public void OnClick_ResetBtn() => Managers.UI.popupCanvas.Open_SelectPopup(Reset_InputKeyValue, null, "버튼 설정을 초기화하시겠습니까?");
    public void OnClick_SaveBtn()
    {
        if (areaPanel.Get_Updatable())
            Managers.UI.popupCanvas.Open_SelectPopup(inputKeyManagement.Save_KeySettingData, null, "버튼 설정을 저장하시겠습니까?");
        else
            Managers.UI.popupCanvas.Open_NotifyPopup("버튼의 영역이 겹쳐 수정이 불가능합니다.");
    }

    public void Reset_InputKeyValue()
    {
        inputKeyManagement.Reset_InputKeyValue();

        sizeSlider.value = inputKeyManagement.Get_KeySettingData(this.inputNum).size;
        opacitySlider.value = inputKeyManagement.Get_KeySettingData(this.inputNum).opacity;
    }

    // 세팅패널 초기화
    public void Reset_SettingPanel()
    {
        this.inputKey = null;
        this.sizeSlider.value = 50;
        this.opacitySlider.value = 100;
        this.sizeText.text = "50%";
        this.opacityText.text = "100%";
    }

    public void Move_SettingPanel()
    {
        if (runningCoroutine != null)
            return;

        Vector3 target = thisRect.position;
        float panelSizeY = thisRect.sizeDelta.y;

        if (isHide)
            panelSizeY *= -1f;

        target.y += panelSizeY;

        runningCoroutine = StartCoroutine(MoveVec(target));
        isHide = !isHide;
    }

    IEnumerator MoveVec(Vector3 vec)
    {
        while (thisRect.position != vec)
        {
            thisRect.position = Vector3.MoveTowards(thisRect.position, vec, 5);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        runningCoroutine = null;
    }
}
