using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CustomRoomCanvas : BaseCanvas
{
    [SerializeField] SelectWindow selectWindow;
    [SerializeField] Text readyText1;
    [SerializeField] Text readyText2;
    [SerializeField] Image btnImage1;
    [SerializeField] Image btnImage2;
    [SerializeField] Button user1;
    [SerializeField] Button user2;

    public int player;
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
        switch (this.player)
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

        this.player = player;
    }

    public void CloseSelectWindow()
    {
        Managers.UI.CloseUI<SelectWindow>();
    }

    public void LoadLobby()
    {
        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Lobby);
    }

    public void UserReady()
    {
        // if 문 같은걸로 버튼 누른 유저확인 후 해당 유저의 버튼을 비활성화 시키면 될 거같은데...
        user1.interactable = !user1.interactable;
        user2.interactable = !user2.interactable;

        readyText1.gameObject.SetActive(!readyText1.IsActive());
        readyText2.gameObject.SetActive(!readyText2.IsActive());

        // 이 뒤에 전부 레디했는지 확인하면 되지 않을까 싶다...

    }
}
