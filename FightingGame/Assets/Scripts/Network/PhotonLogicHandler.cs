using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public interface IPhotonNetwork
{
    public bool IsConnected
    {
        get;
    }

    public bool IsMasterClient
    {
        get;
    }
}

public partial class PhotonLogicHandler : IPhotonNetwork
{
    public bool IsConnected
    {
        get 
        {
            return PhotonNetwork.IsConnected;
        }
    }

    public bool IsMasterClient
    {
        get
        {
            return PhotonNetwork.IsMasterClient;
        }
    }
}

public delegate void DisconnectCallBack(DisconnectCause cause);
public delegate void FailedCallBack(short returnCode, string message); 

public partial class PhotonLogicHandler : MonoBehaviourPunCallbacks
{
    private readonly string GameVersion = "1";
    private static List<MonoBehaviourPhoton> punList = new List<MonoBehaviourPhoton>();

    private Action _OnConnectedToMaster = null;
    private DisconnectCallBack _OnDisconnectedFromMaster = null;

    private Action _OnJoinRoom = null;
    private FailedCallBack _OnJoinRoomFailed = null;

    public bool IsMine(MonoBehaviourPhoton pun)
    {
        var obj = punList.Find(pun => pun.Equals(pun));

        if (obj != null && obj.photonView != null)
        {
            return obj.photonView.IsMine;
        }

        return false;
    }

    #region Register 계열 외부 함수, MonobehaviourPun을 등록, 파기할 때 사용
    public static void Register(MonoBehaviourPhoton pun)
    {
        if (!punList.Exists(pun => pun.Equals(pun)))
        {
            punList.Add(pun);
        }
        else
        {
            Debug.LogWarning($"같은 MonoBehaviourPhoton 오브젝트를 추가하려 들었음. {pun}");
        }
    }

    public static void Unregister(MonoBehaviourPhoton pun)
    {
        if (punList.Exists(pun => pun.Equals(pun)))
        {
            punList.Remove(pun);
        }
        else
        {
            Debug.LogWarning($"등록되지 않은 MonoBehaviourPhoton 오브젝트를 제거하려 들었음. {pun}");
        }
    }

    #endregion

    #region Try 계열 외부 함수

    /// <summary>
    /// 마스터 서버에 접속을 시도합니다. 해당 함수가 성공된 상태여야 다른 네트워크 함수를 사용할 수 있습니다.
    /// </summary>
    /// <param name="_OnConnectedToMaster"> 마스터 서버 접속 성공 시 불리는 콜백 </param>
    /// <param name="_OnDisconnectedFromMaster"> 마스터 서버 접속이 실패했거나, 접속이 끊어졌을 때 불리는 콜백 </param>
    /// <returns> 성공 여부 </returns>

    public bool TryConnectToMaster(Action _OnConnectedToMaster = null, DisconnectCallBack _OnDisconnectedFromMaster = null)
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.LogError("이미 마스터 서버에 연결되어 있는 상태에서 연결을 시도했습니다.");
            return false;
        }
            
        this._OnConnectedToMaster = _OnConnectedToMaster;
        this._OnDisconnectedFromMaster = _OnDisconnectedFromMaster;

        PhotonNetwork.GameVersion = GameVersion;
        return PhotonNetwork.ConnectUsingSettings(); // 마스터 서버 접속 시도
    }

    /// <summary>
    /// 랜덤 룸에 접속을 시도합니다.
    /// </summary>
    /// <param name="_OnJoinRoom"> 룸 접속에 성공했을 때 불리는 콜백 </param>
    /// <param name="_OnJoinRoomFailed">룸 접속에 실패했을 때 불리는 콜백 </param>
    /// <returns> 성공 여부 </returns>

    public bool TryJoinRandomRoom(Action _OnJoinRoom, FailedCallBack _OnJoinRoomFailed)
    {
        Debug.Log($"랜덤 룸에 접속을 시도합니다.");

        this._OnJoinRoom = _OnJoinRoom;
        this._OnJoinRoomFailed = _OnJoinRoomFailed;

        return PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// 방장이 해당 함수를 호출하게 되면, 방의 멤버가 모두 씬이 동기화된 채로 이동하게 됨
    /// </summary>
    /// <param name="sceneName"></param>

    public void TrySceneLoadWithRoomMember(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void TrySceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 방을 새로 만듦
    /// </summary>
    /// <param name="roomName"> 방의 이름 </param>
    /// <param name="maxPlayerCount"> 방 인원 최대 수 </param>
    /// <returns> 성공 여부 </returns>

    public bool TryCreateRoom(string roomName = "이름 없음", int maxPlayerCount = 2)
    {
        return PhotonNetwork.CreateRoom(roomName, new RoomOptions() 
        { 
            MaxPlayers = (byte)maxPlayerCount
        });
    }

    /// <summary>
    /// 프리팹을 룸 내 모든 인원에 대하여 생성
    /// </summary>
    /// <param name="prefabPath">리소스 폴더를 기준으로 한 프리팹 경로</param>
    /// <param name="pos"></param>
    /// <param name="quaternion"></param>

    public void TryInstantiate(string prefabPath, Vector3 pos = default, Quaternion quaternion = default)
    {
        PhotonNetwork.Instantiate(prefabPath, pos, quaternion);
    }

    #endregion

    #region 포톤 자체 콜백 - 가급적 건드리지 마시오.

    /// <summary>
    /// 마스터 서버 접속 시도 성공 시 콜백
    /// </summary>

    public override void OnConnectedToMaster() 
    {
        Debug.Log("마스터 서버에 성공적으로 접속했습니다.");
        _OnConnectedToMaster?.Invoke();
    }

    /// <summary>
    /// 접속 중 접속이 끊어졌을 때 불리는 콜백
    /// </summary>
    /// <param name="cause"></param>

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"마스터 서버로부터 접속이 끊어졌습니다. 사유 : {cause}");
        _OnDisconnectedFromMaster?.Invoke(cause); 
    }

    /// <summary>
    /// Room에 참가했을 때 불리는 콜백
    /// </summary>
    /// 

    public override void OnJoinedRoom()
    {
        Debug.Log($"룸에 성공적으로 접속하였습니다.");
        _OnJoinRoom?.Invoke();
    }

    /// <summary>
    /// Room 참가에 실패했을 때 불리는 콜백
    /// </summary> 
    /// <param name="returnCode"></param> 
    /// <param name="message"></param>

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"룸 접속에 실패하였습니다. 코드 : {returnCode}, 사유 : {message}");
        _OnJoinRoomFailed?.Invoke(returnCode, message);
    }

    #endregion
}
