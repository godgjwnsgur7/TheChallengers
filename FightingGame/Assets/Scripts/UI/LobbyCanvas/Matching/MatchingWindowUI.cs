using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using UnityEngine.UI;

public class MatchingWindowUI : MonoBehaviour
{
    [SerializeField] Text matchingStateText;
    [SerializeField] Text stopwatchText;
    [SerializeField] Text statusDescriptionText;
    [SerializeField] Image fullLodingImage;
    [SerializeField] GameObject exitButtonObj;
    [SerializeField] GameObject deactiveExitButtonObj;

    Coroutine timerCoroutine;
    Coroutine matchingErrorCheckCoroutine;

    bool isStopwatchLock = false;
    bool matchingErrorCheckLock = false;

    private void OnDisable()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        if (matchingErrorCheckCoroutine != null)
            StopCoroutine(matchingErrorCheckCoroutine);
    }

    public void Open()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_MacthingStart);
        fullLodingImage.gameObject.SetActive(false);
        exitButtonObj.SetActive(true);
        deactiveExitButtonObj.SetActive(false);
        matchingStateText.text = "매칭 중";
        statusDescriptionText.text = "다른 유저와 매칭 중입니다.";
        this.gameObject.SetActive(true);

        matchingErrorCheckLock = true;
        timerCoroutine = StartCoroutine(IStopwatch());

        JoinRoomOrCreateRoom();
    }

    public void Close()
    {
        if(!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Cancel);

        matchingErrorCheckLock = false;
        this.gameObject.SetActive(false);
    }

    private void JoinRoomOrCreateRoom()
    {
        PhotonLogicHandler.Instance.TryJoinOrCreateRandomRoom(
            CreateOrJoin_CallBack, null, (ENUM_MAP_TYPE)Random.Range(0, (int)ENUM_MAP_TYPE.Max));
    }

    public void CreateOrJoin_CallBack()
    {
        PhotonLogicHandler.Instance.RequestSyncData(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC);
    }

    /// <summary>
    /// 매칭이 됐을 때 콜백
    /// </summary>
    public void Matching_CallBack()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_MacthingCompleted);
        isStopwatchLock = true;
        exitButtonObj.SetActive(false);
        deactiveExitButtonObj.SetActive(true);
        fullLodingImage.gameObject.SetActive(true);
        statusDescriptionText.text = "곧 게임이 시작됩니다.";
        matchingStateText.text = "매칭 완료!";

        matchingErrorCheckCoroutine = StartCoroutine(IMatchingErrorCheck());
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
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Cancel);

        if(timerCoroutine != null)
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

            if (PhotonLogicHandler.IsJoinedRoom && PhotonLogicHandler.IsFullRoom)
                Matching_CallBack();

            yield return null;
        }

        timerCoroutine = null;
        isStopwatchLock = true;
    }

    protected IEnumerator IMatchingErrorCheck()
    {
        yield return new WaitForSeconds(4f); // 4초 대기 후 에러상태 체크

        if (matchingErrorCheckLock && PhotonLogicHandler.IsJoinedRoom)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup(
               "매칭에 실패했습니다.\n다시 시도해주세요.", OnClick_Exit);
            yield break;
        }
    }
}
