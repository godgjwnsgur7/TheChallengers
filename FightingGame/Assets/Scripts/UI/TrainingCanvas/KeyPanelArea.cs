using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPanelArea : UIElement
{
    // 버튼들 모여있는 영역 Panel
    [SerializeField] GameObject leftButtons;
    [SerializeField] GameObject rightButtons;

    // Panel 안의 버튼들
    [SerializeField] Button joyStick;
    [SerializeField] Button AttackBtn;
    [SerializeField] Button JumpBtn;
    [SerializeField] Button SkillBtn1;
    [SerializeField] Button SkillBtn2;
    [SerializeField] Button SkillBtn3;

    float size;
    float opacity;
    Image image;
    Color color;
    DragAndDrop dragAndDrop;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void init()
    {
        // Base Slider Value Call
        SetInit(joyStick);
        SetInit(AttackBtn);
        SetInit(JumpBtn);
        SetInit(SkillBtn1);
        SetInit(SkillBtn2);
        SetInit(SkillBtn3);
    }

    // Reset Not Saved Slider Value
    public void SliderReset()
    {

        SetSize(joyStick);
        SetSize(AttackBtn);
        SetSize(JumpBtn);
        SetSize(SkillBtn1);
        SetSize(SkillBtn2);
        SetSize(SkillBtn3);

        SetOpactiy(joyStick);
        SetOpactiy(AttackBtn);
        SetOpactiy(JumpBtn);
        SetOpactiy(SkillBtn1);
        SetOpactiy(SkillBtn2);
        SetOpactiy(SkillBtn3);

        Managers.UI.CloseUI<BottomPanel>();
    }

    private void SetInit(Button buttom)
    {
        if (!PlayerPrefs.HasKey($"{buttom.name}Size"))
            PlayerPrefs.SetFloat($"{buttom.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{buttom.name}Opacity"))
            PlayerPrefs.SetFloat($"{buttom.name}Opacity", 100);
    }

    private void SetSize(Button button)
    {
        size = (PlayerPrefs.GetFloat($"{button.name}Size") / 100);
        button.transform.localScale = new Vector3(0.5f + size, 0.5f + size, 0.5f + size);
    }

    private void SetOpactiy(Button button)
    {
        opacity = (PlayerPrefs.GetFloat($"{button.name}Opacity") / 100);
        image = button.GetComponent<Image>();
        color = image.color;
        color.a = opacity;
        image.color = color;
    }
    
    private void SetIsUpdate(Button button)
    {
        dragAndDrop = button.GetComponent<DragAndDrop>();

        if (dragAndDrop == null)
            return;

        dragAndDrop.isUpdate = !dragAndDrop.isUpdate;
    }

    private void SetIsUpdate(GameObject go)
    {
        dragAndDrop = go.GetComponent<DragAndDrop>();

        if (dragAndDrop == null)
            return;

        dragAndDrop.isUpdate = !dragAndDrop.isUpdate;
    }

    // Draggable Change
    public void OnOffDrag()
    {
        SetIsUpdate(leftButtons);
        SetIsUpdate(rightButtons);
        SetIsUpdate(joyStick);
        SetIsUpdate(AttackBtn);
        SetIsUpdate(JumpBtn);
        SetIsUpdate(SkillBtn1);
        SetIsUpdate(SkillBtn2);
        SetIsUpdate(SkillBtn3);
    }
}
