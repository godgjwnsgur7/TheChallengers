using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

[System.Serializable]
public class StatusData
{
    public float currHP;
    public float maxHP;

    public StatusData() { }

    public StatusData(float _maxHP)
    {
        this.currHP = _maxHP;
        this.maxHP = _maxHP;
    }
 }

public class StatusWindowUI : MonoBehaviourPhoton
{
    [SerializeField] ENUM_TEAM_TYPE teamType;
    [SerializeField] Slider hpBarSlider;
    [SerializeField] Image charFrameImage;

    public float NetworkCurrHP
    {
        get
        {
            return NetworkDataHandler.Instance.Get_StatusCurrHP(teamType);
        }
        set
        {
            NetworkDataHandler.Instance.Set_StatusCurrHP(teamType, value);
            if(PhotonLogicHandler.IsConnected)
                PhotonLogicHandler.Instance.TryBroadcastMethod<StatusWindowUI>(this, Sync_CurrHP);
            else
                Sync_CurrHP();
        }
    }

    public float currHP;
    public float maxHP;

    Coroutine hpBarCoroutine;


    public void Set_StatusWindowUI(ENUM_CHARACTER_TYPE _charType, StatusData statusData)
    {
        Set_CharFrameImage(_charType);
        Set_MaxHP(statusData);
    }

    public void Set_CharFrameImage(ENUM_CHARACTER_TYPE _charType)
    {
        switch (_charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:
                // 이미지 아직 없음
                break;
            default:
                Debug.Log($"{_charType} 를 찾을 수 없음");
                break;
        }
    }

    public void Set_MaxHP(StatusData data)
    {
        NetworkCurrHP = data.currHP;
        currHP = data.currHP;
        maxHP = data.maxHP;
    }


    [BroadcastMethod]
    public void Sync_CurrHP()
    {
        currHP = NetworkCurrHP;

        if (hpBarCoroutine != null)
            StopCoroutine(hpBarCoroutine);

        if (currHP > 0)
        {
            hpBarCoroutine = StartCoroutine(IFadeHpBar(currHP / maxHP));
            return;
        }

        hpBarCoroutine = StartCoroutine(IFadeHpBar(0f));
    }

    protected IEnumerator IFadeHpBar(float _goalHPValue)
    {
        while (_goalHPValue < hpBarSlider.value)
        {
            hpBarSlider.value -= 0.01f;
            yield return null;
        }

        hpBarSlider.value = _goalHPValue;
        hpBarCoroutine = null;
    }
}