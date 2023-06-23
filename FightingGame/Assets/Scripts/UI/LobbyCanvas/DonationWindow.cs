using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonationWindow : UIElement
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClick_Payment()
    {

    }

    public void OnClick_Advertising()
    {

    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        Close();
    }
}
