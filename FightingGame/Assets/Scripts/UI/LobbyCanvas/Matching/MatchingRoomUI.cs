using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class MatchingRoomUI : MonoBehaviour, IRoomPostProcess
{
    Action matchingCallBack;

    bool isStarted;

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

    public void Open(Action _matchingCallBack, ENUM_CHARACTER_TYPE _selectCharType)
    {
        PhotonLogicHandler.Instance.ChangeCharacter(_selectCharType);
        Managers.Battle.Set_MyCharacterType(_selectCharType);
        matchingCallBack = _matchingCallBack;

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
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

        PhotonLogicHandler.Instance.OnGameStart();

        isStarted = true;
        matchingCallBack();
    }

    /// <summary>
    /// 자신 외의 다른 클라이언트가 방을 나가면 불리는 콜백함수
    /// ( 이 경우, 자신이 무조건 마스터 클라이언트가 됨 )
    /// </summary>
    public void ExitSlaveClientCallBack(string nickname)
    {
        // 강제종료했을 때 불릴 것으로 예상 중. (구현, 처리해야 함)
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {
        if (PhotonLogicHandler.IsMasterClient || isStarted)
            return; // 나의 변경된 정보이거나 시작중이라면 리턴

        if(property.isStarted) // 게임 시작을 알림받음
        {
            isStarted = true;
            matchingCallBack();
        }
    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        if (property.isMasterClient == PhotonLogicHandler.IsMasterClient)
            return; // 나의 변경된 정보면 리턴

        Debug.Log("상대에게 정보를 받아 갱신합니다.");
        Managers.Battle.Set_EnemyCharacterType(property.characterType);
    }
}
