using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 현재로써는 모든 행동중에 누르면 쿨타임이 돌아감... 쿨타임 돌리는 시점 생각해봐야할 듯
public class SkillUI : UIElement 
{ 
    public Text text_CoolTime;
    public Image image;
    public Button button;
    public float coolTime = 10.0f;
    public bool isClicked = false;
    float leftTime = 10.0f;
    float speed = 1.0f;

    private void Start()
    {
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillOrigin = (int)Image.Origin360.Top;
        image.fillClockwise = false;
    }

    // Update is called once per frame    
    void Update()
    {
        if (isClicked)
        {
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime * speed;
                if (leftTime < 0)
                {
                    leftTime = 0;
                    if (button)
                        button.enabled = true;
                    isClicked = true;

                    image.gameObject.SetActive(false);
                }
                text_CoolTime.text = ((int)leftTime).ToString();
                float ratio = leftTime / coolTime;
                image.fillAmount = ratio;
            }
        }
    }
    public void StartCoolTime()
    {
        leftTime = coolTime;
        isClicked = true;
        if (button)
            button.enabled = false;
        // 버튼 기능을 해지함.   
        image.gameObject.SetActive(true);
    }
}




/*public GameObject coolTime;
    public Image coolTimeImage;
    [SerializeField] Text text_CoolTime;

    public float time_cooltime = 2;
    private float time_current;
    private float time_start;

    bool isEnded = false;

    void Update()
    {
        if (isEnded)
            return;
        Check_CoolTime();
    }

    public void init()
    {
        coolTimeImage = coolTime.GetComponent<Image>();

        Init_UI();
        Trigger_Skill();
    }

    // 이미지 표기 방법 설정 (원, 위를 기준으로 360도로 채움)
    private void Init_UI()
    {
        coolTimeImage.type = Image.Type.Filled;
        coolTimeImage.fillMethod = Image.FillMethod.Radial360;
        coolTimeImage.fillOrigin = (int)Image.Origin360.Top;
        coolTimeImage.fillClockwise = false;
    }

    private void Trigger_Skill()
    {
        if (!isEnded)
        {
            return;
        }

        Reset_CoolTime();
    }

    private void Check_CoolTime()
    {
        time_current = Time.time - time_start;
        if (time_current < time_cooltime)
        {
            Set_FillAmount(time_cooltime - time_current);
        }
        else if (!isEnded)
        {
            End_CoolTime();
        }
    }

    // 쿨타임 종료
    private void End_CoolTime()
    {
        Set_FillAmount(0);
        isEnded = true;
        coolTime.gameObject.SetActive(false);
        this.GetComponent<Button>().interactable = true;
    }

    // 쿨타임 값 리셋
    private void Reset_CoolTime()
    {
        coolTime.gameObject.SetActive(false);
        time_current = time_cooltime;
        time_start = Time.time;
        Set_FillAmount(time_cooltime);
        isEnded = true;
    }

    // 쿨타임 시간, 게이지 표기
    private void Set_FillAmount(float _value)
    {
        coolTimeImage.fillAmount = _value / time_cooltime;
        string txt = _value.ToString("0.0");
        text_CoolTime.text = txt;
    }

    public void OnCoolTime()
    { 
        time_start = Time.time;
        isEnded = false;
        coolTime.gameObject.SetActive(true);
        this.GetComponent<Button>().interactable = false;
    }*/