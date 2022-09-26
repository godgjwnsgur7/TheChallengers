using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;


// 얘는 수정할 예정
[Serializable]
public enum ENUM_BTNPREFS_TYPE
{
    LeftMoveBtn = 0,
    RightMoveBtn = 1,
    AttackBtn = 2,
    JumpBtn = 3,
    SkillBtn1 = 4,
    SkillBtn2 = 5,
    SkillBtn3 = 6,

    Max
}

public class PlayerPrefsData { }

public class SkillUIData : PlayerPrefsData
{
    public float size;
    public float opacity;
    public float transX;
    public float transY;

    public SkillUIData(float _size, float _opacity, Vector2 _transformPosition)
    {
        size = _size;
        opacity = _opacity;
        transX = _transformPosition.x;
        transY = _transformPosition.y;
    }
}

 public class PlayerPrefsManagement
{

}
