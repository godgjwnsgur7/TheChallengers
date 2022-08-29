using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class KeyPanelArea : UIElement
{
    // Panel 안의 버튼들
    [SerializeField] UpdatableUI[] buttons;

    float sizeRatio;
    float opacityRatio;
    Vector2 tempVector;

    float tempX;
    float tempY;

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
        for (int i = 0; i < buttons.Length; i++)
            SetInit(buttons[i]);
    }

    // 초기 PlayerPrefs 값 설정 및 UI 초기배치
    private void SetInit(UpdatableUI updateUI)
    {
        updateUI.init();

        // Size init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size, 50);

        // Opacity init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, 100);

        // RectTransform init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX, updateUI.GetTransform().x);
        else
            tempX = PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX);

        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY, updateUI.GetTransform().y);
        else
            tempY = PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY);

        if (tempX != 0 && tempY != 0)
            updateUI.SetTransform(new Vector2(tempX, tempY));

        // ResetSize init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize, PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size));

        // ResetOpacity init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
                PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        // UI Init Value Accept
        InitSize(updateUI);
        InitOpactiy(updateUI);

        PlayerPrefs.Save();
    }

    // 초기 UI size 설정
    private void InitSize(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        sizeRatio = (PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size) + 50) / 100;

        updateUI.SetSize(sizeRatio, true);
    }

    // 초기 UI Opacity 설정
    private void InitOpactiy(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        opacityRatio = (PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);

        updateUI.SetOpacity(opacityRatio, true);
    }

    // Reset Not Saved Slider Value
    public void SliderReset()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            SetSize(buttons[i]);
            SetOpactiy(buttons[i]);
            SetTransform(buttons[i]);
        }

        Managers.UI.CloseUI<BottomPanel>();
    }

    // Size 리셋
    private void SetSize(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        sizeRatio = (PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize) + 50) / 100;

        if (PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize))
        {
            updateUI.SetSize(sizeRatio);

            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size, PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize));
        }
    }

    // Opacity 리셋
    private void SetOpactiy(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        if (PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
        {
            opacityRatio = (PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity) / 200);

            updateUI.SetOpacity(opacityRatio, true);

            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity));
        }
    }

    // Transform 리셋
    private void SetTransform(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        tempVector = new Vector2(PlayerPrefs.GetFloat($"{updateUI.thisRect.name}" + ENUM_PLAYERPREFS_TYPE.TransX),
            PlayerPrefs.GetFloat($"{updateUI.thisRect.name}" + ENUM_PLAYERPREFS_TYPE.TransY));

        updateUI.SetTransform(tempVector);
    }
}
