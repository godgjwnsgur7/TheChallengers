using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 요기에 동기화할 데이터들을 쌓아서 한번에...
/// byte, short, int, float, double, string + 이걸 쓴 배열들만...
/// </summary>

[CreateAssetMenu(fileName = "NetworkData", menuName = "Make NetworkData For Synchronize", order = 1)]
public class NetworkData : ScriptableObject
{
	public int hpData = 0;
}
