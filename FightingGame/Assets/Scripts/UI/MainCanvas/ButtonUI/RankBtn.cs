using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankBtn : ButtonUI
{
    public override void OnWindowButton()
    {
        base.OnWindowButton();
        Managers.UI.OpenUI<RankWindow>();
    }
}
