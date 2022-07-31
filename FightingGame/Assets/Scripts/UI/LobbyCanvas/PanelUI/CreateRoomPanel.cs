using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : UIElement
{
    [SerializeField] InputField inputField;
    [SerializeField] Text notice;

    public override void Open(UIParam param = null)
    {
        notice.text = "";
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void CreateCustomScene()
    {
        if (inputContentChk())
            return;

        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.CustomRoom);
    }

    public bool inputContentChk()
    {
        if(string.IsNullOrWhiteSpace(inputField.text))
        {
            notice.text = "방 제목을 입력해주세요.";
            return true;
        }
        else if(!string.IsNullOrWhiteSpace(inputField.text))
        {
            notice.text = "";
            return false;
        }
        else
        {
            notice.text = "알 수 없는 오류";
            return false;
        }
    }
}
