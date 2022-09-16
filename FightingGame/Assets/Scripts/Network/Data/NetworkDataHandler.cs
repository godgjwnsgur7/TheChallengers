using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class NetworkDataHandler : SingletonPhoton<NetworkDataHandler>
{
	public override void OnInit()
	{
		isSynchronized = false;
	}
	
	public void StartSync()
	{
		isSynchronized = true;
	}

	public void EndSync()
	{
		isSynchronized = false;
	}

	protected override void OnMineSerializeView(PhotonWriteStream writeStream)
	{
		if (!isSynchronized)
			return;

		base.OnMineSerializeView(writeStream);
	}

	protected override void OnOtherSerializeView(PhotonReadStream readStream)
	{
		if (!isSynchronized)
			return;

		base.OnOtherSerializeView(readStream);
	}
}
