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
    [SerializeField] Image charFrameCoverImage;
    [SerializeField] Slider hpSilder;
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

        Set_Nickname();
        Set_CharFrameImage(_charType);
        
        return Update_CurrHP;
    }

    private void Set_CharFrameImage(ENUM_CHARACTER_TYPE _charType)
    {
        hpSilder.value = 1.0f;

        charFrameImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Characters/CharFrame_{_charType}");
        charFrameImage.gameObject.SetActive(true);

        characterText.text = Managers.Data.Get_CharNameDict(_charType);
    }

    private void Set_Nickname()
    {
        if (PhotonLogicHandler.IsMasterClient ==
            (teamType == ENUM_TEAM_TYPE.Blue))
        {
            Color selectColor = Managers.Data.Get_SelectColor();
            charFrameCoverImage.color = selectColor;
            nicknameText.color = selectColor;
        }
    }

    /// <summary>
    /// HPSilder의 Valuie 값을 반환함 ( 0 ~ 1 )
    /// </summary>
    public float Get_CurrHPFillAmount()
    {
        return hpSilder.value;
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
        while (_goalHPValue < hpSilder.value)
        {
            hpSilder.value -= 0.01f;
            yield return null;
        }

        hpSilder.value = _goalHPValue;
        hpBarCoroutine = null;
    }
}