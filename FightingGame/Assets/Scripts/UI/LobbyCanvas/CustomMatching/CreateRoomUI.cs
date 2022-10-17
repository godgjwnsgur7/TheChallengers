using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField] CustomRoomWindowUI customRoomWindow;

    [SerializeField] Image mapImage;
    [SerializeField] Text masterIDText;
    [SerializeField] Text personnelText;
    [SerializeField] InputField userInputField;

    ENUM_MAP_TYPE currMap;
    ENUM_MAP_TYPE CurrMap
    {
        set
        {
            currMap = value;
            mapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{value}_M");
        }
        get { return currMap; }
    }

    private void OnEnable()
    {
        masterIDText.text = "닉네임 받아와야함";
        userInputField.text = "";
        CurrMap = ENUM_MAP_TYPE.BasicMap;
    }
    
    public void OnClick_CreatRoom()
    {
        Managers.UI.popupCanvas.Open_LoadingPopup();

        userInputField.text = userInputField.text.Trim();

        if(userInputField.text == "")
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방 제목을 입력하지 않았습니다.");
            Managers.UI.popupCanvas.Close_LoadingPopup();
            return;
        }

        // 금지어 체크해야 함

        PhotonLogicHandler.Instance.TryCreateRoom(Open_CustomRoom, null, userInputField.text, 2, CurrMap);
        Managers.UI.popupCanvas.Close_LoadingPopup();
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
            if (_mapIndex <= 0)
                _mapIndex = (int)ENUM_MAP_TYPE.Max - 1;
        }

        CurrMap = (ENUM_MAP_TYPE)_mapIndex;
    }

    private void Open_CustomRoom()
    {
        customRoomWindow.Open();
        this.gameObject.SetActive(false);
    }
}
