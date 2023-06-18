using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class PushOutAttackObject : HitAttackObject
{
    // 수정 시 참조 확인 (재정의해서 사용하고 있음)
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

            Vector3 hitPosVec = collision.transform.position;
            hitPosVec.z = 0;

            Managers.Network.SyncPlaySFX_HitSound(skillValue.hitSoundType, teamType, hitPosVec);

            // 피격된 캐릭터 위치를 기준으로 주어진 범위 내의 랜덤위치로 조정
            Vector2 randomHitPosVec = collision.transform.position;
            randomHitPosVec.x += Random.Range(-0.5f, 0.5f);
            randomHitPosVec.y += Random.Range(-0.3f, 1.0f);

            // 이펙트 생성 ( 임시 랜덤 )
            Summon_EffectObject(Random.Range(0, 3), teamType, randomHitPosVec);
            Summon_EffectObject(Random.Range(3, 5), teamType, collision.transform.position);

            Sync_DestroyMine();
        }
        else
        {
            // Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }
}
