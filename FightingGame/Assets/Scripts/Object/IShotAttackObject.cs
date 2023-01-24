using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShotAttackObject : IFollowAttackObject
{
    /// <summary>
    /// 오브젝트 발사 함수
    /// </summary>
    void Shot_AttackObject();
}
