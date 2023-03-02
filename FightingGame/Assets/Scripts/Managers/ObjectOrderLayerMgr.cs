using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ObjectOrderLayerMgr
{
    Dictionary<ENUM_OBJECTLAYERLEVEL_TYPE, int> currOrderLayerNumDict = new Dictionary<ENUM_OBJECTLAYERLEVEL_TYPE, int>();
    int OrderLayerGroupMaxCount = 20;

    public void Init()
    {
        currOrderLayerNumDict.Clear();

        for(int i = 0; i < (int)ENUM_OBJECTLAYERLEVEL_TYPE.Max; i++)
        {
            currOrderLayerNumDict.Add((ENUM_OBJECTLAYERLEVEL_TYPE)i, i * OrderLayerGroupMaxCount);
        }
    }

    public int Get_CharacterOrderLayer() => (int)ENUM_OBJECTLAYERLEVEL_TYPE.Character * OrderLayerGroupMaxCount;

    public int Get_SequenceOrderLayer(ENUM_OBJECTLAYERLEVEL_TYPE _layerLevelType)
    {
        if(!currOrderLayerNumDict.ContainsKey(_layerLevelType))
            return 0;

        // 만약 범위를 벗어났다면 다시 원점으로
        if(currOrderLayerNumDict[_layerLevelType] >= (((int)_layerLevelType + 1) * (OrderLayerGroupMaxCount)))
            currOrderLayerNumDict[_layerLevelType] = (int)_layerLevelType * OrderLayerGroupMaxCount;

        currOrderLayerNumDict[_layerLevelType] += 1;

        return currOrderLayerNumDict[_layerLevelType];
    }

    public void Return_OrderLayer(ENUM_OBJECTLAYERLEVEL_TYPE _layerLevelType, int _orderLayerNum)
    {
        // 만약 해당하는 LayerTag의 가장 상단에 있는 오브젝트가 비활성화됐을 때, 초기화
        currOrderLayerNumDict.TryGetValue(_layerLevelType, out int currOrderLayerNum);
        if (currOrderLayerNum == _orderLayerNum)
            currOrderLayerNumDict[_layerLevelType] = (int)_layerLevelType * OrderLayerGroupMaxCount;
    }

    public void Clear()
    {
        Init();
    }
}
