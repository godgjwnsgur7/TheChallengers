using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CustomRoomCanvas : BaseCanvas
{
    [SerializeField] SelectWindow selectWindow;
    [SerializeField] Text roomNameText;
    [SerializeField] Text userName1;
    [SerializeField] Text userName2;
    [SerializeField] Text readyText1;
    [SerializeField] Text readyText2;
    [SerializeField] Text notion;
    [SerializeField] Image btnImage1;
    [SerializeField] Image btnImage2;
    [SerializeField] Image mapImage;
    [SerializeField] Button user1;
    [SerializeField] Button user2;

    Sprite[] characterSprite;
    Sprite[] mapSprite;
    private int mapSpriteP = 0;
    public int player;
    // 유저가 선택한 캐릭터 타입
    private ENUM_CHARACTER_TYPE selectCharacter1;
    private ENUM_CHARACTER_TYPE selectCharacter2;
    // 유저 레디 여부
    private bool userReady1;
    private bool userReady2;

    public void init()
    {
        characterSprite = Managers.Resource.LoadAll<Sprite>("Image/Knight-Idle");
        mapSprite = Managers.Resource.LoadAll<Sprite>("Image/test_standing");

        userName1.text = PlayerPrefs.GetString("CreateUser");
        roomNameText.text = PlayerPrefs.GetString("RoomName");

        if (PlayerPrefs.HasKey("EnterUser"))
            userName2.text = PlayerPrefs.GetString("EnterUser");

        mapSpriteP = PlayerPrefs.GetInt("MapSpriteP");
        mapImage.sprite = mapSprite[mapSpriteP];

        userReady1 = false;
        userReady2 = false;
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

    public override T GetUIComponent<T>()
    {

        return default(T);
    }

    // 캐릭터 선택
    public void SelectCharacter(int charType)
    {
        if (notion.gameObject.activeSelf)
            notion.gameObject.SetActive(false);

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
        PlayerPrefs.DeleteAll();
        /*PlayerPrefs.DeleteKey("RoomName");
        PlayerPrefs.DeleteKey("CreateUser");
        PlayerPrefs.DeleteKey("MyName");
        PlayerPrefs.DeleteKey("MyTeam");
        PlayerPrefs.DeleteKey("MapSpriteP");

        if (PlayerPrefs.HasKey("EnterUser"))
            PlayerPrefs.DeleteKey("EnterUser");*/

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
        if (PlayerPrefs.GetString("MyName") == userName1.text)
        {
            if (selectCharacter1 == ENUM_CHARACTER_TYPE.Default)
            {
                notion.gameObject.SetActive(true);
                return;
            }

            user1.interactable = !user1.interactable;
            readyText1.gameObject.SetActive(!readyText1.IsActive());
            userReady1 = !userReady1;
        }
        else if(PlayerPrefs.GetString("MyName") == userName2.text)
        {
            if (selectCharacter2 == ENUM_CHARACTER_TYPE.Default)
            {
                notion.gameObject.SetActive(true);
                return;
            }

            user2.interactable = !user2.interactable;
            readyText2.gameObject.SetActive(!readyText2.IsActive());
            userReady2 = !userReady2;
        }

        // 이 뒤에 전부 레디했는지 확인하면 되지 않을까 싶다...
        if(userReady1 && userReady2)
        {
            Managers.UI.OpenUI<CountDownPopup>();
        }
    }
}
