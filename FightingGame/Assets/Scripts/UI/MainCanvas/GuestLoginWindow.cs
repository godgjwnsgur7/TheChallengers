using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuestLoginWindow : MonoBehaviour
{
    [SerializeField] InputField emailInputField;
    [SerializeField] InputField passwordInputField;

    private void OnEnable()
    {
        emailInputField.text = "";
        passwordInputField.text = "";
    }

    public bool Check_InputField()
    {
        if (string.IsNullOrWhiteSpace(emailInputField.text))
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("이메일을 입력해주세요");
            return false;
        }

        if (string.IsNullOrWhiteSpace(passwordInputField.text))
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("비밀번호를 입력해주세요");
            return false;
        }

        return true;
    }

    public string Get_EmailText() => emailInputField.text;
    public string Get_PasswordText() => passwordInputField.text;
}
