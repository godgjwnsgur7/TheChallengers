using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class PushOutAttackObject : HitAttackObject
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServerSyncState && PhotonLogicHandler.IsMine(viewID)) // 맞는 애가 처리하기 위해
            return;

        ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

        if (enemyCharacter != null && skillValue != null)
        {
            if (enemyCharacter.teamType == teamType || enemyCharacter.invincibility)
                return;

            bool _reverseState = collision.transform.position.x < transform.position.x;
            
            enemyCharacter.Hit(new CharacterAttackParam((ENUM_ATTACKOBJECT_NAME)skillValue.skillType, _reverseState));

            // 이펙트 생성
            int effectNum = UnityEngine.Random.Range(0, 3);
            // 임시로 갈기는중
            Summon_EffectObject(effectNum, collision.transform.position);

            Sync_DestroyMine();
        }
        else
        {
            // Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }
}
