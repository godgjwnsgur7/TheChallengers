using Photon.Pun;

/// <summary>
/// 네트워크 통신이 필요한 MonoBehaviour 클래스는 대신 해당 클래스를 사용하시오. 
/// </summary>

public class MonoBehaviourPhoton : MonoBehaviourPun
{
    protected virtual void OnEnable()
    {
        PhotonLogicHandler.Register(this);
    }

    protected virtual void OnDisable()
    {
        PhotonLogicHandler.Unregister(this);
    }
}
