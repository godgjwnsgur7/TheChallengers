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

	public void StartSync()
	{
		if(!PhotonLogicHandler.IsMasterClient)
		{
			Debug.LogWarning("마스터 클라이언트가 아닌 유저가 데이터 싱크를 시도합니다.");
			EndSync();
			return;
		}
	}

	public void EndSync()
	{
		if (!PhotonLogicHandler.IsMasterClient)
		{
			Debug.LogWarning("마스터 클라이언트가 아닌 유저가 데이터 싱크 해제를 시도합니다.");
			return;
		}

		PhotonLogicHandler.Instance.TryDestroy(this);
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
