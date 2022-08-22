using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class KeyPanelArea : UIElement
{
    // 버튼들 모여있는 영역 Panel
    [SerializeField] GameObject leftButtons;
    [SerializeField] GameObject rightButtons;

    // Panel 안의 버튼들
    [SerializeField] UpdatableUI[] buttons;

    RectTransform rectTransform;
    DragAndDrop dragAndDrop;
    Image image;
    float size;
    float opacity;
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

    private void SetInit(UpdatableUI button)
    {
        button.init();
        rectTransform = button.GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        // Size init
        if (!PlayerPrefs.HasKey($"{button.name}" + ENUM_PLAYERPREFS_TYPE.Size))
            PlayerPrefs.SetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.Size, 50);

        // Opacity init
        if (!PlayerPrefs.HasKey($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity))
            PlayerPrefs.SetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, 0);

        if (!PlayerPrefs.HasKey($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity))
            PlayerPrefs.SetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, 100);

        if (!PlayerPrefs.HasKey($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity))
            PlayerPrefs.SetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, 100);

        // BeforeSize init
        if (!PlayerPrefs.HasKey($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX))
            PlayerPrefs.SetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX, button.btnAreaRect.sizeDelta.x);
        if (!PlayerPrefs.HasKey($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY))
            PlayerPrefs.SetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY, button.btnAreaRect.sizeDelta.y);

        if (!PlayerPrefs.HasKey($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX))
            PlayerPrefs.SetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX, button.iconRect.sizeDelta.x);
        if (!PlayerPrefs.HasKey($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY))
            PlayerPrefs.SetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY, button.iconRect.sizeDelta.y);

        if (!PlayerPrefs.HasKey($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX))
            PlayerPrefs.SetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX, button.backGroundRect.sizeDelta.x);
        if (!PlayerPrefs.HasKey($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY))
            PlayerPrefs.SetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY, button.backGroundRect.sizeDelta.y);

        // RectTransform init
        if (!PlayerPrefs.HasKey($"{button.name}" + ENUM_PLAYERPREFS_TYPE.TransX))
            PlayerPrefs.SetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.TransX, rectTransform.anchoredPosition.x);
        else
            tempX = PlayerPrefs.GetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.TransX);

        if (!PlayerPrefs.HasKey($"{button.name}" + ENUM_PLAYERPREFS_TYPE.TransY))
            PlayerPrefs.SetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.TransY, rectTransform.anchoredPosition.y);
        else
            tempY = PlayerPrefs.GetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.TransY);

        if (tempX != 0 && tempY != 0)
            rectTransform.anchoredPosition = new Vector2(tempX, tempY);

        // ResetSize init
        if (!PlayerPrefs.HasKey($"{button.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize))
            PlayerPrefs.SetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize, PlayerPrefs.GetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.Size));

        // ResetOpacity init
        if (!PlayerPrefs.HasKey($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
            PlayerPrefs.SetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
                PlayerPrefs.GetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        if (!PlayerPrefs.HasKey($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
            PlayerPrefs.SetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
                PlayerPrefs.GetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        if (!PlayerPrefs.HasKey($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
            PlayerPrefs.SetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
                PlayerPrefs.GetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        // UI Init Value Accept
        InitSize(button);
        InitOpactiy(button);

        PlayerPrefs.Save();
    }

    private void InitSize(UpdatableUI button)
    {
        button.backGroundRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

        button.btnAreaRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

        button.iconRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));
    }

    private void InitOpactiy(UpdatableUI button)
    {
        opacity = (PlayerPrefs.GetFloat($"{button.btnAreaImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 100);
        image = button.btnAreaImage;
        tempColor = image.color;
        tempColor.a = opacity;
        image.color = tempColor;

        opacity = 0.5f + (PlayerPrefs.GetFloat($"{button.iconImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);
        image = button.iconImage;
        tempColor = image.color;
        tempColor.a = opacity;
        image.color = tempColor;

        opacity = 0.5f + (PlayerPrefs.GetFloat($"{button.backGroundImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);
        image = button.backGroundImage;
        tempColor = image.color;
        tempColor.a = opacity;
        image.color = tempColor;
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

    private void SetSize(UpdatableUI button)
    {
        if (PlayerPrefs.HasKey($"{button.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize))
        {
            button.backGroundRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
            PlayerPrefs.GetFloat($"{button.backGroundRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

            button.btnAreaRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
                PlayerPrefs.GetFloat($"{button.btnAreaRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

            button.iconRect.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeX),
                PlayerPrefs.GetFloat($"{button.iconRect.name}" + ENUM_PLAYERPREFS_TYPE.BaseSizeY));

            PlayerPrefs.SetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.Size, PlayerPrefs.GetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize));
        }
    }

    private void SetOpactiy(UpdatableUI button)
    {
        if (PlayerPrefs.HasKey($"{button.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
        {
            opacity = 0.5f;
            image = button.btnAreaImage;
            tempColor = image.color;
            tempColor.a = opacity;
            image.color = tempColor;

            opacity = 0.5f + (PlayerPrefs.GetFloat($"{button.iconImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);
            image = button.iconImage;
            tempColor = image.color;
            tempColor.a = opacity;
            image.color = tempColor;

            opacity = 0.5f + (PlayerPrefs.GetFloat($"{button.backGroundImage.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);
            image = button.backGroundImage;
            tempColor = image.color;
            tempColor.a = opacity;
            image.color = tempColor;

            PlayerPrefs.SetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, PlayerPrefs.GetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity));
        }
    }

    private void SetTransform(UpdatableUI button)
    {
        rectTransform = button.GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        tempVector = new Vector2(PlayerPrefs.GetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.TransX),
            PlayerPrefs.GetFloat($"{button.name}" + ENUM_PLAYERPREFS_TYPE.TransY));
        rectTransform.anchoredPosition = tempVector;
    }

    private void SetIsUpdate(UpdatableUI go)
    {
        dragAndDrop = go.GetComponent<DragAndDrop>();

        tempColor = go.btnAreaImage.color;

        // Highlight
        if (isSelect)
            tempColor = new Color(255, 255, 255, 0);
        else
            tempColor = new Color(0, 255, 0, 0.5f);

        go.btnAreaImage.color = tempColor;
        isSelect = !isSelect;

        if (dragAndDrop == null)
            return;

        // Draggable
        dragAndDrop.isUpdate = !dragAndDrop.isUpdate;
    }

    // Draggable Change
    public void OnOffDrag(UpdatableUI go)
    {
        SetIsUpdate(go);
    }
}
