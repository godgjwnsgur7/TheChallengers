using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using UnityEngine.UI;

public class MatchingWindowUI : MonoBehaviour
{
    // [SerializeField] MatchingRoomUI matchingRoom;

    [SerializeField] Text stopwatchText;

    Coroutine timerCoroutine;

    bool isStopwatchLock = false;

    public void Open()
    {
        this.gameObject.SetActive(true);
        isStopwatchLock = false;
        timerCoroutine = StartCoroutine(IStopwatch());

        JoinRoomOrCreateRoom();
    }

    private void JoinRoomOrCreateRoom()
    {
        PhotonLogicHandler.Instance.TryJoinOrCreateRandomRoom(
            CreateOrJoin_MatchingRoom, null, (ENUM_MAP_TYPE)Random.Range(0, (int)ENUM_MAP_TYPE.Max));
    }

    public void CreateOrJoin_MatchingRoom()
    {
        // 임시처리
        PhotonLogicHandler.Instance.ChangeCharacter(ENUM_CHARACTER_TYPE.Knight);

        PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
    }
    
    public void MathingFailed() => OnClick_Exit();

    public void MathingCallBack()
    {
        StopCoroutine(timerCoroutine);
        isStopwatchLock = true;

        Managers.UI.popupCanvas.Play_FadeOutEffect(Managers.UI.currCanvas.GetComponent<LobbyCanvas>().Open_FightingInfoWindow);
    }

    public void OnClick_Exit()
    {
        if (PhotonLogicHandler.IsJoinedRoom)
        {
            bool isLeaveRoom = PhotonLogicHandler.Instance.TryLeaveRoom(LeaveRoom_CallBack);
            
            if(!isLeaveRoom)
            {
                // 음?
                Debug.Log("알 수 없는 이유로 방 나가기 실패");
            }
        }
        else
        {
            LeaveRoom_CallBack();
        }
    }

    private void LeaveRoom_CallBack()
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
