using FGDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoginBot : FGBot
{
    MainCanvas mainCanvas;

    public LoginBot(BotController controller) : base(controller)
    {

    }

    public override bool Initialize()
    {
        return true;
    }

    public override IEnumerator ProcessMain()
    {
        yield return SceneManager.LoadSceneAsync(ENUM_SCENE_TYPE.Main.ToString(), LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetActiveScene());

        if(!FindCanvas())
        {
            Fail();
        }

        yield return ProcessLogin();

        Success();
    }

    private bool FindCanvas()
    {
        mainCanvas = controller.Find<MainCanvas>();

        if (mainCanvas == null)
            return false;

        return true;

    }

    private IEnumerator ProcessLogin()
    {
        int count = DefaultRetryCount;

        mainCanvas.OnClick_Login();

        while (!IsValidLogin(Managers.Platform.GetUserID()) && count > 0)
        {
            count--;
            yield return DefaultWaitTime;
        }

        // 시간 내에 못했을 경우 그냥 실패 처리함
        if (count <= 0)
        {
            Fail();
        }
    }

    private bool IsValidLogin(string userId)
    {
        return userId != string.Empty && userId != null;
    }
}
