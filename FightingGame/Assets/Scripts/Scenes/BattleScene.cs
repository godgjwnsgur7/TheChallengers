using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    BaseMap map;

    [SerializeField] PlayerCharacter playerCharacter;

    [SerializeField] EnemyPlayer enemyPlayer; // 디버그용

    ENUM_TEAM_TYPE teamType;

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Battle;

        // 일단 무조건 베이직맵 가져와 (임시)
        map = Managers.Resource.Instantiate("Maps/BasicMap").GetComponent<BaseMap>();

        // 디버그용으로 일단 다 박아.
        playerCharacter.Set_Character(Init_Character(ENUM_TEAM_TYPE.Blue, map.blueTeamSpawnPoint.position));
        enemyPlayer.Set_Character(Init_Character(ENUM_TEAM_TYPE.Red, map.redTeamSpawnPoint.position));

    }

    public override void Clear()
    {

    }

    public ActiveCharacter Init_Character(ENUM_TEAM_TYPE _teamType, Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position).GetComponent<ActiveCharacter>();
        activeCharacter.Init();
        activeCharacter.teamType = _teamType;
        
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
                break;

            default:
                Debug.Log($"Failed to SkillObject : {charType}");
                break;
        }
    }
}