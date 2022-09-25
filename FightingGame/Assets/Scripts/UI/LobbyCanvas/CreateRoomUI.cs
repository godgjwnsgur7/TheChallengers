using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CreateRoomUI : MonoBehaviour
{
    // 일단 맵은 베이직맵으로 무조건 실행되게 해놓음 (임시) - 전용 이미지도 아직 없음

    [SerializeField] CustomRoomWindowUI customRoomWindow;

    [SerializeField] Text masterIDText;
    [SerializeField] Text personnelText;
    [SerializeField] InputField userInputField;

    private void OnEnable()
    {
        masterIDText.text = "닉네임 받아와야함";
        userInputField.text = "";
    }
    
    public void OnClick_CreatRoom()
    {
        Managers.UI.popupCanvas.Open_LoadingPopup();

        userInputField.text = userInputField.text.Trim();

        if(userInputField.text == "")
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방 제목을 입력하지 않았습니다.");
            return;
        }
        
        // 금지어 체크해야 함
    
        PhotonLogicHandler.Instance.TryCreateRoom(Open_CustomRoom, null, userInputField.text, "닉네임받아야함");
        
        Managers.UI.popupCanvas.Close_LoadingPopup();
    }

    private void Open_CustomRoom()
    {
        customRoomWindow.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
