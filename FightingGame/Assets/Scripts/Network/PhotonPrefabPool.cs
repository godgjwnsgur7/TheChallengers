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
		var obj = Managers.Resource.Instantiate(prefabId, position, rotation);
		return obj;
	}

	public void Destroy(GameObject gameObject)
	{
		Managers.Resource.Destroy(gameObject);
	}

}
