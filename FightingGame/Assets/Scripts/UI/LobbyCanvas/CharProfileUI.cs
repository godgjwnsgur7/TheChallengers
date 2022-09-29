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
    public bool isReady = false;
    public int userId = 0;

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
        // 현재 

    }

    public void Select_Char(ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Default)
    {
        if (currCharType == _charType) return;

        currCharType = _charType;
        switch (currCharType)
        {
            case ENUM_CHARACTER_TYPE.Default:

                charNameText.text = "";
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

    public void OnClick_Ready()
    {
        if (PhotonLogicHandler.IsMasterClient)
            return;

        PhotonLogicHandler.Instance.TryBroadcastMethod<CharProfileUI, bool>
            (this, Sync_ReadyStateImage, isReady);

        isReady = !isReady;
    }

    [BroadcastMethod]
    private void Sync_ReadyStateImage(bool _isReady)
    {
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
