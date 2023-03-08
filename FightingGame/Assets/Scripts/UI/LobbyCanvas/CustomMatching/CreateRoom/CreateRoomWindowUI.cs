using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CreateRoomWindowUI : MonoBehaviour
{
    [SerializeField] Image mapImage;
    [SerializeField] Text mapNameText;
    [SerializeField] InputField userInputField;

    [SerializeField] SelectableMapElement[] selectableMapElements = new SelectableMapElement[3];
    [SerializeField] GameObject[] inputFieldEventObject = new GameObject[2];

    ENUM_MAP_TYPE currMap = ENUM_MAP_TYPE.ForestMap;

    private void Open_CustomRoom()
    {
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_CustomRoomWindow();
    }

    private void Init()
    {
        userInputField.text = "";

        for(int i = 0; i < selectableMapElements.Length; i++)
            selectableMapElements[i].Init(CallBack_ChangeMap);

        selectableMapElements[0].OnClick_ChangeMap();
    }

    public void Open()
    {
        Init();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void CallBack_ChangeMap(ENUM_MAP_TYPE _mapType)
    {
        if (currMap == _mapType)
            return;

        currMap = _mapType;
        Set_ChangeMapImage();
    }

    private void Set_ChangeMapImage()
    {
        Sprite _mapImageSprite;

        for(int i = 0; i < selectableMapElements.Length; i++)
        {
            _mapImageSprite = selectableMapElements[i].Get_MapImageSprite(currMap);

            if(_mapImageSprite != null)
            {
                mapImage.sprite = _mapImageSprite;
            }
        }

        mapNameText.text = Managers.Data.Get_MapNameDict(currMap);
    }
   
    public void OnClick_CreatRoom()
    {
        userInputField.text = userInputField.text.Trim();

        if (userInputField.text == "")
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방 제목을 입력하지 않았습니다.");
            return;
        }

        // 금지어 체크해야 함

        PhotonLogicHandler.Instance.TryCreateRoom(userInputField.text, Open_CustomRoom, null, true, 2, currMap);
    }

    public void InputField_ValueChange()
    {
        for (int i = 0; i < inputFieldEventObject.Length; i++)
            inputFieldEventObject[i].SetActive(true);
    }

    public void InputField_EndEdit()
    {
        for (int i = 0; i < inputFieldEventObject.Length; i++)
            inputFieldEventObject[i].SetActive(false);
    }
}
