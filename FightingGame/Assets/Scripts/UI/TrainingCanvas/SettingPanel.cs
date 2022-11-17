using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class SettingPanel : UIElement
{
    private float moveSpeed = 1;
    private bool isMove = false;
    public bool isValueChange = false;
    public bool isHide = false;
    private Coroutine runningCoroutine = null;

    private int inputNum = -1;
    private InputPanel inputPanel = null;
    private InputKey inputKey = null;
    private KeySettingData keySettingData = null;

    private AreaPanel areaPanel = null;
    private RectTransform thisRect;

    [SerializeField] InputKeyManagement inputKeyManagement;
    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider opacitySlider;
    [SerializeField] Text sizeText;
    [SerializeField] Text opacityText;

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
        if (inputKey == null)
            return;

        if (inputNum < 0)
            return;

        int sizeValue = (int)this.sizeSlider.value;

        this.sizeText.text = $"{sizeValue}%";
        this.inputKeyManagement.Set_InputKeySize(sizeValue, this.inputNum);

        if (!isValueChange)
            isValueChange = true;
    }

    // Opacity 값 변경
    public void OnValueChanged_SetOpacitySlider()
    {
        if (inputKey == null)
            return;

        if (inputNum < 0)
            return;

        int opacityValue = (int)this.opacitySlider.value;

        this.opacityText.text = $"{opacityValue}%";
        this.inputKeyManagement.Set_InputKeyOpacity(opacityValue, this.inputNum);

        if (!isValueChange)
            isValueChange = true;
    }

    // InputKet 이동
    public void OnPointerDown_MovePos(string _direction)
    {
        if (this.inputKey == null)
            return;

        Vector2 movePos = this.inputKey.GetComponent<RectTransform>().anchoredPosition;
        isMove = true;
        StartCoroutine(MovePosCoroutine(movePos, _direction));
    }

    // InputKey 이동 중지
    public void OnPointerUp_MovePos()
    {
        isMove = false;
        this.moveSpeed = 1;

        if (!isValueChange)
            isValueChange = true;
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

    public void OnClick_CloseBtn()
    {
        if(isValueChange)
            Managers.UI.popupCanvas.Open_SelectPopup(Close, null, "값을 저장하지않고 종료하시겠습니까?");
        else
            Managers.UI.popupCanvas.Open_SelectPopup(Close, null, "버튼 설정을 종료하시겠습니까?");
    }
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

        if (this.inputNum < 0)
            return;

        sizeSlider.value = inputKeyManagement.Get_KeySettingData(this.inputNum).size;
        opacitySlider.value = inputKeyManagement.Get_KeySettingData(this.inputNum).opacity;
    }

    // 세팅패널 초기화
    public void Reset_SettingPanel()
    {
        this.inputNum = -1;
        this.inputKey = null;
        this.sizeSlider.value = 50;
        this.opacitySlider.value = 100;
        this.sizeText.text = "50%";
        this.opacityText.text = "100%";
    }

    // 세팅패널 숨기기, 보이기
    public void Move_SettingPanel()
    {
        if (runningCoroutine != null)
            return;

        if (isHide)
            Show_SettingPanel();
        else
            Hide_SettingPanel();
    }

    public void Hide_SettingPanel()
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        isHide = true;


        float showPos = Screen.height;
        Vector3 target = thisRect.position;
        target.y = showPos + thisRect.sizeDelta.y;

        runningCoroutine = StartCoroutine(MoveVec(target));
    }

    public void Show_SettingPanel()
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        isHide = false;

        float showPos = Screen.height;
        Vector3 target = thisRect.position;
        target.y = showPos;

        runningCoroutine = StartCoroutine(MoveVec(target));
    }

    IEnumerator MoveVec(Vector3 vec)
    {
        while (thisRect.position != vec)
        {
            thisRect.position = Vector3.MoveTowards(thisRect.position, vec, 30);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        runningCoroutine = null;
    }
}
