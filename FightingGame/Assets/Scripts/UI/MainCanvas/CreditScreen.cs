using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScreen : UIElement
{
    // 활성화 시 애니메이션 자동 재생
    [SerializeField] GameObject creditAreaObject;

    public void Open()
    {


        this.gameObject.SetActive(true);
        creditAreaObject.SetActive(true);
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        creditAreaObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
