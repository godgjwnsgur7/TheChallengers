using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownPopup : PopupUI
{
    [SerializeField] Text count;

    public override void Close()
    {
        gameObject.SetActive(false);
    }

    public override void Open()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeScene(3));
    }

    IEnumerator FadeScene(int Second)
    {
        while (Second != 0)
        {
            count.text = "" + Second--;
            yield return new WaitForSeconds(1f);
        }

        Managers.UI.CloseUI<CountDownPopup>();
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Battle);
    }
}
