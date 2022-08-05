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
    [SerializeField] Image mapImage;
    [SerializeField] Button user1;
    [SerializeField] Button user2;

    Sprite[] characterSprite;
    Sprite[] mapSprite;
    private int mapSpriteP = 0;
    public int player;
    private ENUM_CHARACTER_TYPE selectCharacter1;
    private ENUM_CHARACTER_TYPE selectCharacter2;

    private void Start()
    {
        characterSprite = Managers.Resource.LoadAll<Sprite>("Image/Knight-Idle");
        mapSprite = Managers.Resource.LoadAll<Sprite>("Image/test_standing");
        // mapSpriteP = ;
    }

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

    // 캐릭터 선택
    public void SelectCharacter(int charType)
    {
        switch (this.player)
        {
            case 1:
                selectCharacter1 = (ENUM_CHARACTER_TYPE)charType;
                // 버튼 이미지 변경
                btnImage1.sprite = characterSprite[charType-1];
                break;

            case 2:
                selectCharacter2 = (ENUM_CHARACTER_TYPE)charType;
                btnImage2.sprite = characterSprite[charType-1];
                break;
        }


        CloseSelectWindow();
    }

    // 캐릭터 선택창 활성
    public void OpenSelectWindow(int player)
    {
        Managers.UI.OpenUI<SelectWindow>();

        this.player = player;
    }

    // 캐릭터 선택창 비활성
    public void CloseSelectWindow()
    {
        Managers.UI.CloseUI<SelectWindow>();
    }

    // 로비씬 이동
    public void LoadLobby()
    {
        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Lobby);
    }

    // 맵 변경
    public void LMoveMap()
    {
        mapSpriteP--;
        if (mapSpriteP < 0)
            mapSpriteP = mapSprite.Length - 1;

        mapImage.sprite = mapSprite[mapSpriteP];
    }

    public void RMoveMap()
    {
        mapSpriteP++;
        if (mapSpriteP > mapSprite.Length - 1)
            mapSpriteP = 0;

        mapImage.sprite = mapSprite[mapSpriteP];
    }

    // 유저의 Reddy
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
