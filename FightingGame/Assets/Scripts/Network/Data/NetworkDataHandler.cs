using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

// 일단 안쓰는 걸로 가.
public class NetworkDataHandler : SingletonPhoton<NetworkDataHandler>
{
	public override void OnInit()
	{

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


	protected override void OnMineSerializeView(PhotonWriteStream writeStream)
	{

		base.OnMineSerializeView(writeStream);
	}

	protected override void OnOtherSerializeView(PhotonReadStream readStream)
	{

		base.OnOtherSerializeView(readStream);
	}
}
