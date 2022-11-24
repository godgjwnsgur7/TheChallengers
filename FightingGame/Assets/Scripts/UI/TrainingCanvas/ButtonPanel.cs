using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class ButtonPanel : UIElement
{
    public BaseMap map;

    [SerializeField] Text panelOpenBtnText;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] EnemyPlayer enemyPlayer;

    ENUM_CHARACTER_TYPE playerType;
    ENUM_CHARACTER_TYPE enemyType;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
        SetPanelOpenButtonText("닫기");
    }

    public override void Close()
    {
        base.Close();
        SetPanelOpenButtonText("설정");
    }

    public void Set_Map(BaseMap map) => this.map = map;

    public void Init()
    {
        playerCamera.Set_MapData(map);

        Change_PlayerType(ENUM_CHARACTER_TYPE.Default);
        Change_EnemyType(ENUM_CHARACTER_TYPE.Default);

        playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
        enemyPlayer.teamType = ENUM_TEAM_TYPE.Red;

    }

    public void OnClick_OpenSettingPanel()
    {
        if (playerCharacter.activeCharacter != null)
            DeletePlayer();

        if (enemyPlayer.activeCharacter != null)
            DeleteEnemy();

        InputKeyManagement inputKeyManagement = Managers.Input.Get_InputKeyManagement();
        inputKeyManagement.Init();
        this.Close();
    }

    public void OnClick_OnOffButtonPanel()
    {
        /*
        if (inputKeyManagement.settingPanel.isOpen)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("버튼설정 중에 누를 수 없습니다.");
            return;
        }
        */

        if (this.gameObject.activeSelf)
            Close();
        else
            Open();
    }

    public void SetPanelOpenButtonText(string text) => panelOpenBtnText.text = text;

    // 캐릭터 선택창 오픈
    public void OnClick_CallPlayer() => Managers.UI.popupCanvas.Open_CharSelectPopup(OnClick_SelectPlayerCharacter);
    public void OnClick_CallEnemy() => Managers.UI.popupCanvas.Open_CharSelectPopup(OnClick_SelectEnemyCharacter);

    public void OnClick_SelectPlayerCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        SelectPlayerCharacter(_charType);
        this.Close();
    }
    public void OnClick_SelectEnemyCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        SelectEnemyCharacter(_charType);
        this.Close();
    }

    // 캐릭터 선택
    public void SelectPlayerCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        if (playerType == _charType)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("같은 캐릭터가 이미 소환되어있습니다.");
            return;
        }

        Change_PlayerType(_charType);
        Debug.Log(playerType);
        Managers.UI.popupCanvas.Open_SelectPopup(CallPlayer, Reset_PlayerType, $"{playerType}를 소환하시겠습니까?");
    }

    public void SelectEnemyCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        Change_EnemyType(_charType);
        Debug.Log(enemyType);
        Managers.UI.popupCanvas.Open_SelectPopup(CallEnemy, Reset_EnemyType, $"{enemyType}를 소환하시겠습니까?");
    }

    public void OnClick_DestroyPlayer()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(DeletePlayer, null, "소환된 캐릭터를 역소환하시겠습니까?");
        this.Close();
    }
    public void OnClick_DestroyEnemy()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(DeleteEnemy, null, "소환된 적를 역소환하시겠습니까?");
        this.Close();
    }

    // 플레이어 소환
    public void CallPlayer()
    {
        /*
        if (inputKeyManagement.settingPanel.isOpen)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("버튼 설정중에는 소환이불가능합니다.");
            return;
        }
        */

        // 이미 소환된 플레이어 캐릭터가 있을 경우
        if (playerCharacter.activeCharacter != null)
        {
            if (playerCharacter.activeCharacter.name.Equals(Enum.GetName(typeof(ENUM_CHARACTER_TYPE), playerType)))
            {
                Managers.UI.popupCanvas.Open_TimeNotifyPopup("같은 캐릭터가 이미 소환되어있습니다.", 2.0f);
                Change_PlayerType(ENUM_CHARACTER_TYPE.Default);
                return;
            }

            Managers.Resource.Destroy(playerCharacter.activeCharacter.gameObject);
        }

        // 플레이어 스폰
            playerCharacter.Set_Character(Init_Character(map.blueTeamSpawnPoint.position, playerType));

        Change_PlayerType(ENUM_CHARACTER_TYPE.Default);
    }

    // 적 소환
    public void CallEnemy()
    {
        /*
        if (inputKeyManagement.settingPanel.isOpen)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("버튼 설정중에는 소환이불가능합니다.");
            return;
        }
        */

        // 이미 소환된 적이 있을 경우
        if (enemyPlayer.activeCharacter != null)
        {
            if (enemyPlayer.activeCharacter.name.Equals(Enum.GetName(typeof(ENUM_CHARACTER_TYPE), enemyType)))
            {
                Managers.UI.popupCanvas.Open_TimeNotifyPopup("같은 캐릭터가 이미 소환되어있습니다.", 2.0f);
                Change_EnemyType(ENUM_CHARACTER_TYPE.Default);
                return;
            }

            Managers.Resource.Destroy(enemyPlayer.activeCharacter.gameObject);
        }

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

        Change_EnemyType(ENUM_CHARACTER_TYPE.Default);
    }

    // 적 제거
    public void DeleteEnemy()
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
    public void DeletePlayer()
    {
        if (playerCharacter.activeCharacter == null)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("제거할 플레이어가 없습니다.");
            return;
        }

        playerCamera.Set_ZoomOut();

        Managers.Resource.Destroy(playerCharacter.activeCharacter.gameObject);
        Reset_PlayerType();
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
    private void Reset_EnemyType() => Change_PlayerType(ENUM_CHARACTER_TYPE.Default);

}
