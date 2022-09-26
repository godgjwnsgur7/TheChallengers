using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : UIElement
{
    [SerializeField] Button slideBtn;
    RectTransform thisRect;
    RectTransform sliderRect;
    float thisSizeX;
    bool isPanelShow;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void Init()
    {
        thisRect = this.GetComponent<RectTransform>();
        thisSizeX = thisRect.sizeDelta.x;
        sliderRect = slideBtn.GetComponent<RectTransform>();
        isPanelShow = false;
    }

    // ButtonPanel SliderBtn Interactable On/Off
    public void InteractableBtn()
    {
        slideBtn.interactable = !slideBtn.interactable;
    }

    // 훈련장 버튼패널 슬라이드
    public void SlidePanel()
    {
        if (isPanelShow)
        {
            thisRect.Translate(-thisSizeX, 0, 0);
            sliderRect.Rotate(new Vector3(0, 0, -180));
        }
        else
        {
            thisRect.Translate(thisSizeX, 0, 0);
            sliderRect.Rotate(new Vector3(0, 0, 180));
        }

        isPanelShow = !isPanelShow;
    }
}
