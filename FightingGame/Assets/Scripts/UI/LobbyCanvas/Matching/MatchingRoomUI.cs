using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingRoomUI : MonoBehaviour, IRoomPostProcess
{
    private void OnEnable()
    {
        // 포톤콜백함수 등록
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= EnterSlaveClientCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= ExitSlaveClientCallBack;
        
        PhotonLogicHandler.Instance.onEnterRoomPlayer += EnterSlaveClientCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += ExitSlaveClientCallBack;
        
        this.RegisterRoomCallback();
    }

    private void OnDisable()
    {
        // 포톤콜백함수 해제
        PhotonLogicHandler.Instance.onEnterRoomPlayer -= EnterSlaveClientCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer -= ExitSlaveClientCallBack;

        this.UnregisterRoomCallback();
    }

    public void OnUpdateLobby(List<CustomRoomInfo> roomList)
    {
    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {
        if (PhotonLogicHandler.IsMasterClient)
            return; // 나의 변경된 정보면 리턴

        
    }

    /// <summary>
    /// 슬레이브 클라이언트가 방에 입장하면 불리는 콜백함수
    /// </summary>
    public void EnterSlaveClientCallBack(string nickname)
    {
        if (PhotonLogicHandler.IsMasterClient)
        {
            Managers.Battle.Set_SlaveNickname(nickname);
        }

        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();
    }

    /// <summary>
    /// 자신 외의 다른 클라이언트가 방을 나가면 불리는 콜백함수
    /// ( 이 경우, 자신이 무조건 마스터 클라이언트가 됨 )
    /// </summary>
    public void ExitSlaveClientCallBack(string nickname)
    {

    }
}
