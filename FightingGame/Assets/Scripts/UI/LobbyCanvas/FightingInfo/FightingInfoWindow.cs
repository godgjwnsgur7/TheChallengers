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
        Managers.UI.popupCanvas.Play_FadeOutEffect(Wait_PlayFadeInEffect);
    }

    public void Close()
    {
        this.UnregisterRoomCallback();
    }

    public void Wait_PlayFadeInEffect()
    {
        this.gameObject.SetActive(true);

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
        // 아직 미구현 - Map은 일단 배치되어있음
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
            StartCoroutine(IWaitGameStart(1.0f));
        }
    }

    protected IEnumerator IWaitGameStart(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Managers.Scene.Sync_LoadScene(ENUM_SCENE_TYPE.Battle);
    }
}
