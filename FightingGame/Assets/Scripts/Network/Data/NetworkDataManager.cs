using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDataHandler : SingletonPhoton<NetworkDataHandler>
{
	private NetworkData data = Resources.Load<NetworkData>("Data/NetworkData");

	public float GetHP() => data.hpData;

	public override void OnInit()
	{
		// 초기값 세팅...
	}

	/// <summary>
	/// 이 쪽에서 동기화...
	/// </summary>
	/// <param name="writeStream"></param>


	protected override void OnMineSerializeView(PhotonWriteStream writeStream)
	{
		writeStream.Write(data.hpData);
		base.OnMineSerializeView(writeStream);
	}

	protected override void OnOtherSerializeView(PhotonReadStream readStream)
	{
		data.hpData = readStream.Read<float>();
		base.OnOtherSerializeView(readStream);
	}
}
