using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class TrainingMenuWindowUI : UIElement
{
    [SerializeField] GameObject moveCriteriaGroupObject;
    [SerializeField] GameObject menuOutStateObject;
    [SerializeField] GameObject menuInStateObject;

    TrainingScene trainingScene;

    float originalPosX;
    bool isHideState = false;

    Coroutine moveToWindowCoroutine;

    public void Init()
    {
        trainingScene = Managers.Scene.CurrentScene.GetComponent<TrainingScene>();
        originalPosX = moveCriteriaGroupObject.transform.localPosition.x;
        Set_HideMenuState(false);
    }

    public void OnClick_ChangeMap()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Light);
        Managers.UI.popupCanvas.Open_MapSelectPopup(trainingScene.Change_CurrMap);
    }

    public void OnClick_ChangeEnemyCharacter()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Light);
        Managers.UI.popupCanvas.Open_CharSelectPopup(trainingScene.Change_EnemyCharacter);
    }

    public void OnClick_ChangeMyCharacter()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Light);
        Managers.UI.popupCanvas.Open_CharSelectPopup(trainingScene.Change_MyCharacter);
    }

    public void OnClick_Setting()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Light);
        Managers.UI.popupCanvas.Open_SettingWindow();
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        OnClick_GoToLobby();       
    }

    public void OnClick_GoToLobby()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Notify);
        Managers.UI.popupCanvas.Open_SelectPopup
        (GoTo_LobbyScene, null, "로비로 돌아가시겠습니까?");
    }

    private void GoTo_LobbyScene() => Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);

    private void Set_HideMenuState(bool ishideState)
    {
        this.isHideState = ishideState;
        menuOutStateObject.SetActive(!ishideState);
        menuInStateObject.SetActive(ishideState);
    }

    public void OnClick_PushOut()
    {
        if (moveToWindowCoroutine != null)
            return;

        if(!isHideState)
        {
            Set_HideMenuState(true);
            moveToWindowCoroutine = StartCoroutine(IMoveToWindow(originalPosX - 500f));
        }
        else
        {
            Set_HideMenuState(false);
            moveToWindowCoroutine = StartCoroutine(IMoveToWindow(originalPosX));
        }
    }

    protected IEnumerator IMoveToWindow(float movePosX)
    {
        float time = 0;
        float currPosX = moveCriteriaGroupObject.transform.localPosition.x;

        while(movePosX != moveCriteriaGroupObject.transform.localPosition.x)
        {
            time += Time.deltaTime * 5f;

            moveCriteriaGroupObject.transform.localPosition = new Vector2(
                Mathf.Lerp(currPosX, movePosX, time)
                , moveCriteriaGroupObject.transform.localPosition.y);

            yield return null;
        }

        moveToWindowCoroutine = null;
    }
}
