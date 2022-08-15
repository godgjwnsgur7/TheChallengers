using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
public class MatchTimeWindow : UIElement
{
    [SerializeField] MatchBtn matchBtn;
    [SerializeField] Text timer;
    [SerializeField] Image charImage;

    Sprite[] characterSprite;

    int minute, second;
    public ENUM_CHARACTER_TYPE charType;

    private void Awake()
    {
        characterSprite = Managers.Resource.LoadAll<Sprite>("Image/Knight-Idle");
        minute = second = 0;
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);

        charImage.sprite = characterSprite[(int)charType-1];
        matchBtn.SwitchInterctable();

        StartCoroutine(CountTime());
        // 매칭버튼으로 인해 활성화 시 charType에 맞춰 선택창에 캐릭터이미지 출력

        // 매칭서버 연결?
    }

    public override void Close()
    {
        base.Close();
    }

    public void StopMatch() 
    {
        // 서버매칭 종료

        // 매칭창 비활성화
        Managers.UI.CloseUI<MatchTimeWindow>();
        matchBtn.SwitchInterctable();

        minute = second = 0;
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            second++;

            if(second > 59)
            {
                minute++;
                second = 0;
            }

            if (minute < 10 && second < 10)
                timer.text = "0" + minute + " : 0" + second;
            else if (second < 10)
                timer.text = minute + " : 0" + second;
            else if (minute < 10)
                timer.text = "0" + minute + " : " + second;
            else
                timer.text = minute + " : " + second;

            if (minute * 60 + second >= 61f) // 임시 정지 테스트
                break;

            yield return new WaitForSeconds(1f);
        }

        StopMatch();
    }
}
