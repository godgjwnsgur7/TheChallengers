using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackOutPopup : PopupUI
{
    Image img;
    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        img = gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeScene());
    }

    IEnumerator FadeScene()
    {
        Color color = img.color;
        while (img.color.a != 255f) 
        {
            color.a += 15f;
            img.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        color.a = 0f;
        img.color = color;

        Managers.UI.CloseUI<BlackOutPopup>();
    }
}
