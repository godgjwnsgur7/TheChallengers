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
        Managers.UI.popupCanvas.Open_CharSelectPopup(Check_MathingStart);
    }

    public void Check_MathingStart(ENUM_CHARACTER_TYPE charType)
    {
        selectedCharType = charType;

        Managers.UI.popupCanvas.Open_SelectPopup(MathingStart, null,
            $"'{Managers.Battle.Get_CharNameDict(charType)}'캐릭터로 랭킹전 매칭을 시작하시겠습니까?");
    }

    public void MathingStart()
    {
        this.gameObject.SetActive(true);
        isStopwatchLock = false;
        timerCoroutine = StartCoroutine(IStopwatch());

        Debug.Log(selectedCharType);

        // PhotonLogicHandler.Instance.TryJoinOrCreateRandomRoom();
        // 매칭 돌리기 선택한 캐릭터는 selectCharType로 확인하면 됨
    }


    public void OnClick_Exit()
    {
        StopCoroutine(timerCoroutine);
        isStopwatchLock = true;
        this.gameObject.SetActive(false);
    }

    protected IEnumerator IStopwatch()
    {
        float startTime = Time.time;
        float elapsedSecondTime;
        int minute = 0;

        while (!isStopwatchLock)
        {
            elapsedSecondTime = Time.time - startTime;

            if(elapsedSecondTime >= 60.0f)
            {
                minute += 1;
                startTime += 60.0f;
            }

            stopwatchText.text = string.Format("{0:00}:{1:00.00}", minute, elapsedSecondTime);

            yield return null;
        }
    }

}
