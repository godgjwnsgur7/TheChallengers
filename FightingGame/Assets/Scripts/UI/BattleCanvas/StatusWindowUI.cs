using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class StatusWindowUI : MonoBehaviour
{
    [SerializeField] ENUM_TEAM_TYPE teamType;
    [SerializeField] Image charFrameImage;
    [SerializeField] Image HpFill;

    private float currHP;
    public float maxHP;

    Coroutine hpBarCoroutine;

    public void Set_StatusWindowUI(ENUM_CHARACTER_TYPE _charType, float _maxHP)
    {
        maxHP = _maxHP;
        currHP = _maxHP;
        Set_CharFrameImage(_charType);

        Update_CurrHP(maxHP);
    }

    public void Set_CharFrameImage(ENUM_CHARACTER_TYPE _charType)
    {
        switch (_charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:
                // 이미지 아직 없음
                break;
            case ENUM_CHARACTER_TYPE.Wizard:
                // 이미지 아직 없음
                break;
            default:
                Debug.Log($"{_charType} 를 찾을 수 없음");
                break;
        }
    }

    public void Update_CurrHP(float _currHP)
    {
        currHP = _currHP;

        if (hpBarCoroutine != null)
            StopCoroutine(hpBarCoroutine);

        if (currHP > 0)
            hpBarCoroutine = StartCoroutine(IFadeHpBar(currHP / maxHP));
        else
            hpBarCoroutine = StartCoroutine(IFadeHpBar(0f));
    }

    protected IEnumerator IFadeHpBar(float _goalHPValue)
    {
        while (_goalHPValue < HpFill.fillAmount)
        {
            HpFill.fillAmount -= 0.01f;
            yield return null;
        }

        HpFill.fillAmount = _goalHPValue;
        hpBarCoroutine = null;
    }
}