using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPrefabPool : IPunPrefabPool
{
	public static void Init()
	{
		PhotonNetwork.PrefabPool = new PhotonPrefabPool();
	}

	public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
	{
		var obj = Managers.Resource.Instantiate(prefabId, position, rotation, false);
		if (obj == null)
		{
			Debug.LogError($"{prefabId} 경로가 잘못되어 프리팹을 불러올 수 없습니다.");
			return null;
		}

		if (obj.activeSelf) // 혹시 활성화되버리는 경우
		{
			obj.SetActive(false);
		}

		return obj;
	}

	public void Destroy(GameObject gameObject)
	{
		Managers.Resource.Destroy(gameObject);
	}

}
