using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class SlaveProfileUI : BaseProfile
{
    [SerializeField] Image readyStateImage;

    // IsReadyInfoUpdateCallBack 함수에서만 접근해야 함
    private bool isReady = false;
    public bool IsReady
    {
        get { return isReady; }
        private set { IsReadyInfoUpdateCallBack(value); }
    }

    public override void Init()
    {
        base.Init();
    }

    public void IsReadyInfoUpdateCallBack(bool _readyState)
    {
        if (isReady == _readyState)
            return;

        isReady = _readyState;

        if (isReady)
        {
            readyStateImage.sprite = Managers.Resource.Load<Sprite>("Art/Sprites/ReadySprite");
            if (isMine) // 제어권을 가졌다면 서버의 정보를 변경함
                PhotonLogicHandler.Instance.OnReady();
        }
        else
        {
            readyStateImage.sprite = Managers.Resource.Load<Sprite>("Art/Sprites/UnreadySprite");
            if (isMine) // 제어권을 가졌다면 서버의 정보를 변경함
                PhotonLogicHandler.Instance.OnUnReady();
        }
    }

    public void Set_ReadyState(bool readyState)
    {
        if (readyState && currCharType == ENUM_CHARACTER_TYPE.Default)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("캐릭터를 선택하지 않았습니다.");
            return;
        }

        IsReady = readyState;
    }

    public override void Clear()
    {
        IsReady = false;
        base.Clear();
    }
}
