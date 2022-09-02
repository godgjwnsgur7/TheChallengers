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
        // createUser.text = $"방장 : {}";
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

        // 임시
        PlayerPrefs.SetString("CreateUser", "kuj");
        PlayerPrefs.SetString("MyName", "kuj");
        PlayerPrefs.SetString("MyTeam", "Blue");
        PlayerPrefs.SetString("RoomName", inputField.text);
        PlayerPrefs.SetInt("MapSpriteP", mapSpriteP);

        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.CustomRoom);
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
