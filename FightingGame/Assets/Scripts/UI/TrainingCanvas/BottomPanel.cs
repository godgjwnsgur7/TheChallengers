using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : UIElement
{
    private GameObject setBtn;
    Image setBtnImage;
    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider opacitySlider;


    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void setSlider(GameObject go)
    {
        setBtn = go;
        setBtnImage = setBtn.GetComponent<Image>();
        sizeSlider.value = PlayerPrefs.GetInt($"{setBtn.name} + Size");
        opacitySlider.value = PlayerPrefs.GetInt($"{setBtn.name} + Opacity");
    }


    public void SettingSizeSlider()
    {
        setBtn.transform.localScale = new Vector3(0.5f + (sizeSlider.value/100), 0.5f + (sizeSlider.value / 100), 0.5f + (sizeSlider.value / 100)); 
    }

    public void SettingOpacitySlider()
    {
        Color color = setBtnImage.color;
        color.a = 2.55f * opacitySlider.value;
        setBtnImage.color = color;
    }
}
