using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : UIElement
{
    [SerializeField] Button closeBtn;
    [SerializeField] Button saveBtn;
    [SerializeField] Button resetBtn;
    [SerializeField] NotionPopup notionPopup;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    // 팝업창 Text 설정 및 Open
    public void OpenNotionPopup(int BtnType)
    {
        notionPopup.SetNotion(BtnType);
        Managers.UI.OpenUI<NotionPopup>();
    }
}
