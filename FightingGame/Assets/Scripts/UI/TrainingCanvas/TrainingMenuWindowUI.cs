using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class TrainingMenuWindowUI : UIElement
{
    TrainingScene trainingScene;

    float originalPosX;
    bool isHideState = false;

    Coroutine moveToWindowCoroutine;

    public void Init()
    {
        trainingScene = Managers.Scene.CurrentScene.GetComponent<TrainingScene>();
        originalPosX = this.transform.localPosition.x;
        isHideState = false;
    }

    public void OnClick_ChangeMap()
    {
        Managers.UI.popupCanvas.Open_MapSelectPopup(trainingScene.Change_CurrMap);
    }

    public void OnClick_ChangeEnemyCharacter()
    {
        Managers.UI.popupCanvas.Open_CharSelectPopup(trainingScene.Change_EnemyCharacter);
    }

    public void OnClick_ChangeMyCharacter()
    {
        Managers.UI.popupCanvas.Open_CharSelectPopup(trainingScene.Change_MyCharacter);
    }

    public void OnClick_Setting()
    {
        Managers.UI.popupCanvas.Open_SettingWindow();
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        OnClick_GoToLobby();       
    }

    public void OnClick_GoToLobby() => Managers.UI.popupCanvas.Open_SelectPopup
        (GoTo_LobbyScene, null, "로비로 돌아가시겠습니까?");

    private void GoTo_LobbyScene() => Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);

    public void OnClick_PushOut()
    {
        if (moveToWindowCoroutine != null)
            return;

        if(!isHideState)
        {
            isHideState = true;
            moveToWindowCoroutine = StartCoroutine(IMoveToWindow(originalPosX - 500f));
        }
        else
        {
            isHideState = false;
            moveToWindowCoroutine = StartCoroutine(IMoveToWindow(originalPosX));
        }
    }

    protected IEnumerator IMoveToWindow(float movePosX)
    {
        float time = 0;
        float currPosX = this.transform.localPosition.x;

        while(movePosX != this.transform.localPosition.x)
        {
            time += Time.deltaTime * 5f;

            this.transform.localPosition = new Vector2(
                Mathf.Lerp(currPosX, movePosX, time)
                , this.transform.localPosition.y);

            yield return null;
        }

        moveToWindowCoroutine = null;
    }
}
