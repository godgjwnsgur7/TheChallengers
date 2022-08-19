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

    private float size;
    private Color color;

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
        sizeSlider.value = PlayerPrefs.GetFloat($"{setBtn.name}Size");
        opacitySlider.value = PlayerPrefs.GetFloat($"{setBtn.name}Opacity");
    }


    public void SettingSizeSlider()
    {
        size = (sizeSlider.value / sizeSlider.maxValue);
        setBtn.transform.localScale = new Vector3(0.5f + size, 0.5f + size, 0.5f + size); 
    }

    public void SettingOpacitySlider()
    {
        color = setBtnImage.color;
        color.a = opacitySlider.value / opacitySlider.maxValue;
        setBtnImage.color = color;
    }

    public void SaveSliderValue()
    {
        PlayerPrefs.SetFloat($"{setBtn.name}Size", sizeSlider.value);
        PlayerPrefs.SetFloat($"{setBtn.name}Opacity", opacitySlider.value);
        PlayerPrefs.Save();
    }
}
