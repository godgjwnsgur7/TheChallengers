using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

[System.Serializable]
public class NetworkData
{
	public int hp = 0;

	// 데이터 이 곳에 추가
}

/// <summary>
/// 양 쪽 클라에 모두 있어야 해서... 초기화가 비동기적임
/// 그러므로 초기화가 됐는 지 확인한 이후 씬을 시작해야 함
/// 안 그러면 접근조차 위험하다.
/// </summary>
public class NetworkDataHandler : SingletonPhoton<NetworkDataHandler>
{
	private bool isSynchronized = false;
	private bool isMasterHandler = false;

	public static NetworkData MasterData => masterInstance?.data;
	public static NetworkData SlaveData => slaveInstance?.data;

	// 관리되는 데이터 대상
	NetworkData data = new NetworkData();

	public override void OnInit()
	{
		isMasterHandler = PhotonLogicHandler.IsMasterClient;
	}

	#region 마스터만 이 두 함수의 제어권을 갖는다.
	public void StartSync()
	{
		if (!isMasterHandler)
			return;

		isSynchronized = true;

		slaveInstance?.StartSync();
	}

	public void EndSync()
	{
		if (!isMasterHandler)
			return;

		isSynchronized = false;

		slaveInstance?.EndSync();
	}
	#endregion

	protected override void OnMineSerializeView(PhotonWriteStream writeStream)
	{
		if (!isSynchronized)
			return;

		writeStream.Write(data.hp);
		// 이 곳에 데이터 목록 추가

		base.OnMineSerializeView(writeStream);
	}

	protected override void OnOtherSerializeView(PhotonReadStream readStream)
	{
		if (!isSynchronized)
			return;

		data.hp = readStream.Read<int>();
		// 이 곳에 데이터 목록 추가

		base.OnOtherSerializeView(readStream);
	}
}
