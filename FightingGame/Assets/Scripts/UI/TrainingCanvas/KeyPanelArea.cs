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
    [SerializeField] Button[] buttons;

    RectTransform rectTransform;
    DragAndDrop dragAndDrop;
    Image image;
    float size;
    float opacity;
    Vector2 vec;
    Color color;

    bool isRed = false;
    float x;
    float y;

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

    private void SetInit(Button button)
    {
        rectTransform = button.GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        // Size, Opacity
        if (!PlayerPrefs.HasKey($"{button.name}Size"))
            PlayerPrefs.SetFloat($"{button.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{button.name}Opacity"))
            PlayerPrefs.SetFloat($"{button.name}Opacity", 100);

        // BeforeSize
        if (!PlayerPrefs.HasKey($"{button.name}BaseSizeX"))
            PlayerPrefs.SetFloat($"{button.name}BaseSizeX", rectTransform.sizeDelta.x);

        if (!PlayerPrefs.HasKey($"{button.name}BaseSizeY"))
            PlayerPrefs.SetFloat($"{button.name}BaseSizeY", rectTransform.sizeDelta.x);

        // RectTransform
        if (!PlayerPrefs.HasKey($"{button.name}transX"))
            PlayerPrefs.SetFloat($"{button.name}transX", rectTransform.anchoredPosition.x);
        else 
            x = PlayerPrefs.GetFloat($"{button.name}transX");

        if (!PlayerPrefs.HasKey($"{button.name}transY"))
            PlayerPrefs.SetFloat($"{button.name}transY", rectTransform.anchoredPosition.y);
        else
            y = PlayerPrefs.GetFloat($"{button.name}transY");

        if (x != 0 && y != 0)
            rectTransform.anchoredPosition = new Vector2(x, y);

        if (!PlayerPrefs.HasKey($"{button.name}ResetSize"))
            PlayerPrefs.SetFloat($"{button.name}ResetSize", PlayerPrefs.GetFloat($"{button.name}Size"));
        if (!PlayerPrefs.HasKey($"{button.name}ResetOpacity"))
            PlayerPrefs.SetFloat($"{button.name}ResetOpacity", PlayerPrefs.GetFloat($"{button.name}Opacity"));

        InitSize(button);
        InitOpactiy(button);

        PlayerPrefs.Save();
    }

    private void InitSize(Button button)
    {
        rectTransform = button.GetComponent<RectTransform>();

        size = (PlayerPrefs.GetFloat($"{button.name}Size") / 100);
        rectTransform.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{button.name}BaseSizeX"), PlayerPrefs.GetFloat($"{button.name}BaseSizeY"));

    }

    private void InitOpactiy(Button button)
    {
        opacity = (PlayerPrefs.GetFloat($"{button.name}Opacity") / 100);
        image = button.GetComponent<Image>();
        color = image.color;
        color.a = opacity;
        image.color = color;
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

    private void SetSize(Button button)
    {
        rectTransform = button.GetComponent<RectTransform>();

        if (PlayerPrefs.HasKey($"{button.name}ResetSize"))
        {
            size = (PlayerPrefs.GetFloat($"{button.name}ResetSize") + 50) / 100;
            rectTransform.sizeDelta = new Vector2(PlayerPrefs.GetFloat($"{button.name}BaseSizeX") * size, PlayerPrefs.GetFloat($"{button.name}BaseSizeY") * size);

            PlayerPrefs.SetFloat($"{button.name}Size", PlayerPrefs.GetFloat($"{button.name}ResetSize"));
            //bottomPanel.sizeSlider.value = PlayerPrefs.GetFloat($"{button.name}Size");
        }
    }

    private void SetOpactiy(Button button)
    {
        if (PlayerPrefs.HasKey($"{button.name}ResetOpacity"))
        {
            opacity = (PlayerPrefs.GetFloat($"{button.name}ResetOpacity") / 200);
            image = button.GetComponent<Image>();
            color = image.color;
            color.a = opacity;
            image.color = color;
            PlayerPrefs.SetFloat($"{button.name}Opacity", PlayerPrefs.GetFloat($"{button.name}ResetOpacity"));
            //bottomPanel.opacitySlider.value = PlayerPrefs.GetFloat($"{button.name}Opacity");
        }
        
    }

    private void SetTransform(Button button)
    {
        rectTransform = button.GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        vec = new Vector2(PlayerPrefs.GetFloat($"{button.name}transX"), PlayerPrefs.GetFloat($"{button.name}transY"));
        rectTransform.anchoredPosition = vec;
    }

    private void SetIsUpdate(GameObject go)
    {
        dragAndDrop = go.GetComponent<DragAndDrop>();
        image = go.GetComponent<Image>();
        color = image.color;

        // Highlight
        if (isRed)
            color = new Color(255, 255, 255, color.a);
        else
            color = new Color(255, 0, 0, color.a);

        image.color = color;
        isRed = !isRed;

        if (dragAndDrop == null)
            return;

        // Draggable
        dragAndDrop.isUpdate = !dragAndDrop.isUpdate;
    }

    // Draggable Change
    public void OnOffDrag(GameObject go)
    {
        SetIsUpdate(go);
    }
}
