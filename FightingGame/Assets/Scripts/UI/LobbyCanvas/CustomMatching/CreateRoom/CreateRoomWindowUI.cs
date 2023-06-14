using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using FGDefine;

public class CreateRoomWindowUI : MonoBehaviour
{
    [SerializeField] InputField userInputField;
    [SerializeField] Image inputFieldLineImage;
    [SerializeField] Image inputFieldEditImage;

    [SerializeField] CustormRoom_MapInfo mapInfo;


    ENUM_MAP_TYPE currMap = ENUM_MAP_TYPE.CaveMap;

    private void Open_CustomRoom()
    {
        Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_CustomRoomWindow();
    }

    private void Init()
    {
        userInputField.text = "";
        userInputField.characterLimit = Managers.Data.RoomNameTextLimit;
        
        userInputField.onValueChanged.RemoveAllListeners(); 
        userInputField.onValueChanged.AddListener(
            (word) => userInputField.text = Regex.Replace(word, @"[^0-9a-zA-Z가-힣\!\?\~)]", "")
        );
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

    public void OnClick_ChangeMap(int _mapTypeNum)
    {
        currMap = (ENUM_MAP_TYPE)_mapTypeNum;
        mapInfo.Set_CurrMapInfo(currMap);
    }

    public void OnClick_CreatRoom()
    {
        userInputField.text = userInputField.text.Trim();

        if (userInputField.text == "")
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방 제목을 입력하지 않았습니다.");
            return;
        }
        else if(Managers.Data.BadWord_Discriminator(userInputField.text))
        {
            userInputField.text = "";
            Managers.UI.popupCanvas.Open_NotifyPopup("올바르지 않은 방 제목입니다.");
            return;
        }

        PhotonLogicHandler.Instance.TryCreateRoom(userInputField.text, CreateRoomSuccessCallBack, null, true, 2, currMap);
    }

    private void CreateRoomSuccessCallBack()
    {
        Managers.UI.popupCanvas.Play_FadeOutEffect(Open_CustomRoom);
    }

    public void InputField_ValueChange()
    {
        inputFieldLineImage.color = Managers.Data.Get_SelectColor();
        inputFieldEditImage.color = Managers.Data.Get_SelectColor();
    }

    public void InputField_EndEdit()
    {
        inputFieldLineImage.color = Managers.Data.Get_DeselectColor();
        inputFieldEditImage.color = Managers.Data.Get_DeselectColor();
    }
}
