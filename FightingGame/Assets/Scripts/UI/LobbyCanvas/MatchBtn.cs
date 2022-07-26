using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchBtn : UIElement
{
    Button btn;

    private void Start()
    {
        btn = gameObject.GetComponent<Button>();
    }

    public void SwitchInterctable() 
    {
        btn.interactable = !btn.interactable;
    }
}
