using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class Knight : ActiveCharacter
{
    [SerializeField] Skill[] skills = new Skill[3];
    
    public override void Init()
    {
        base.Init();

        characterType = ENUM_CHARACTER_TYPE.Knight;
    }
}
