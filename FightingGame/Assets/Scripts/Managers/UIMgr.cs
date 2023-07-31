using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIMgr
{
    BaseCanvas s_CurrCanvas; 

    public BaseCanvas currCanvas
    {
        get
        {
            if(s_CurrCanvas == null)
                s_CurrCanvas = GameObject.FindObjectOfType<BaseCanvas>();

            return s_CurrCanvas;
        }
        set { s_CurrCanvas = value; }
    }
    public PopupCanvas popupCanvas = null;

    Stack<Action> onWindowExitStack = new Stack<Action>();

    bool isExitKeyLock = false;

    public void Init()
    {
        popupCanvas = GameObject.FindObjectOfType<PopupCanvas>();
        if (popupCanvas == null)
        {
            popupCanvas = Managers.Resource.Instantiate("UI/PopupCanvas").GetComponent<PopupCanvas>();
            popupCanvas.Init();
        }
            
        currCanvas = GameObject.FindObjectOfType<BaseCanvas>();
        if (currCanvas == null)
            return;

        currCanvas.Init();
    }

    public void Clear()
    {
        currCanvas = null;
        onWindowExitStack.Clear();
    }

    public void IsExitKeyLock(bool isLockState) => isExitKeyLock = isLockState;

    public void Update_InputBackKeyCheck()
    {
        if (isExitKeyLock)
            return;

        if (UnityEngine.Input.GetKey(KeyCode.Escape))
        {
            if (onWindowExitStack.Count <= 0)
                Exit_Application();
            else
                onWindowExitStack.Peek()?.Invoke();
        }
    }
    
    /// <summary>
    /// null을 넣을 경우, 해당 Window는 뒤로가기로 끌 수 없고
    /// 이 경우, 뒤로가기로 해당 Window를 종료할 수 없음.
    /// </summary>
    public void Push_WindowExitStack(Action _onWindowExit)
    {
        onWindowExitStack.Push(_onWindowExit);
    }

    public void Pop_WindowExitStack()
    {
        if (onWindowExitStack.Count > 0)
            onWindowExitStack.Pop();
    }

    private void Exit_Application()
    {
        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Notify);

        string str = "게임을 종료하시겠습니까?";
        if (Managers.Network.IsServerSyncState || Managers.Battle.isGamePlayingState)
            str = "게임을 종료하시겠습니까?\n진행 중인 게임은 패배처리 됩니다.";

        popupCanvas.Open_SelectPopup(Quit_Application, null, str);
    }

    private void Quit_Application()
    {
        if(Managers.Network.IsServerSyncState || Managers.Battle.isGamePlayingState)
            Managers.Network.Update_DBUserData(false);

        Application.Quit();
    }

    public void OpenUI<T>()
    {
        if (typeof(T).IsSubclassOf(typeof(UIElement))) currCanvas.Open<T>();
        // else if (typeof(T).IsSubclassOf(typeof(PopupUI))) popupCanvas.Open<T>();
        else Debug.Log($"범위 벗어남 : {typeof(T)}");
    }
    
    public void CloseUI<T>()
    {
        if (typeof(T).IsSubclassOf(typeof(UIElement))) currCanvas.Close<T>();
        else Debug.Log($"범위 벗어남 : {typeof(T)}");
    }
}