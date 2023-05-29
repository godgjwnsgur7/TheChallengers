using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class StatusWindowUI : MonoBehaviour
{
    [SerializeField] ENUM_TEAM_TYPE teamType;
    [SerializeField] Image charFrameImage;
    [SerializeField] Image hpFill;
    [SerializeField] Text characterText;
    [SerializeField] Text nicknameText;

    private float currHP;
    public float maxHP;

    Coroutine hpBarCoroutine;

    private void OnDisable()
    {
        if(hpBarCoroutine != null)
            StopCoroutine(hpBarCoroutine);
    }

    public Action<float> Connect_Character(ENUM_CHARACTER_TYPE _charType)
    {
        Managers.Data.CharInfoDict.TryGetValue((int)_charType, out CharacterInfo characterInfo);
        maxHP = characterInfo.maxHP;
        currHP = characterInfo.maxHP;

        Set_CharFrameImage(_charType);
        
        return Update_CurrHP;
    }

    public void Set_CharFrameImage(ENUM_CHARACTER_TYPE _charType)
    {
        hpFill.fillAmount = 1.0f;

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

        characterText.text = Managers.Data.Get_CharNameDict(_charType);

    }

    /// <summary>
    /// HP의 FillAmount 값을 반환함 ( 0 ~ 1 )
    /// </summary>
    public float Get_CurrHPFillAmount()
    {
        return hpFill.fillAmount;
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
        while (_goalHPValue < hpFill.fillAmount)
        {
            hpFill.fillAmount -= 0.01f;
            yield return null;
        }

        hpFill.fillAmount = _goalHPValue;
        hpBarCoroutine = null;
    }
}