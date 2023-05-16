using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using UnityEngine.UI;

public class MatchingWindowUI : MonoBehaviour
{
    [SerializeField] Text matchingStateText;
    [SerializeField] Text stopwatchText;
    [SerializeField] Image fullLodingImage;
    [SerializeField] GameObject exitButtonObj;

    Coroutine timerCoroutine;

    bool isStopwatchLock = false;

    private void OnDisable()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    public void Open()
    {
        fullLodingImage.gameObject.SetActive(false);
        exitButtonObj.SetActive(true);
        matchingStateText.text = "매칭 중";
        this.gameObject.SetActive(true);

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
        CoroutineHelper.StartCoroutine(IDelayDataSyncCheck(2f));
    }
    
    public void MathingFailed() => OnClick_Exit();

    /// <summary>
    /// 매칭이 됐을 때 콜백
    /// </summary>
    public void MathingCallBack()
    {
        isStopwatchLock = true;
        exitButtonObj.SetActive(false);
        fullLodingImage.gameObject.SetActive(true);
        matchingStateText.text = "매칭 완료!";
    }

    public void OnClick_Exit()
    {
        if (PhotonLogicHandler.IsJoinedRoom)
        {
            PhotonLogicHandler.Instance.TryLeaveRoom(LeaveRoom_CallBack);
        }
        else
        {
            LeaveRoom_CallBack();
        }
    }

    /// <summary>
    /// 매칭 취소버튼
    /// </summary>
    private void LeaveRoom_CallBack()
    {
        StopCoroutine(timerCoroutine);
        isStopwatchLock = true;
        this.gameObject.SetActive(false);
    }

    protected IEnumerator IStopwatch()
    {
        isStopwatchLock = false;
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

            stopwatchText.text = string.Format("{0:00} : {1:00}", minutes, (int)seconds);

            if (PhotonLogicHandler.IsFullRoom)
                MathingCallBack();

            yield return null;
        }

        timerCoroutine = null;
        isStopwatchLock = true;
    }

    protected IEnumerator IDelayDataSyncCheck(float second)
    {
        yield return new WaitUntil(() => PhotonLogicHandler.IsJoinedRoom);

        if (PhotonLogicHandler.IsMasterClient || !PhotonLogicHandler.IsFullRoom)
            yield break;

        yield return new WaitForSeconds(second);

        if (!PhotonLogicHandler.IsMasterClient && PhotonLogicHandler.IsFullRoom
            && !Managers.Network.Get_DataSyncStateAll())
            PhotonLogicHandler.Instance.RequestSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);

    }
}
