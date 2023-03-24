using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        // 금지어 체크해야 함

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
