using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class FightingInfoWindow : MonoBehaviour, IRoomPostProcess
{
    [SerializeField] FightingUserInfoUI masterFightingInfo;
    [SerializeField] FightingUserInfoUI slaveFightingInfo;

    [SerializeField] Image mapImage;
    [SerializeField] Text mapNameText;

    Coroutine infoSettingCheckCoroutine = null;
    Coroutine waitFadeInEffectCoroutine = null;

    private void OnDisable()
    {
        if (infoSettingCheckCoroutine != null)
            StopCoroutine(infoSettingCheckCoroutine);

        if(waitFadeInEffectCoroutine != null)
            StopCoroutine(waitFadeInEffectCoroutine);
    }

    public void Open()
    {
        Managers.Clear();

        this.RegisterRoomCallback();

        this.gameObject.SetActive(true);
        infoSettingCheckCoroutine = StartCoroutine(IInfoSettingCheck());

        PhotonLogicHandler.Instance.RequestRoomCustomProperty();
        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();
    }

    public void Close()
    {
        this.UnregisterRoomCallback();
    }

    public void Wait_PlayFadeInEffect()
    {
        waitFadeInEffectCoroutine = StartCoroutine(IWaitFadeInEffect(3.0f));
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {
        Set_MapInfo(property.currentMapType);
    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        if (property.isMasterClient)
            masterFightingInfo.Set_UserInfo(property.data);
        else
            slaveFightingInfo.Set_UserInfo(property.data);
    }

    private void Set_MapInfo(ENUM_MAP_TYPE _mapType)
    {
        if (this == null || !this.gameObject.activeSelf)
            return;

        mapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
        mapNameText.text = Managers.Data.Get_MapNameDict(_mapType);
    }

    protected IEnumerator IInfoSettingCheck()
    {
        // 상대방 정보까지 Init이 됐다면, 보여주기
        while((mapImage.sprite == null) || masterFightingInfo.isInit || slaveFightingInfo.isInit)
        {
            yield return null;
        }

        Managers.UI.popupCanvas.Play_FadeInEffect(Wait_PlayFadeInEffect);
        infoSettingCheckCoroutine = null;
    }

    protected IEnumerator IWaitFadeInEffect(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Managers.UI.popupCanvas.Play_FadeOutEffect(GoTo_BattleScene);
        waitFadeInEffectCoroutine = null;
    }

    public void GoTo_BattleScene()
    {
        if(PhotonLogicHandler.IsMasterClient)
        {
            StartCoroutine(IWaitGameStart(1.0f));
        }
    }

    protected IEnumerator IWaitGameStart(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Managers.Scene.Sync_LoadScene(ENUM_SCENE_TYPE.Battle);
    }
}
