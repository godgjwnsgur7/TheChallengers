using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Test> MakeDict();
}

// 예제코드만 존재하는 상태
public class DataMgr
{
    // 게임 내의 모든 정적 데이터를 관리하는 매니저가 될 것.
    // + 게임 내의 설정값 등을 PlayerPref을 활용하여 저장해놓을 예정 (미구현)

    public Dictionary<int, Test> testDict { get; private set; } = new Dictionary<int, Test>();

    public void Init()
    {
        testDict = LoadJson<TestData, int, Test>("TestData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
