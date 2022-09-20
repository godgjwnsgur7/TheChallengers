using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    // 일단 맵은 베이직맵으로 무조건 실행되게 해놓음 (임시) - 전용 이미지도 아직 없음

    [SerializeField] Text masterIDText;
    [SerializeField] Text personnelText;
    public Text inputFieldText;

    private void OnEnable()
    {
        masterIDText.text = "닉네임 받아와야함";
    }

    private void OnDisable()
    {
        masterIDText.text = "유저 닉네임";
    }

    public void OnClick_CreatRoom()
    {
        inputFieldText.text = inputFieldText.text.Trim();

        if(inputFieldText.text == "")
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("방 제목을 입력하지 않았습니다.");
            return;
        }

        PhotonLogicHandler.Instance.TryCreateRoom(OnCreateRoom, null, inputFieldText.text, "닉네임받아야함");

    }

    private void OnCreateRoom()
    {
        Debug.Log("방 생성 성공");
        // 씬 넘어가는 로직 ㅋㅋ
        // 일단... 커스텀룸 씬부터 만들어야할듯 함...
    }
}
