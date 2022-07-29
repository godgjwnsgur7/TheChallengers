using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CustomRoomCanvas : BaseCanvas
{
    [SerializeField] SelectWindow selectWindow;
    [SerializeField] Image player1;
    [SerializeField] Image player2;

    private ENUM_CHARACTER_TYPE playerType1;
    private ENUM_CHARACTER_TYPE playerType2;

    public override void Open<T>(UIParam param = null)
    {
        if (typeof(T) == typeof(SelectWindow)) selectWindow.Open();
        else
        {
            Debug.Log("범위 벗어남");
        }
    }

    public override void Close<T>()
    {
        if (typeof(T) == typeof(SelectWindow)) selectWindow.Close();
        else
        {
            Debug.Log("범위 벗어남");
        }
    }

    public void SelectCharacter(int charType) 
    {
        switch (selectWindow.player)
        {
            case 1:
                playerType1 = (ENUM_CHARACTER_TYPE)charType;
                // 버튼 이미지 변경
                // player1.sprite = ~~
                break;

            case 2:
                playerType2 = (ENUM_CHARACTER_TYPE)charType;
                // player2.sprite = ~~
                break;
        }


        CloseSelectWindow();
    }

    public void OpenSelectWindow(int player)
    {
        Managers.UI.OpenUI<SelectWindow>();

        selectWindow.player = player;
    }

    public void CloseSelectWindow()
    {
        Managers.UI.CloseUI<SelectWindow>();
    }

    public void LoadLobby()
    {
        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Lobby);
    }
}
