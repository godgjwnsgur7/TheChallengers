using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ShotAttackObejct : AttackObejct
{
    public Rigidbody2D rigid2D;
    
    public override void Init()
    {
        base.Init();
    
        rigid2D = GetComponent<Rigidbody2D>();
    }

    public override void ActivatingAttackObject(Transform _playerTr, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        base.ActivatingAttackObject(_playerTr, _teamType, _reverseState);

        // 날아가는 힘을 받아야 하는데, 고민중
        // 1. 그냥 데이터에 박고 필요없는 애들은 0으로 만듬 - 필요 없는 애들도 0으로 가지게 됨
        // 2. 따로 데이터를 받음 - 효율이 낮은 데이터 구조? 냄새가 남
        // 3. 프리팹 오브젝트에 SerializeField로 박음 (지양사항) - 수정하면 재업로드 ㅋㅋ
        // 4. 등등
        float speed = 500.0f;

        if (_reverseState) speed *= -1f;

        rigid2D.AddForce(new Vector2(speed, 0));
    }
}
