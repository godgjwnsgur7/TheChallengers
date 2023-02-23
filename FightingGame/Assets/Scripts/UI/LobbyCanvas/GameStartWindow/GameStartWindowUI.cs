using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

/// <summary>
/// 구현 중
/// 처음에 게임이 시작되면 최초에 브로드캐스트 함수롤 실행시켜 활성화됨 (유저싱크메디에이터)
/// 유저싱크메디에이터에서 
/// 캐릭터 선택 시간을 줄 것이고 시간이 다 되면, 랜덤으로 선택.
/// 마스터 선택 -> 슬레이브 선택 -> 브로드캐스트 호출 -> 게임돌입
/// 슬레이브는 마스터가 선택된걸 확인 후에 캐릭터 완료를 줄 것
/// </summary>
public class GameStartWindowUI : MonoBehaviour, IRoomPostProcess
{
    [SerializeField] UserInfoUI masterInfoUI;
    [SerializeField] UserInfoUI slaveInfoUI;

    [SerializeField] CharacterSelectArea characterSelectArea;

    [SerializeField] Text timerText;
    [SerializeField] Image Image_VS; // VS 이미지

    ENUM_CHARACTER_TYPE selectedMyCharType = ENUM_CHARACTER_TYPE.Default;
    ENUM_CHARACTER_TYPE selectedEnemyCharType = ENUM_CHARACTER_TYPE.Default;

    private void OnDisable()
    {
        this.UnregisterRoomCallback();
    }

    private void OnEnable()
    {
        Open(); // 디버그용
    }

    public void Open()
    {
        this.RegisterRoomCallback();

        characterSelectArea.Init(CharSelectCallBack);

        this.gameObject.SetActive(true);

        PhotonLogicHandler.Instance.RequestRoomCustomProperty();
        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();
    }

    public void CharSelectCallBack(ENUM_CHARACTER_TYPE _selectedCharType)
    {

    }

    public void OnUpdateRoomProperty(CustomRoomProperty property)
    {

    }

    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {

    }
}
