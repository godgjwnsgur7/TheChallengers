using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CharProfileUI : MonoBehaviourPhoton
{
    public bool isInit = false;

    [Header ("Set In Editor")]
    [SerializeField] Image charImage;
    [SerializeField] Image readyStateImage;

    [SerializeField] Text charNameText;
    [SerializeField] Text userNicknameText;

    [Header("Setting Resources With Editor")]
    [SerializeField] Sprite readySprite;
    [SerializeField] Sprite unreadySprite;

    // 확인용 public (임시)
    public ENUM_CHARACTER_TYPE currCharType = ENUM_CHARACTER_TYPE.Default;
    public int userId = 0;
    public bool isReady = false;
    public bool isMine = false;

    public override void Init()
    {
        if (isInit) return;

        isInit = true;

        base.Init();

        PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI>
            (this, Sync_UpdateProfileInfo);
    }

    [BroadcastMethod]
    public void Sync_UpdateProfileInfo()
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI, ENUM_CHARACTER_TYPE>
            (this, Sync_SelectChar, currCharType);

        PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI, bool>
            (this, Sync_ReadyStateImage, isReady);

        // 내 아이디도 다시 세팅해야 함
    }

    public void Select_Char(ENUM_CHARACTER_TYPE _charType)
    {
        if (!isMine || currCharType == _charType) return;

        PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI, ENUM_CHARACTER_TYPE>
            (this, Sync_SelectChar, _charType);
    }

    [BroadcastMethod]
    private void Sync_SelectChar(ENUM_CHARACTER_TYPE _charType)
    {
        currCharType = _charType;

        switch (currCharType)
        {
            case ENUM_CHARACTER_TYPE.Default:

                charNameText.text = "캐릭터 미선택";
                break;
            case ENUM_CHARACTER_TYPE.Knight:

                charNameText.text = "나이트";
                break;
            case ENUM_CHARACTER_TYPE.Wizard:

                charNameText.text = "위저드";
                break;
            default:
                charNameText.text = "없는 캐릭터";
                break;
        }
    }

    public void OnClick_SeleteChar()
    {
        if (!isMine) return;

        Managers.UI.popupCanvas.Open_CharSelectPopup(Select_Char);
    }

    public void OnClick_Ready()
    {
        if (PhotonLogicHandler.IsMasterClient)
            return;

        PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI, bool>
            (this, Sync_ReadyStateImage, isReady);

        isReady = !isReady;
    }

    [BroadcastMethod]
    public void Sync_ReadyStateImage(bool _isReady)
    {
        if (isReady != _isReady)
            isReady = _isReady;

        if (_isReady)
            readyStateImage.sprite = unreadySprite;
        else
            readyStateImage.sprite = readySprite;
    }

    [BroadcastMethod]
    public void Clear()
    {
        Select_Char(ENUM_CHARACTER_TYPE.Default);

        isInit = false;
        userId = 0;
    }
}
