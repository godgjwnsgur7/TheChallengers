using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TestCanvas : BaseCanvas
{
    [SerializeField] ENUM_BGM_TYPE bgmType;
    public void OnClick_BGM()
    {
        Managers.Sound.Play_BGM(bgmType);
    }
}
