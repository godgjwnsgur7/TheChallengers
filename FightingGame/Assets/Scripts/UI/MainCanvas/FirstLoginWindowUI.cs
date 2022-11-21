using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FirstLoginWindowUI : MonoBehaviour
{
    [SerializeField] InputField userInputField;

    Action<string> nickNameCallBack = null;

    public void Open(Action<string> _nickNameCallBack)
    {
        nickNameCallBack = _nickNameCallBack;
    }

    public void OnClick_Check()
    {
        userInputField.text = userInputField.text.Trim();
        if (userInputField.text == "")
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("닉네임을 입력하지 않았습니다.");
            return;
        }

        // 금지어 체크해야 함

        nickNameCallBack(userInputField.text);
    }
}
