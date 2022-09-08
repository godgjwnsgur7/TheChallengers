using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class StatusWindowUI : UIElement
{
    public Slider hpBarSlider;
    [SerializeField] Image charFrameImage;

    public float maxHP;
    public float curHP;

    Coroutine hpBarCoroutine;

    public void Set_StatusWindowUI(ENUM_CHARACTER_TYPE _charType, float _maxHP)
    {
        if(PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod<StatusWindowUI, ENUM_CHARACTER_TYPE>(this, Set_CharFrameImage, _charType);
            PhotonLogicHandler.Instance.TryBroadcastMethod<StatusWindowUI, float>(this, Set_MaxHP, _maxHP);
        }
        else
        {
            Set_CharFrameImage(_charType);
            Set_MaxHP(maxHP);
        }
    }

    /*
    protected override void OnMineSerializeView(PhotonWriteStream stream)
    {
        stream.Write(hpBarSlider.value);
        stream.Write(maxHP);
        stream.Write(curHP);

        base.OnMineSerializeView(stream);
    }

    protected override void OnOtherSerializeView(PhotonReadStream stream)
    {
        hpBarSlider.value = stream.Read<float>();
        maxHP = stream.Read<float>();
        curHP = stream.Read<float>();

        base.OnOtherSerializeView(stream);
    }
    */

    [BroadcastMethod]
    public void Set_CharFrameImage(ENUM_CHARACTER_TYPE _charType)
    {
        switch(_charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:
                Debug.Log("이미지 아직 없음 ㅋㅋ");
                break;
            default:
                Debug.Log($"{_charType} 를 찾을 수 없음");
                break;
        }
    }

    [BroadcastMethod]
    public void Set_MaxHP(float _maxHP)
    {
        maxHP = _maxHP;
        curHP = _maxHP;
    }

    /// <summary>
    /// false 리턴 시 HP가 전부 닳은 것
    /// </summary>

    [BroadcastMethod]
    public bool Input_Damage(float _damege)
    {
        curHP -= _damege;

        if(hpBarCoroutine != null)
            StopCoroutine(hpBarCoroutine);

        if(curHP > 0)
        {
            hpBarCoroutine = StartCoroutine(IFadeHpBar(curHP / maxHP));
            return true;
        }

        hpBarCoroutine = StartCoroutine(IFadeHpBar(0f));
        return false;
    }

    protected IEnumerator IFadeHpBar(float _goalHPValue)
    {
        float _curHPValue = curHP / maxHP;
            
        while(_goalHPValue < hpBarSlider.value)
        {
            hpBarSlider.value -= 0.01f;
            yield return null;
        }

        hpBarSlider.value = _goalHPValue;
        hpBarCoroutine = null;
    }
}

