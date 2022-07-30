using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
public class MatchTimeWindow : UIElement
{
    [SerializeField] MatchBtn matchBtn;
    [SerializeField] Text timer;
    private float time = 0f;
    private string[] times = new string[2];
    public ENUM_CHARACTER_TYPE charType;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    private void OnEnable()
    {
        matchBtn.SwitchInterctable();

        StartCoroutine(CountTime());
        // 매칭버튼으로 인해 활성화 시 charType에 맞춰 선택창에 캐릭터이미지 출력

        // 매칭서버 연결?
        
    }

    public void StopMatch() 
    {
        // 서버매칭 종료

        // 매칭창 비활성화
        Managers.UI.CloseUI<MatchTimeWindow>();
        matchBtn.SwitchInterctable();

        time = 0f;
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            time += 1 * Time.deltaTime;
            times[0] = ((int)(time / 60)).ToString();
            times[1] = ((int)(time % 60)).ToString();

            // 분
            if ((int)(time / 60) < 10)
                times[0] = "0" + ((int)(time / 60)).ToString();
            else
                times[0] = ((int)(time / 60)).ToString();

            // 초
            if (((int)(time % 60)) < 10)
                times[1] = "0" + ((int)(time % 60)).ToString();
            else
                times[1] = ((int)(time % 60)).ToString();

            timer.text = times[0] + ":" + times[1];

            if (time >= 90f) // 임시 정지 테스트
                break;

            yield return null;
        }

        StopMatch();
    }
}
