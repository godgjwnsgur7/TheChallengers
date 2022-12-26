using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputSkillKey : InputBasicKey
{
    [SerializeField] Image coolTimeImage;
    float coolTime;

    Coroutine coolTimeCoroutine;

    public override void EventTrigger_PointerDown()
    {
        if(coolTimeCoroutine != null)
            return;

        base.EventTrigger_PointerDown();
    }

    public void Set_SkillSetting(float _coolTime, ENUM_CHARACTER_TYPE _charType, int _num)
    {
        coolTime = _coolTime;
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
        float coolTimeFillAmount = 1.0f;

        while (coolTimeFillAmount > 0.01f)
        {
            coolTimeFillAmount -= 1.0f * Time.deltaTime / coolTime;
            coolTimeImage.fillAmount = coolTimeFillAmount;
            yield return null;
        }

        coolTimeImage.fillAmount = 0.0f;
        coolTimeCoroutine = null;
        coolTimeImage.gameObject.SetActive(false);
    }
}
