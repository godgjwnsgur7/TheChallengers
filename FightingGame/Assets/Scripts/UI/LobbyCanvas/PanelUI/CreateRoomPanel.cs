using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomPanel : UIElement
{
    [SerializeField] InputField inputField;
    [SerializeField] Text notice;
    [SerializeField] Text createUser;
    [SerializeField] Image mapImage;

    Sprite[] mapSprite;
    private int mapSpriteP;
    Coroutine notionCoroutine;

    public void init()
    {
        //mapSprite = Managers.Resource.LoadAll<Sprite>("Image/test_standing");
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);

        // 기본 정보 초기화
        createUser.text = PlayerPrefs.GetString("LoginUser");
        notice.text = "";
        //mapImage.sprite = mapSprite[0];
        mapSpriteP = 0;
    }

    public override void Close()
    {
        base.Close();
    }

    // 맵 변경
    public void LMoveMap()
    {
        mapSpriteP--;
        if (mapSpriteP < 0)
            mapSpriteP = mapSprite.Length - 1;

        mapImage.sprite = mapSprite[mapSpriteP];

        // 서버에 맵정보 변경해야할 거 같은데..
    }

    public void RMoveMap()
    {
        mapSpriteP++;
        if (mapSpriteP > mapSprite.Length - 1)
            mapSpriteP = 0;

        mapImage.sprite = mapSprite[mapSpriteP];
    }

    // 커스텀씬 이동
    public void CreateCustomScene()
    {
        if (string.IsNullOrWhiteSpace(inputField.text))
        {
            setNotionText("방 제목을 입력해주세요.");
            return;
        }

        // 여기서 방을 생성하면 좋을 거 같음...
        if (PhotonLogicHandler.IsConnected)
            OnClickJoinLobby();
        else
            OnClickMasterServer();
    }

    public void OnClickMasterServer()
    {
        bool a = PhotonLogicHandler.Instance.TryConnectToMaster(
            () => { OnClickJoinLobby(); },
            SetError);
    }

    public void OnClickJoinLobby()
    {
        PhotonLogicHandler.Instance.TryJoinLobby(
               () => { OnClickCreateRoom();},
              SetError);

    }

    public void OnClickCreateRoom()
    {
        PhotonLogicHandler.Instance.TryCreateRoom(
        OnCreateRoom: () => { Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.CustomRoom); },
        OnCreateRoomFailed: null,
        masterClientNickname: createUser.text);
    }

    public void SetError(string cause)
    {
        setNotionText($"{cause} - 해당 사유로 접속 실패, 혹은 끊어짐");
    }

    public void SetError(short returnCode, string message)
    {
        setNotionText($"{returnCode} - {message}");
    }

    public void setNotionText(string text)
    {
        if(notionCoroutine != null)
        {
            StopCoroutine(notionCoroutine);
        }

        notionCoroutine = StartCoroutine(ShowNotion(text));
    }

    IEnumerator ShowNotion(string text)
    {
        notice.text = text;
        notice.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        notice.gameObject.SetActive(false);
        notionCoroutine = null;
    }
}
