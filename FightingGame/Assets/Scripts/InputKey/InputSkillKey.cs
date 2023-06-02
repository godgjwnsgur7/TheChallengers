using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputSkillKey : InputBasicKey
{
    [SerializeField] Image coolTimeImage;
    [SerializeField] Image coolTimeCoverImage;
    [SerializeField] Text coolTimeText;
    
    Coroutine coolTimeCoroutine;
    float coolTime;

    public override void EventTrigger_PointerDown()
    {
        if(coolTimeCoroutine != null)
            return;

        base.EventTrigger_PointerDown();
    }

    public void Set_SkillInfo(float _coolTime, ENUM_CHARACTER_TYPE _charType, int _num)
    {
        coolTime = _coolTime;
        
        if(_num != 0) // 대쉬스킬 예외처리
            iconImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/SkillKey/Icon_{_charType}Skill{_num}");
    }

    public void Use_Skill()
    {
        coolTimeImage.fillAmount = 1.0f;
        coolTimeImage.gameObject.SetActive(true);
        coolTimeCoroutine = StartCoroutine(ICoolTime());
    }

    protected IEnumerator ICoolTime()
    {
        float currCoolTime = coolTime;
        coolTimeText.text = ((int)coolTime + 1).ToString();

        while (0 < currCoolTime)
        {
            currCoolTime -= Time.deltaTime;
            coolTimeImage.fillAmount = currCoolTime / coolTime;
            coolTimeText.text = ((int)currCoolTime + 1).ToString();

            yield return null;
        }

        coolTimeImage.fillAmount = 0.0f;
        coolTimeText.text = "";
        coolTimeCoroutine = null;
        coolTimeImage.gameObject.SetActive(false);
    }
}
