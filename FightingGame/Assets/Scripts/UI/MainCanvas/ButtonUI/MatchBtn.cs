using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchBtn : ButtonUI
{
    public override void OnWindowButton()
    {
        base.OnWindowButton();
        Managers.UI.OpenUI<MatchWindow>();
    }
}
