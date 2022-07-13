using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPopupUI : UIElement
{
    GameObject RankPane;
    GameObject CharacterPane;
    GameObject SettingPane;
    private void Start()
    {
        RankPane = gameObject.transform.Find("RankPane").gameObject;
        CharacterPane = gameObject.transform.Find("CharacterPane").gameObject;
        SettingPane = gameObject.transform.Find("SettingPane").gameObject;
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        ClosePane();
        base.Close();
    }

    public void OpenPane(int btn)
    {
        switch (btn) {
            case 3:
                RankPane.SetActive(true);
                break;
            case 4:
                CharacterPane.SetActive(true);
                break;
            case 5:
                SettingPane.SetActive(true);
                break;
        }
    }

    public void ClosePane()
    {
        if (RankPane.activeSelf)
        {
            RankPane.SetActive(false);
        }
        else if (CharacterPane.activeSelf)
        {
            CharacterPane.SetActive(false);
        }
        else if (SettingPane.activeSelf)
        {
            SettingPane.SetActive(false);
        }
    }
}
