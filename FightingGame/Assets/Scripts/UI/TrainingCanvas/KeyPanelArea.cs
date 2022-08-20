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
    Color color;
    float size;
    float opacity;
    Vector2 vec;

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

    private void SetInit(Button button)
    {
        rectTransform = button.GetComponent<RectTransform>();

        if (!PlayerPrefs.HasKey($"{button.name}Size"))
            PlayerPrefs.SetFloat($"{button.name}Size", 50);
        if (!PlayerPrefs.HasKey($"{button.name}Opacity"))
            PlayerPrefs.SetFloat($"{button.name}Opacity", 100);

        if(!PlayerPrefs.HasKey($"{button.name}transX"))
            PlayerPrefs.SetFloat($"{button.name}transX", rectTransform.anchoredPosition.x);
        else 
            x = PlayerPrefs.GetFloat($"{button.name}transX");

        if (!PlayerPrefs.HasKey($"{button.name}transY"))
            PlayerPrefs.SetFloat($"{button.name}transY", rectTransform.anchoredPosition.y);
        else
            y = PlayerPrefs.GetFloat($"{button.name}transY");

        if (x != 0 && y != 0)
            rectTransform.anchoredPosition = new Vector2(x, y);
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

    private void SetTransform(Button button)
    {
        rectTransform = button.GetComponent<RectTransform>();
        vec = new Vector2(PlayerPrefs.GetFloat($"{button.name}transX"), PlayerPrefs.GetFloat($"{button.name}transY"));
        rectTransform.anchoredPosition = vec;
    }

    private void SetIsUpdate(GameObject go)
    {
        dragAndDrop = go.GetComponent<DragAndDrop>();

        if (dragAndDrop == null)
            return;

        dragAndDrop.isUpdate = !dragAndDrop.isUpdate;
    }

    // Draggable Change
    public void OnOffDrag(GameObject go)
    {
        SetIsUpdate(go);
    }
}
