using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
public class MatchTimeWindow : UIElement
{
    [SerializeField] MatchBtn matchBtn;
    public ENUM_CHARACTER_TYPE charType;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    private void OnEnable()
    {
        matchBtn.UnInterctable();
        // 매칭버튼으로 인해 활성화 시 charType에 맞춰 선택창에 캐릭터이미지 출력

        // 매칭서버 연결?
        
    }

    public void StopMatch() 
    {
        // 서버매칭 종료

        // 매칭창 비활성화
        Managers.UI.CloseUI<MatchTimeWindow>();

        matchBtn.Interctable();
    }
}
