using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using UnityEngine.UI;

public class MatchingWindowUI : MonoBehaviour
{
    [SerializeField] MatchingRoomUI matchingRoom;

    [SerializeField] Text stopwatchText;

    Coroutine timerCoroutine;

    bool isStopwatchLock = false;

    ENUM_CHARACTER_TYPE selectedCharType;

    public void OnClick_Matching()
    {
        Managers.UI.popupCanvas.Open_CharSelectPopup(CallBack_MathingStart);
    }

    public void CallBack_MathingStart(ENUM_CHARACTER_TYPE charType)
    {
        selectedCharType = charType;

        Managers.UI.popupCanvas.Open_SelectPopup(MathingStart, null,
            $"'{Managers.Battle.Get_CharNameDict(charType)}'캐릭터로 랭킹전 매칭을 시작하시겠습니까?");
    }

    private void MathingStart()
    {
        this.gameObject.SetActive(true);
        isStopwatchLock = false;
        timerCoroutine = StartCoroutine(IStopwatch());

        PhotonLogicHandler.Instance.TryJoinLobby(ENUM_MATCH_TYPE.RANDOM, JoinRoomOrCreateRoom);
    }

    private void JoinRoomOrCreateRoom()
    {
        PhotonLogicHandler.Instance.TryJoinOrCreateRandomRoom(
            Create_MatchingRoom, null, (ENUM_MAP_TYPE)Random.Range(0, (int)ENUM_MAP_TYPE.Max));
    }

    public void Create_MatchingRoom() => matchingRoom.Open(MathingCallBack, selectedCharType);
    public void MathingFailed() => OnClick_Exit();

    public void MathingCallBack()
    {
        StopCoroutine(timerCoroutine);
        isStopwatchLock = true;

        Managers.UI.popupCanvas.Play_FadeInEffect(Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_FightingInfoWindow);
    }

    public void OnClick_Exit()
    {
        if (PhotonLogicHandler.IsJoinedRoom)
        {
            bool isLeaveRoom = PhotonLogicHandler.Instance.TryLeaveRoom(LeaveRoom_CallBack);
            
            if(!isLeaveRoom)
            {
                Managers.UI.popupCanvas.Open_NotifyPopup("알 수 없는 오류");
                // 여기도 알 수 없는 오륜데...
            }
        }
        else
        {
            LeaveRoom_CallBack();
        }
    }

    private void LeaveRoom_CallBack()
    {
        bool isLeaveLobby = PhotonLogicHandler.Instance.TryLeaveLobby(LeaveLobby_CallBack);

        if(!isLeaveLobby)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("알 수 없는 오류");
            // 알 수 없는 오류? 강제로그인해제 하던가 해야하나?...
        }
    }

    private void LeaveLobby_CallBack()
    {
        StopCoroutine(timerCoroutine);
        isStopwatchLock = true;
        this.gameObject.SetActive(false);
    }

    protected IEnumerator IStopwatch()
    {
        float seconds = 0;
        int minutes = 0;

        while (!isStopwatchLock)
        {
            seconds += Time.deltaTime;
            
            if((int)seconds >= 60)
            {
                seconds -= 60;
                minutes++;
            }

            stopwatchText.text = string.Format("{0:00}:{1:00}", minutes, (int)seconds);

            yield return null;
        }

        timerCoroutine = null;
    }

}
