using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    BaseMap map;

    [SerializeField] BattleCanvas battleCanvas;

    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] PlayerCamera playerCamera;

    [SerializeField] EnemyPlayer enemyPlayer; // 디버그용

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Battle;

        // 일단 무조건 베이직맵 가져와 (임시)
        map = Managers.Resource.Instantiate("Maps/BasicMap").GetComponent<BaseMap>();
        
        // 카메라 가두기
        playerCamera.Set_CameraBounds(map.maxBound, map.minBound);

        if(PhotonLogicHandler.IsConnected)
        {
            if (PhotonLogicHandler.IsMasterClient)
            {
                playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
                SetCharacterWithPos(map.blueTeamSpawnPoint.position);
            }
            else
            {
                playerCharacter.teamType = ENUM_TEAM_TYPE.Red;
                SetCharacterWithPos(map.redTeamSpawnPoint.position);
            }
        }
        else // 클라 하나
        {
            playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
            playerCharacter.Set_Character(Init_Character(map.blueTeamSpawnPoint.position));
            playerCharacter.Connect_Status(battleCanvas.Get_StatusWindowUI(playerCharacter.teamType));
            
            enemyPlayer.gameObject.SetActive(true);
            enemyPlayer.teamType = ENUM_TEAM_TYPE.Red;
            enemyPlayer.Set_Character(Init_Character(map.redTeamSpawnPoint.position));
            enemyPlayer.Connect_Status(battleCanvas.Get_StatusWindowUI(enemyPlayer.teamType));
        }
    }

    private void SetCharacterWithPos(Vector3 spawnPos)
    {
        playerCharacter.Set_Character(Init_Character(spawnPos));
    }

    public override void Clear()
    {

    }

    public ActiveCharacter Init_Character(Vector3 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter;

        if (!PhotonLogicHandler.IsConnected)
        {
            activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position).GetComponent<ActiveCharacter>();
        }
        else
        {
            activeCharacter = Managers.Resource.InstantiateEveryone($"{_charType}", _position).GetComponent<ActiveCharacter>();
        }

        Skills_Pooling(_charType);

        return activeCharacter;
    }

    private void Skills_Pooling(ENUM_CHARACTER_TYPE charType)
    {
        switch (charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_JumpAttack", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack1", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack2", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack3", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_ThrowSkill", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_SuperArmourSkill", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_SuperArmourSkill_1", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_SuperArmourSkill_2", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_SuperArmourSkill_3", 3);

                break;

            default:
                Debug.Log($"Failed to SkillObject : {charType}");
                break;
        }
    }
}