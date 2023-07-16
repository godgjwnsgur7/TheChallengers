using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonationWindow : UIElement
{
    [SerializeField] private Text coffeeText = null;
    [SerializeField] private Text adText = null;
    public void Open()
    {
        gameObject.SetActive(true);
        coffeeText.text = Managers.Platform.GetCoffeePrice().ToString("n0") + "원";
        adText.text = "무료";
	}

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnClick_Payment()
    {
        Managers.Platform.Purchase();
    }

    public void OnClick_Advertising()
    {
        Managers.Platform.ShowRewardedAd();
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        Close();
    }
}
