using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFallAttackObject : IMultiAttackObject
{
    IEnumerator Folling_AttackObejct();
    void Check_GroundHit();

    [BroadcastMethod]
    void Shot_AttackObject();

    [BroadcastMethod]
    void Reverse_Bool(string _parametorName);
}
