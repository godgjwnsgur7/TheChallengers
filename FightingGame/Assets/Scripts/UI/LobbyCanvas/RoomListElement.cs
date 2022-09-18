using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListElement : UIElement
{
    [SerializeField] Image MapImage;
    [SerializeField] Text roomNameText;
    [SerializeField] Text masterNicknameText;

    public Image personnelImage;
    public Text personnelText; // "1 / 2" or "2 / 2"

    public void Set_()
    {

    }
}
