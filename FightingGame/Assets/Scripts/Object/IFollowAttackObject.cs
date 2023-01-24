using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFollowAttackObject
{
    /// <summary>
    /// 런타임 시간 체크
    /// </summary>
    IEnumerator IRunTimeCheck(float _runTime);

    /// <summary>
    /// 히트 시 이펙트 소환
    /// </summary>
    void Summon_EffectObject(int _effectTypeNum, Vector2 _targetTr);

    void Destroy_Mine();
    void Sync_DestroyMine();
}
