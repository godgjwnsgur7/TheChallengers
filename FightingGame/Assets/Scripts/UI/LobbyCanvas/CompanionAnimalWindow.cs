using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanionAnimalWindow : UIElement
{
    [SerializeField] Sprite[] horizonSprites;
    [SerializeField] Sprite[] verticalSprites;

    [SerializeField] Image horizonImage;
    [SerializeField] Image verticalImage;

    [SerializeField] GameObject horizonObject;
    [SerializeField] GameObject verticalObject;

    [SerializeField] Text text;

    public void Open()
    {
        Init();

        this.gameObject.SetActive(true);
    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    private void Init()
    {
        if (Random.Range(0, 2) == 0)
        {
            horizonObject.SetActive(true);
            verticalObject.SetActive(false);
        }
        else
        {
            horizonObject.SetActive(false);
            verticalObject.SetActive(true);
        }

        if (horizonObject.activeSelf)
        {
            horizonImage.sprite = horizonSprites[Random.Range(0, horizonSprites.Length)];

            if (horizonImage.sprite.ToString()[0] == 'C')
                text.text = "고맙다냥";
            else if (horizonImage.sprite.ToString()[0] == 'P')
                text.text = "고맙다멍";
            else
                text.text = "감사합니다";
        }
        else if (verticalObject.activeSelf)
        {
            verticalImage.sprite = verticalSprites[Random.Range(0, verticalSprites.Length)];

            if (verticalImage.sprite.ToString()[0] == 'C')
                text.text = "고맙다냥";
            else if (verticalImage.sprite.ToString()[0] == 'P')
                text.text = "고맙다멍";
            else
                text.text = "감사합니다";
        }
        else
            Managers.UI.popupCanvas.Open_ErrorPopup(04, "알 수 없는 에러");
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        Close();
    }
}
