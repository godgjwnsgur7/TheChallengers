using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ObjectOrderLayerMgr
{
    Dictionary<ENUM_OBJECTLAYERTAG_NAME, int> currOrderLayerNumDict = new Dictionary<ENUM_OBJECTLAYERTAG_NAME, int>();

    int OrderLayerGroupMaxCount = 20; // 같은 타입의 레이어 갯수

    public void Init()
    {
        currOrderLayerNumDict.Clear();

        for(int i = 0; i < (int)ENUM_OBJECTLAYERTAG_NAME.Max; i++)
        {
            if((ENUM_OBJECTLAYERTAG_NAME)i == ENUM_OBJECTLAYERTAG_NAME.Character)
                continue;

            currOrderLayerNumDict.Add((ENUM_OBJECTLAYERTAG_NAME)i, i * OrderLayerGroupMaxCount);
        }
    }

    public int Get_CharacterOrderLayer() => (int)ENUM_OBJECTLAYERTAG_NAME.Character * OrderLayerGroupMaxCount;

    public int Get_SequenceOrderLayer(ENUM_OBJECTLAYERTAG_NAME _objectLayerTagName)
    {


        return 0;
    }


    public void Clear()
    {
        Init();
    }
}
