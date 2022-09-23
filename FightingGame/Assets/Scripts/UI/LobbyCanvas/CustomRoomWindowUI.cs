using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomRoomWindowUI : MonoBehaviour
{
    [SerializeField] Image currMapIamge;
    [SerializeField] Image nextMapIamge_Left;
    [SerializeField] Image nextMapIamge_Right;

    [SerializeField] CharProfileUI masterProfile;
    [SerializeField] CharProfileUI slaveProfile;

    public void OnClick_ChangeMap()
    {
        // 함수 두개로 나눌지, 하나의 함수에 인자를 줄지 고민중
    }

    public void OnClick_ExitRoom()
    {
        // 누르자 마자 준비해제 되야함

        Managers.UI.popupCanvas.Open_SelectPopup(ExitRoom, null, "정말 방에서 나가시겠습니까?");
    }

    public void ExitRoom()
    {
        Debug.Log("방을 나갑니다 ㅎㅎ - 미구현");
    }
}
