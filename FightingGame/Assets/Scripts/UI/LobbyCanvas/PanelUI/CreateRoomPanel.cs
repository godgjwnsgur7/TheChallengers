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

    private void Awake() // 맵 리소스 생성
    {
        mapSprite = Managers.Resource.LoadAll<Sprite>("Image/test_standing");
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);

        // 기본 정보 초기화
        // createUser.text = $"방장 : {}";
        notice.text = "";
        mapImage.sprite = mapSprite[0];
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
        if (inputContentChk())
            return;

        // 임시
        PlayerPrefs.SetString("CreateUser", "kuj");
        PlayerPrefs.SetString("MyName", "kuj");
        PlayerPrefs.SetString("MyTeam", "Blue");
        PlayerPrefs.SetString("RoomName", inputField.text);
        PlayerPrefs.SetInt("MapSpriteP", mapSpriteP);

        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.CustomRoom);
    }

    // 방 제목 입력 여부 확인
    public bool inputContentChk()
    {
        if(string.IsNullOrWhiteSpace(inputField.text))
        {
            notice.text = "방 제목을 입력해주세요.";
            return true;
        }
        else if(!string.IsNullOrWhiteSpace(inputField.text))
        {
            notice.text = "";
            return false;
        }
        else
        {
            notice.text = "알 수 없는 오류";
            return false;
        }
    }
}
