using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatableUI : UIElement
{
    public GameObject btnArea;
    public GameObject backGround;
    public GameObject icon;

    public Image btnAreaImage;
    public Image backGroundImage;
    public Image iconImage;

    public RectTransform btnAreaRect;
    public RectTransform backGroundRect;
    public RectTransform iconRect;

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
        btnAreaImage = btnArea.GetComponent<Image>();
        backGroundImage = backGround.GetComponent<Image>();
        iconImage = icon.GetComponent<Image>();

        btnAreaRect = btnArea.GetComponent<RectTransform>();
        backGroundRect = backGround.GetComponent<RectTransform>();
        iconRect = icon.GetComponent<RectTransform>();
    }
}
