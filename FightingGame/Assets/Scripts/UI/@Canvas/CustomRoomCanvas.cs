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
    [SerializeField] Text selectCharText1;
    [SerializeField] Text selectCharText2;
    [SerializeField] Text notion;
    [SerializeField] Image readyImage1;
    [SerializeField] Image readyImage2;
    [SerializeField] Image selectCharImage1;
    [SerializeField] Image selectCharImage2;
    [SerializeField] Image mapImage;
    [SerializeField] Button characterBtn1;
    [SerializeField] Button characterBtn2;
    [SerializeField] Button characterBtn3;
    [SerializeField] Button characterBtn4;

    Sprite[] characterSprite;
    Sprite[] mapSprite;
    Sprite[] readySprite;
    private int mapSpriteP = 0;
    public int player;
    // 유저가 선택한 캐릭터 타입
    private ENUM_CHARACTER_TYPE selectCharacter1;
    private ENUM_CHARACTER_TYPE selectCharacter2;
    // 유저 레디 여부
    private bool userReady1;
    private bool userReady2;

    Coroutine notionCoroutine;

    public void init()
    {
        // 필요 UI이미지 불러오기 불러오기
        readySprite = new Sprite[2];
        readySprite[0] = Managers.Resource.Load<Sprite>("Prefabs/UI/CustomMch_btn3-1");
        readySprite[1] = Managers.Resource.Load<Sprite>("Prefabs/UI/CustomMch_btn3-2");
        // 방장이름, 방이름 불러오기
        //userName1.text = PhotonLogicHandler.Instance.CurrentMasterClientNickname;
        //roomNameText.text = PhotonLogicHandler.Instance.CurrentRoomName; 있으면 좋겠다

        // 맵이름 불러오기

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
        if (charType < 0 || charType > 2)
        {
            setNotionText("선택할 수 없는 캐릭터입니다.");
            return;
        }


        if(PhotonLogicHandler.IsMasterClient)
        {
            selectCharacter1 = (ENUM_CHARACTER_TYPE)charType;
            selectCharText1.text = ((ENUM_CHARACTER_TYPE)charType).ToString();
            // 캐릭터 이미지 변경
            //selectCharImage1.sprite = characterSprite[charType - 1];
        }
        else
        {
            selectCharacter2 = (ENUM_CHARACTER_TYPE)charType;
            selectCharText2.text = ((ENUM_CHARACTER_TYPE)charType).ToString();
            //selectCharImage2.sprite = characterSprite[charType - 1];
        }

        setNotionText($"{((ENUM_CHARACTER_TYPE)charType).ToString()}를 선택하셨습니다.");
    }

    // 로비씬 이동
    public void LoadLobby()
    {
        PlayerPrefs.DeleteAll();

        // 마스터 서버 종료있으면 좋을 듯.
        /*PhotonLogicHandler.Instance.OnLeftRoom();
        PhotonLogicHandler.Instance.OnLeftLobby();
        PhotonLogicHandler.Instance.OnLeftRoom(
               () => { PhotonLogicHandler.Instance.OnLeftLobby(); },
              setNotionText);*/
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
        if (PhotonLogicHandler.IsMasterClient)
        {
            if (selectCharacter1 == ENUM_CHARACTER_TYPE.Default)
            {
                //notion.gameObject.SetActive(true);
                setNotionText("캐릭터를 선택해주세요.");
                return;
            }

            SetInteractableButton();
            userReady1 = !userReady1;

            if (userReady1)
            {
                readyImage1.sprite = readySprite[1];
                PhotonLogicHandler.Instance.Ready();
            }
            else
            {
                readyImage1.sprite = readySprite[0];
                PhotonLogicHandler.Instance.UnReady();
            }
        }
        else if (PlayerPrefs.GetString("MyName") == userName2.text)
        {
            if (selectCharacter2 == ENUM_CHARACTER_TYPE.Default)
            {
                notion.gameObject.SetActive(true);
                return;
            }

            SetInteractableButton();
            userReady2 = !userReady2;

            if (userReady2)
            {
                readyImage2.sprite = readySprite[1];
                PhotonLogicHandler.Instance.Ready();
            }
            else
            {
                readyImage2.sprite = readySprite[0];
                PhotonLogicHandler.Instance.UnReady();
            }

            // 전부 레디했는지 확인하면 되지 않을까 싶다...
            if (PhotonLogicHandler.IsFullRoom && PhotonLogicHandler.Instance.IsAllReady())
            {
                Managers.UI.OpenUI<CountDownPopup>();
            }
        }
    }

    private void SetInteractableButton()
    {
        characterBtn1.interactable = !characterBtn1.interactable;
        characterBtn2.interactable = !characterBtn2.interactable;

        // 아직 비활성화
        /*characterBtn3.interactable = !characterBtn3.interactable;
        characterBtn4.interactable = !characterBtn4.interactable;*/
    }

    public void setNotionText(string text)
    {
        if (notionCoroutine != null)
        {
            StopCoroutine(notionCoroutine);
        }

        notionCoroutine = StartCoroutine(ShowNotion(text));
    }

    IEnumerator ShowNotion(string text)
    {
        notion.text = text;
        notion.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        notion.gameObject.SetActive(false);
        notionCoroutine = null;
    }

    /*// 캐릭터 선택창 활성
    public void OpenSelectWindow(int player)
    {
        Managers.UI.OpenUI<SelectWindow>();

        this.player = player;
    }

    // 캐릭터 선택창 비활성
    public void CloseSelectWindow()
    {
        Managers.UI.CloseUI<SelectWindow>();
    }*/
}
