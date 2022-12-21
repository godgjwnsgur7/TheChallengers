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

    public void Open()
    {
        this.RegisterRoomCallback();

        PhotonLogicHandler.Instance.RequestRoomCustomProperty();
        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();

        Managers.Battle.Start_ServerSync();
        this.gameObject.SetActive(true);
        StartCoroutine(IInfoSettingCheck());
    }

    public void Close()
    {
        this.UnregisterRoomCallback();
    }

    public void Wait_PlayFadeInEffect()
    {
        StartCoroutine(IWaitFadeInEffect(3.0f));
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

        if(property.isMasterClient == PhotonLogicHandler.IsMasterClient)
            Managers.Battle.Set_MyDBData(property.data);
        else
            Managers.Battle.Set_EnemyScore(property.data.ratingPoint);
    }

    private void Set_MapInfo(ENUM_MAP_TYPE _mapType)
    {
        mapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
    }

    protected IEnumerator IInfoSettingCheck()
    {
        while((mapImage.sprite == null) || masterFightingInfo.isInit || slaveFightingInfo.isInit)
        {
            yield return null;
        }

        Managers.UI.popupCanvas.Play_FadeOutEffect(Wait_PlayFadeInEffect);
    }

    protected IEnumerator IWaitFadeInEffect(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Managers.UI.popupCanvas.Play_FadeInEffect(GameStart);
    }

    public void GameStart()
    {
        if(PhotonLogicHandler.IsMasterClient)
        {
            // 실행 전에 둘 다 들어와있는지 확인해야 하려나?
            StartCoroutine(IWaitGameStart(1.0f));
        }
    }

    protected IEnumerator IWaitGameStart(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Managers.Scene.Sync_LoadScene(ENUM_SCENE_TYPE.Battle);
    }
}
