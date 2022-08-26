using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class KeyPanelArea : UIElement
{
    // Panel 안의 버튼들
    [SerializeField] UpdatableUI[] buttons;

    RectTransform rectTransform;
    DragAndDrop dragAndDrop;
    float tempOpacity;
    Vector2 tempVector;
    Color tempColor;

    bool isSelect = false;
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
        rectTransform = updateUI.GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        // Size init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size, 50);

        // Opacity init
        if (!PlayerPrefs.HasKey($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity))
            PlayerPrefs.SetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, 0);

        if (!PlayerPrefs.HasKey($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity))
            PlayerPrefs.SetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, 100);

        if (!PlayerPrefs.HasKey($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity))
            PlayerPrefs.SetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, 100);

        // BeforeSize init
        if (!PlayerPrefs.HasKey($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX))
            PlayerPrefs.SetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX, updateUI.btnAreaRect.sizeDelta.x);
        if (!PlayerPrefs.HasKey($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY))
            PlayerPrefs.SetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY, updateUI.btnAreaRect.sizeDelta.y);

        if (!PlayerPrefs.HasKey($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX))
            PlayerPrefs.SetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX, updateUI.iconRect.sizeDelta.x);
        if (!PlayerPrefs.HasKey($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY))
            PlayerPrefs.SetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY, updateUI.iconRect.sizeDelta.y);

        if (!PlayerPrefs.HasKey($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX))
            PlayerPrefs.SetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX, updateUI.backGroundRect.sizeDelta.x);
        if (!PlayerPrefs.HasKey($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY))
            PlayerPrefs.SetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY, updateUI.backGroundRect.sizeDelta.y);

        // RectTransform init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX, rectTransform.anchoredPosition.x);
        else
            tempX = PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX);

        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY, rectTransform.anchoredPosition.y);
        else
            tempY = PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY);

        if (tempX != 0 && tempY != 0)
            rectTransform.anchoredPosition = new Vector2(tempX, tempY);

        // ResetSize init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize, PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size));

        // ResetOpacity init
        if (!PlayerPrefs.HasKey($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
            PlayerPrefs.SetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
                PlayerPrefs.GetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        if (!PlayerPrefs.HasKey($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
            PlayerPrefs.SetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
                PlayerPrefs.GetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        if (!PlayerPrefs.HasKey($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
            PlayerPrefs.SetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
                PlayerPrefs.GetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

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

        updateUI.backGroundRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

        updateUI.btnAreaRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

        updateUI.iconRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));
    }

    // 초기 UI Opacity 설정
    private void InitOpactiy(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        tempOpacity = (PlayerPrefs.GetFloat($"{updateUI.btnAreaImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 100);
        tempColor = updateUI.btnAreaImage.color;
        tempColor.a = tempOpacity;
        updateUI.btnAreaImage.color = tempColor;

        tempOpacity = 0.5f + (PlayerPrefs.GetFloat($"{updateUI.iconImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);
        tempColor = updateUI.btnAreaImage.color;
        tempColor.a = tempOpacity;
        updateUI.btnAreaImage.color = tempColor;

        tempOpacity = 0.5f + (PlayerPrefs.GetFloat($"{updateUI.backGroundImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);
        tempColor = updateUI.btnAreaImage.color;
        tempColor.a = tempOpacity;
        updateUI.btnAreaImage.color = tempColor;
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

        if (PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize))
        {
            updateUI.backGroundRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{updateUI.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

            updateUI.btnAreaRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
                PlayerPrefs.GetFloat($"{updateUI.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

            updateUI.iconRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
                PlayerPrefs.GetFloat($"{updateUI.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

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
            tempOpacity = 0.5f;
            tempColor = updateUI.btnAreaImage.color;
            tempColor.a = tempOpacity;
            updateUI.btnAreaImage.color = tempColor;

            tempOpacity = 0.5f + (PlayerPrefs.GetFloat($"{updateUI.iconImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);
            tempColor = updateUI.btnAreaImage.color;
            tempColor.a = tempOpacity;
            updateUI.btnAreaImage.color = tempColor;

            tempOpacity = 0.5f + (PlayerPrefs.GetFloat($"{updateUI.backGroundImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);
            tempColor = updateUI.btnAreaImage.color;
            tempColor.a = tempOpacity;
            updateUI.btnAreaImage.color = tempColor;

            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity));
        }
    }

    // Transform 리셋
    private void SetTransform(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        rectTransform = updateUI.GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        tempVector = new Vector2(PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX),
            PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY));
        rectTransform.anchoredPosition = tempVector;
    }

    // HighLight
    public void OnOffHighLight(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        SetHighLight(updateUI);
    }

    //선택 UI 강조
    private void SetHighLight(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        tempColor = updateUI.btnAreaImage.color;

        // Highlight
        if (isSelect)
            tempColor = new Color(255, 255, 255, 0);
        else
            tempColor = new Color(0, 255, 0, 0.5f);

        updateUI.btnAreaImage.color = tempColor;
        isSelect = !isSelect;

        if (dragAndDrop == null)
            return;
    }
}
