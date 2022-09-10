using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkDataHandler : SingletonPhoton<NetworkDataHandler>
{
	public StatusData statusData = null;

	public float CurrHP
    {
		get => statusData.currHP;
		set => statusData.currHP = value;
    }


	public override void OnInit()
	{
		statusData = new StatusData();
	}

	/// <summary>
	/// 이 쪽에서 동기화...
	/// </summary>
	/// <param name="writeStream"></param>


	protected override void OnMineSerializeView(PhotonWriteStream writeStream)
	{
		writeStream.Write(CurrHP);
		base.OnMineSerializeView(writeStream);
	}

	protected override void OnOtherSerializeView(PhotonReadStream readStream)
	{
		CurrHP = readStream.Read<float>();
		base.OnOtherSerializeView(readStream);
	}
}
