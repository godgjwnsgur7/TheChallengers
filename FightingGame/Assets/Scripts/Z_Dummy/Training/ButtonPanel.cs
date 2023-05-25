using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

/*
public class ButtonPanel : UIElement
{
    BaseMap map;

    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] TrainingCharacter trainingPlayer;
    [SerializeField] TrainingCharacter trainingEnemy;

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
        Summon_Map(ENUM_MAP_TYPE.CaveMap);

        Reset_PlayerType();
        Reset_EnemyType();

        trainingPlayer.teamType = ENUM_TEAM_TYPE.Blue;
        trainingEnemy.teamType = ENUM_TEAM_TYPE.Red;
        trainingPlayer.inabilityState = false;
        trainingEnemy.inabilityState = true;

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void Summon_Map(ENUM_MAP_TYPE mapType)
    { 
        if(map != null)
        {
            Managers.Resource.Destroy(map.gameObject);
            map = null;
        }

        DestroyCharacter();

        string mapName = Enum.GetName(typeof(ENUM_MAP_TYPE), mapType);
        map = Managers.Resource.Instantiate($"Maps/{mapName}").GetComponent<BaseMap>();
        playerCamera.Init(map);

        Close();
    }

    public void OnClick_OpenSettingPanel()
    {
        DestroyCharacter();
        Managers.Input.Get_InputKeyManagement().Init();
        this.Close();
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

        if (trainingPlayer.activeCharacter != null)
            Managers.UI.popupCanvas.Open_SelectPopup(CallPlayer, SelectCancel_Player, $"{playerType}를 재소환하시겠습니까?");
        else
            CallPlayer();
    }

    public void SelectEnemyCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        Change_EnemyType(_charType);

        if (trainingEnemy.activeCharacter != null)
            Managers.UI.popupCanvas.Open_SelectPopup(CallEnemy, SelectCancel_Enemy, $"{enemyType}를 재소환하시겠습니까?");
        else
            CallEnemy();
    }

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
        if (trainingPlayer.activeCharacter != null)
            Managers.Resource.Destroy(trainingPlayer.activeCharacter.gameObject);

        // 플레이어 스폰
        trainingPlayer.Set_Character(Init_Character(map.blueTeamSpawnPoint.position, charType));

        Reset_PlayerType();
    }

    // 적 소환
    public void CallEnemy()
    {
        // 이미 소환된 적이 있을 경우
        if (trainingEnemy.activeCharacter != null)
            Managers.Resource.Destroy(trainingEnemy.activeCharacter.gameObject);

        // 소환된 플레이어가 없을 경우 지정된 스폰에서 생성
        if (trainingPlayer.activeCharacter == null)
        {
            trainingEnemy.Set_Character(Init_Enemy(map.redTeamSpawnPoint.position, enemyType));
        }
        else
        {
            float playerFrontPos = trainingPlayer.activeCharacter.reverseState ? -2f : 2f;

            // 플레이어 앞 위치
            Vector2 respownPos = new Vector2(trainingPlayer.activeCharacter.transform.position.x + playerFrontPos,
                trainingPlayer.activeCharacter.transform.position.y + 1);

            // 맵의 크기 밖에 소환되지 않게 체크
            if (respownPos.x <= map.minBound.x)
                respownPos.x = map.minBound.x + 1;
            if (respownPos.x >= map.maxBound.x)
                respownPos.x = map.maxBound.x - 1;
            if (respownPos.y >= map.maxBound.y)
                respownPos.y = map.maxBound.y + 1;
            if (respownPos.y <= map.minBound.y)
                respownPos.y = map.minBound.y - 1;

            trainingEnemy.Set_Character(Init_Enemy(respownPos, enemyType));
        }

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
        if (trainingEnemy.activeCharacter == null)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("제거할 적이 없습니다.");
            return;
        }

        Managers.Resource.Destroy(trainingEnemy.activeCharacter.gameObject);
        Reset_EnemyType();
    }

    // 플레이어 제거
    private void DestroyPlayer()
    {
        if (trainingPlayer.activeCharacter == null)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("제거할 플레이어가 없습니다.");
            return;
        }

        Managers.Resource.Destroy(trainingPlayer.activeCharacter.gameObject);
        Managers.Input.Destroy_InputKeyController();
        Reset_PlayerType();

        playerCamera.Set_ZoomOut();
    }

    public void DestroyCharacter()
    {
        if (trainingPlayer.activeCharacter != null)
        {
            DestroyPlayer();
            playerCamera.Set_ZoomOut();
        }

        if (trainingEnemy.activeCharacter != null)
            DestroyEnemy();
    }

    public ActiveCharacter Init_Character(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, trainingPlayer.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();
        activeCharacter.Skills_Pooling();

        return activeCharacter;
    }

    public ActiveCharacter Init_Enemy(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, trainingEnemy.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();
        activeCharacter.Skills_Pooling();

        return activeCharacter;
    }

    // 캐릭터 타입 변경
    public void Change_PlayerType(ENUM_CHARACTER_TYPE _value) => playerType = _value;
    public void Change_EnemyType(ENUM_CHARACTER_TYPE _value) => enemyType = _value;
    private void Reset_PlayerType() => Change_PlayerType(ENUM_CHARACTER_TYPE.Default);
    private void Reset_EnemyType() => Change_EnemyType(ENUM_CHARACTER_TYPE.Default);
}
*/
