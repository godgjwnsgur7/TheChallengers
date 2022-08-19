using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPanelArea : UIElement
{
    [SerializeField] GameObject joyStick;
    [SerializeField] Button AttackBtn;
    [SerializeField] Button JumpBtn;
    [SerializeField] Button SkillBtn1;
    [SerializeField] Button SkillBtn2;
    [SerializeField] Button SkillBtn3;

    float size;
    float opacity;

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
        // LeftBtnArea
        if (!PlayerPrefs.HasKey($"{joyStick.name}Size"))
            PlayerPrefs.SetFloat($"{joyStick.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{joyStick.name}Opacity"))
            PlayerPrefs.SetFloat($"{joyStick.name}Opacity", 100);

        // RightBtnArea
        if (!PlayerPrefs.HasKey($"{AttackBtn.name}Size"))
            PlayerPrefs.SetFloat($"{AttackBtn.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{AttackBtn.name}Opacity"))
            PlayerPrefs.SetFloat($"{AttackBtn.name}Opacity", 100);

        if (!PlayerPrefs.HasKey($"{JumpBtn.name}Size"))
            PlayerPrefs.SetFloat($"{JumpBtn.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{JumpBtn.name}Opacity"))
            PlayerPrefs.SetFloat($"{JumpBtn.name}Opacity", 100);

        if (!PlayerPrefs.HasKey($"{SkillBtn1.name}Size"))
            PlayerPrefs.SetFloat($"{SkillBtn1.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{SkillBtn1.name}Opacity"))
            PlayerPrefs.SetFloat($"{SkillBtn1.name}Opacity", 100);

        if (!PlayerPrefs.HasKey($"{SkillBtn2.name}Size"))
            PlayerPrefs.SetFloat($"{SkillBtn2.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{SkillBtn2.name}Opacity"))
            PlayerPrefs.SetFloat($"{SkillBtn2.name}Opacity", 100);

        if (!PlayerPrefs.HasKey($"{SkillBtn3.name}Size"))
            PlayerPrefs.SetFloat($"{SkillBtn3.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{SkillBtn3.name}Opacity"))
            PlayerPrefs.SetFloat($"{SkillBtn3.name}Opacity", 100);
    }

    public void SliderReset()
    {
        size = (PlayerPrefs.GetFloat($"{joyStick.name}Size") / 100);
        joyStick.transform.localScale = new Vector3(0.5f + size, 0.5f + size, 0.5f + size);

        size = (PlayerPrefs.GetFloat($"{AttackBtn.name}Size") / 100);
        AttackBtn.transform.localScale = new Vector3(0.5f + size, 0.5f + size, 0.5f + size);

        size = (PlayerPrefs.GetFloat($"{JumpBtn.name}Size") / 100);
        JumpBtn.transform.localScale = new Vector3(0.5f + size, 0.5f + size, 0.5f + size);

        size = (PlayerPrefs.GetFloat($"{SkillBtn1.name}Size") / 100);
        SkillBtn1.transform.localScale = new Vector3(0.5f + size, 0.5f + size, 0.5f + size);

        size = (PlayerPrefs.GetFloat($"{SkillBtn2.name}Size") / 100);
        SkillBtn2.transform.localScale = new Vector3(0.5f + size, 0.5f + size, 0.5f + size);

        size = (PlayerPrefs.GetFloat($"{SkillBtn3.name}Size") / 100);
        SkillBtn3.transform.localScale = new Vector3(0.5f + size, 0.5f + size, 0.5f + size);

        Managers.UI.CloseUI<BottomPanel>();
    }
}
