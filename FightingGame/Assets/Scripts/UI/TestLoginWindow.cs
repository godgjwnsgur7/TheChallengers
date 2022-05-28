using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestLoginWindow : MonoBehaviour
{
    [SerializeField] private InputField emailInputField = null;
    [SerializeField] private InputField passwordInputField = null;

    public void OnClickLogin()
    {
        string currEmail = emailInputField.text;
        string currPassword = passwordInputField.text;

        var loginScene = Managers.Scene.CurrentScene as LoginScene; // 이렇게 로그인 씬을 가져오는 방식은 클라가 마음껏 수정하면 될 듯, 그냥 예시임다
        loginScene.SignIn(ENUM_LOGIN_TYPE.Guest, currEmail, currPassword);
    }
}
