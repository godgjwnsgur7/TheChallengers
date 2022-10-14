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
        this.inputPanel = this.inputKeyManagement.transform.Find("InputPanel").GetComponent<InputPanel>();
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
        this.sizeText.text = (int)this.sizeSlider.value + "%";
        this.keySettingData.size = this.sizeSlider.value;
        this.inputKeyManagement.Set_InputKeySize(this.keySettingData.size, this.inputNum);
    }

    // Opacity 값 변경
    public void OnValueChanged_SetOpacitySlider()
    {
        this.opacityText.text = (int)this.opacitySlider.value + "%";
        this.keySettingData.opacity = this.opacitySlider.value;
        this.inputKeyManagement.Set_InputKeyOpacity(this.keySettingData.opacity, this.inputNum);
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
}
