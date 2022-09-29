using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "Resources/Prefabs/EffectObjects/" 경로 안의 프리팹 이름 리스트
/// </summary>
public enum ENUM_EFFECTOBJECT_NAME
{

}

public class EffectObject : Poolable
{
    // AttackObject를 참고하여 이펙트오브젝트를 만들 것.
    // 어택오브젝트와 이펙트오브젝트는 엄연히 다른 역할을 할 것.
    // 그래서 비슷한 구조를 띄나 결국 다르게 동작할 것임

    // 스크립트는 해당 스크립트로 하고, 만약 여러 스크립트가 필요 시에는
    // AttackObject를 분할해놓은 것처럼 할 것.
    // 분할 할 경우엔 ENUM_EFFECTOBJECT_TYPE으로 선언하고,
    // 모든 열거형은 해당 스크립트에 일단 모아놓을 것.!
    
    // 가져다 쓰는 건 준혁이 할 것.

    // Summon_AttackObject와 비슷하게 애니메이션 이벤트를 이용해서
    // Summon_EffectObject 같은 함수를 선언하여 사용할 것.
    // 관련해서 어택오브젝트나 액티브캐릭터 코드리딩 중에 질문이 있다면, 질문하면 됨
    // (우진)
}
