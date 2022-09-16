using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class StatusWindowUI : MonoBehaviourPhoton
{
    [SerializeField] ENUM_TEAM_TYPE teamType;
    [SerializeField] Slider hpBarSlider;
    [SerializeField] Image charFrameImage;

    private float currHP;
    public float CurrHP
    {
        get { return currHP; }
        set
        {
            currHP = value;
            if (PhotonLogicHandler.IsConnected)
                PhotonLogicHandler.Instance.TryBroadcastMethod<StatusWindowUI>(this, Sync_CurrHP);
            else
                Sync_CurrHP();
        }
    }
    public float maxHP;

    Coroutine hpBarCoroutine;


    public void Set_StatusWindowUI(ENUM_CHARACTER_TYPE _charType, float _maxHP)
    {
        if(PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod<StatusWindowUI, ENUM_CHARACTER_TYPE>
                (this, Set_CharFrameImage, _charType);
            PhotonLogicHandler.Instance.TryBroadcastMethod<StatusWindowUI, float>
                (this, Set_MaxHP, _maxHP);
        }
        else
        {
            Set_CharFrameImage(_charType);
            Set_MaxHP(_maxHP);
        }
    }

    [BroadcastMethod]
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

    [BroadcastMethod]
    public void Set_MaxHP(float _maxHP)
    {
        CurrHP = _maxHP;
        maxHP = _maxHP;
    }


    [BroadcastMethod]
    public void Sync_CurrHP()
    {
        if (hpBarCoroutine != null)
            StopCoroutine(hpBarCoroutine);

        if (CurrHP > 0)
        {
            hpBarCoroutine = StartCoroutine(IFadeHpBar(CurrHP / maxHP));
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

    protected override void OnMineSerializeView(PhotonWriteStream writeStream)
    {
        writeStream.Write(CurrHP);

        base.OnMineSerializeView(writeStream);
    }

    protected override void OnOtherSerializeView(PhotonReadStream readStream)
    {
        CurrHP = readStream.Read<float>();

        base.OnOtherSerializeView(readStream);
    }
}