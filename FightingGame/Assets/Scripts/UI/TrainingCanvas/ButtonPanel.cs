using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class ButtonPanel : UIElement
{
    BaseMap map;

    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] EnemyPlayer enemyPlayer;
    [SerializeField] Button[] buttons;

    ENUM_CHARACTER_TYPE playerType;
    ENUM_CHARACTER_TYPE enemyType;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void Init()
    {
        Summon_Map(ENUM_MAP_TYPE.ForestMap);

        Reset_PlayerType();
        Reset_EnemyType();

        playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
        enemyPlayer.teamType = ENUM_TEAM_TYPE.Red;

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void Summon_Map(ENUM_MAP_TYPE mapType)
    { 
        if(map != null)
        {
            Managers.UI.popupCanvas.Play_FadeInEffect();
            Managers.Resource.Destroy(map.gameObject);
            map = null;
        }

        DestroyCharacter();

        string mapName = Enum.GetName(typeof(ENUM_MAP_TYPE), mapType);
        map = Managers.Resource.Instantiate($"Maps/{mapName}").GetComponent<BaseMap>();
        playerCamera.Set_MapData(map);

        Managers.UI.popupCanvas.Play_FadeOutEffect();

        Close();
    }

    public void OnClick_OpenSettingPanel()
    {
        Managers.UI.popupCanvas.Play_FadeInEffect();

        DestroyCharacter();
        Managers.Input.Get_InputKeyManagement().Init();
        this.Close();

        Managers.UI.popupCanvas.Play_FadeOutEffect();
    }

    public void OnClick_OnOffButtonPanel()
    {
        if (this.gameObject.activeSelf)
            Close();
        else
            Open();
    }

    // 캐릭터 선택창 오픈
    public void OnClick_CallPlayer() => Managers.UI.popupCanvas.Open_CharSelectPopup(Set_PlayerCharCallBack);
    public void OnClick_CallEnemy() => Managers.UI.popupCanvas.Open_CharSelectPopup(Set_EnemyCharCallBack);

    public void Set_PlayerCharCallBack(ENUM_CHARACTER_TYPE _charType)
    {
        SelectPlayerCharacter(_charType);
        this.Close();
    }
    public void Set_EnemyCharCallBack(ENUM_CHARACTER_TYPE _charType)
    { 
        SelectEnemyCharacter(_charType);
        this.Close();
    }

    // 캐릭터 선택
    public void SelectPlayerCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        Change_PlayerType(_charType);

        if (playerCharacter.activeCharacter != null)
            Managers.UI.popupCanvas.Open_SelectPopup(CallPlayer, SelectCancel_Player, $"{playerType}를 재소환하시겠습니까?");
        else
            CallPlayer();
    }

    public void SelectEnemyCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        Change_EnemyType(_charType);

        if (enemyPlayer.activeCharacter != null)
            Managers.UI.popupCanvas.Open_SelectPopup(CallEnemy, SelectCancel_Enemy, $"{enemyType}를 재소환하시겠습니까?");
        else
            CallEnemy();
    }

    // 임시 주석화
    /*public void OnClick_DestroyPlayer()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(DestroyPlayer, null, "소환된 캐릭터를 역소환하시겠습니까?");
        this.Close();
    }
    public void OnClick_DestroyEnemy()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(DestroyEnemy, null, "소환된 적를 역소환하시겠습니까?");
        this.Close();
    }*/

    public void OnClick_DestroyCharacter()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(DestroyCharacter, null, "소환된 캐릭터를 역소환하시겠습니까?");
        this.Close();
    }

    // 플레이어 소환
    public void CallPlayer()
    {
        ENUM_CHARACTER_TYPE charType = playerType;

        // 이미 소환된 플레이어 캐릭터가 있을 경우
        if (playerCharacter.activeCharacter != null)
            Managers.Resource.Destroy(playerCharacter.activeCharacter.gameObject);

        // 플레이어 스폰
        playerCharacter.Set_Character(Init_Character(map.blueTeamSpawnPoint.position, charType));

        GameObject go = playerCharacter.activeCharacter.transform.Find("Sound").gameObject;
        Managers.Sound.Set_TeamAudioSource(go.GetComponent<AudioSource>(), ENUM_SOUND_TYPE.SFX_BLUE);

        Reset_PlayerType();
    }

    // 적 소환
    public void CallEnemy()
    {
        // 이미 소환된 적이 있을 경우
        if (enemyPlayer.activeCharacter != null)
            Managers.Resource.Destroy(enemyPlayer.activeCharacter.gameObject);

        // 소환된 플레이어가 없을 경우 지정된 스폰에서 생성
        if (playerCharacter.activeCharacter == null)
        {
            enemyPlayer.Set_Character(Init_Enemy(map.redTeamSpawnPoint.position, enemyType));
        }
        else
        {
            float playerFrontPos = playerCharacter.activeCharacter.reverseState ? -2f : 2f;

            // 플레이어 앞 위치
            Vector2 respownPos = new Vector2(playerCharacter.activeCharacter.transform.position.x + playerFrontPos,
                playerCharacter.activeCharacter.transform.position.y + 1);

            // 맵의 크기 밖에 소환되지 않게 체크
            if (respownPos.x <= map.minBound.x)
                respownPos.x = map.minBound.x + 1;
            if (respownPos.x >= map.maxBound.x)
                respownPos.x = map.maxBound.x - 1;
            if (respownPos.y >= map.maxBound.y)
                respownPos.y = map.maxBound.y + 1;
            if (respownPos.y <= map.minBound.y)
                respownPos.y = map.minBound.y - 1;

            enemyPlayer.Set_Character(Init_Enemy(respownPos, enemyType));
        }

        GameObject go = enemyPlayer.activeCharacter.transform.Find("Sound").gameObject;
        Managers.Sound.Set_TeamAudioSource(go.GetComponent<AudioSource>(), ENUM_SOUND_TYPE.SFX_RED);

        Reset_EnemyType();
    }

    private void SelectCancel_Player()
    {
        Reset_PlayerType();
        return;
    }

    private void SelectCancel_Enemy()
    {
        Reset_EnemyType();
        return;
    }

    // 적 제거
    private void DestroyEnemy()
    {
        if (enemyPlayer.activeCharacter == null)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("제거할 적이 없습니다.");
            return;
        }

        Managers.Resource.Destroy(enemyPlayer.activeCharacter.gameObject);
        Reset_EnemyType();
    }

    // 플레이어 제거
    private void DestroyPlayer()
    {
        if (playerCharacter.activeCharacter == null)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("제거할 플레이어가 없습니다.");
            return;
        }

        Managers.Resource.Destroy(playerCharacter.activeCharacter.gameObject);
        Managers.Input.Destroy_InputKeyController();
        Reset_PlayerType();

        playerCamera.Set_ZoomOut();
    }

    public void DestroyCharacter()
    {
        if (playerCharacter.activeCharacter != null)
        {
            DestroyPlayer();
            playerCamera.Set_ZoomOut();
        }

        if (enemyPlayer.activeCharacter != null)
            DestroyEnemy();
    }

    public ActiveCharacter Init_Character(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, playerCharacter.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();

        return activeCharacter;
    }

    public ActiveCharacter Init_Enemy(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, enemyPlayer.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();

        return activeCharacter;
    }

    // 캐릭터 타입 변경
    public void Change_PlayerType(ENUM_CHARACTER_TYPE _value) => playerType = _value;
    public void Change_EnemyType(ENUM_CHARACTER_TYPE _value) => enemyType = _value;
    private void Reset_PlayerType() => Change_PlayerType(ENUM_CHARACTER_TYPE.Default);
    private void Reset_EnemyType() => Change_EnemyType(ENUM_CHARACTER_TYPE.Default);
}
