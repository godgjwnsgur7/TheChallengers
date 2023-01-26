using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CreateRoomWindowUI : MonoBehaviour
{
    [SerializeField] Image mapImage;
    [SerializeField] Text mapNameText;
    [SerializeField] Text personnelText;
    [SerializeField] InputField userInputField;

    ENUM_MAP_TYPE currMap;
    ENUM_MAP_TYPE CurrMap
    {
        set { CurrMapInfoUpdateCallBack(value); }
        get { return currMap; }
    }

    private void Open_CustomRoom()
    {
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_CustomMatchingWindow();
    }
    public void CurrMapInfoUpdateCallBack(ENUM_MAP_TYPE _mapType)
    {
        currMap = _mapType;
        mapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
        mapNameText.text = Managers.Data.Get_MapNameDict(_mapType);
    }

    public void Open()
    {
        userInputField.text = "";
        CurrMap = ENUM_MAP_TYPE.ForestMap;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    
    public void OnClick_CreatRoom()
    {
        userInputField.text = userInputField.text.Trim();

        if(userInputField.text == "")
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방 제목을 입력하지 않았습니다.");
            return;
        }

        // 금지어 체크해야 함

        PhotonLogicHandler.Instance.TryCreateRoom(userInputField.text, Open_CustomRoom, null, true, 2, CurrMap);
    }

    public void OnClick_ChangeMap(bool _isUp)
    {
        int _mapIndex = (int)CurrMap;

        if (_isUp)
        {
            _mapIndex += 1;
            if (_mapIndex >= (int)ENUM_MAP_TYPE.Max)
                _mapIndex = 0;
        }
        else
        {
            _mapIndex -= 1;
            if (_mapIndex < 0)
                _mapIndex = (int)ENUM_MAP_TYPE.Max - 1;
        }

        CurrMap = (ENUM_MAP_TYPE)_mapIndex;
    }

}
