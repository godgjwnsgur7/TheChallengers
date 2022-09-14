using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class NetworkDataHandler : SingletonPhoton<NetworkDataHandler>
{
	StatusData redTeamStatusData = null;
	StatusData blueTeamStatusData = null;

	public float RedTeamCurrHP
	{
		get => redTeamStatusData.currHP;
		set => redTeamStatusData.currHP = value;
    }
	
	public float BlueTeamCurrHP
    {
		get => blueTeamStatusData.currHP;
		set => blueTeamStatusData.currHP = value;
	}

	public override void OnInit()
	{
		redTeamStatusData = new StatusData();
		blueTeamStatusData = new StatusData();
	}

	public void Set_StatusCurrHP(ENUM_TEAM_TYPE _teamType, float _currHP)
	{
		if (_teamType == ENUM_TEAM_TYPE.Red)
			redTeamStatusData.currHP = _currHP;
		else if (_teamType == ENUM_TEAM_TYPE.Blue)
			blueTeamStatusData.currHP = _currHP;
		else
			Debug.Log($"_teamType 오류 : {_teamType}");
	}

	public float Get_StatusCurrHP(ENUM_TEAM_TYPE _teamType)
	{
		if (_teamType == ENUM_TEAM_TYPE.Red)
			return redTeamStatusData.currHP;
		else if (_teamType == ENUM_TEAM_TYPE.Blue)
			return blueTeamStatusData.currHP;
		else
        {
			Debug.Log($"_teamType 오류 : {_teamType}");
			return 0;
        }
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
		writeStream.Write(RedTeamCurrHP);
		writeStream.Write(BlueTeamCurrHP);

		base.OnMineSerializeView(writeStream);
	}

	protected override void OnOtherSerializeView(PhotonReadStream readStream)
	{
		RedTeamCurrHP = readStream.Read<float>();
		BlueTeamCurrHP = readStream.Read<float>();

		base.OnOtherSerializeView(readStream);
	}
}
