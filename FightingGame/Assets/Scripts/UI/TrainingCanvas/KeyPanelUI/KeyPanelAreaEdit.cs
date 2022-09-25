using System.Collections;
using UnityEngine;
using FGDefine;
using UnityEngine.UI;

public class KeyPanelAreaEdit : UIElement
{
    public PlayerCharacter player;
    public UISettingHelper settingHelper;
    [SerializeField] SettingPanel settingPanel;

    // Panel 안의 버튼들
    // 0 LeftMoveBtn, 1 RightMoveBtn, 2 AttackBtn, 3 JumpBtn, 4 5 6 SkillBtn
    [SerializeField] Button[] buttons;

    private float sizeRatio;
    private float opacityRatio;

    public Sprite[] skillImage;

    SubPrefsType subPrefsList;
    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    /*
    // KeyPanelArea 공용함수--------------------------------------------------
    public void Init()
    {
        // Base Slider Value Call
        for (int i = 0; i < buttons.Length; i++)
        {
            // settingHelper에 버튼 등록
            settingHelper.SetBtnInit(buttons[i]);

            // 해당 버튼의 Prefs 정보 불러오기
            subPrefsList = Managers.Prefs.GetSubPrefsList(i);

            // 등록한 버튼의 위치 값을 Prefs에 갱신
            if (!subPrefsList.GetIsInit())
            {
                subPrefsList.SetTransX(settingHelper.GetTransform().x);
                subPrefsList.SetTransY(settingHelper.GetTransform().y);
                subPrefsList.SetResetTransX(settingHelper.GetTransform().x);
                subPrefsList.SetResetTransY(settingHelper.GetTransform().y);
                subPrefsList.SetIsInit(true);

                Managers.Prefs.SetSubPrefsList(subPrefsList);
                Managers.Prefs.SaveButtonPrefs(subPrefsList.GetExist());
            }

            SetInit();
        }
    }

    // UI 초기배치
    private void SetInit()
    {
        InitButton(subPrefsList);

        settingHelper.Clear();
    }

    // UI 크기, 투명도, 위치 배치
    private void InitButton(SubPrefsType prefsList)
    {
        if (prefsList == null)
            return;

        sizeRatio = (prefsList.GetSize() + 50) / 100;
        settingHelper.SetSize(sizeRatio, true);

        opacityRatio = (prefsList.GetOpacity() / 200);
        settingHelper.SetOpacity(opacityRatio, true);

        settingHelper.SetTransform(new Vector2(prefsList.GetTransX(), prefsList.GetTransY()));
    }

    // 아래부터 수정용 함수---------------------------------------------------------------
    // 전체 리셋
    public void SliderResetAll()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Reset(i);
        }

        settingHelper.Clear();
        Managers.UI.CloseUI<BottomPanel>();
    }

    // 리셋
    public void Reset(int bpType)
    {
        subPrefsList = Managers.Prefs.GetSubPrefsList(bpType);

        sizeRatio = (subPrefsList.GetResetSize() + 50) / 100; // Size Scale 0.5 ~ 1.5배
        settingHelper.SetBtnInit(buttons[bpType]);
        settingHelper.SetSize(sizeRatio);
        subPrefsList.SetSize(subPrefsList.GetResetSize());

        opacityRatio = (subPrefsList.GetResetOpacity() / 200); // 투명도 최소 값은 0.5, 추가 비율은 0 ~ 0.5f
        settingHelper.SetOpacity(opacityRatio, true);
        subPrefsList.SetOpacity(subPrefsList.GetResetOpacity());

        settingHelper.SetTransform(new Vector2(subPrefsList.GetResetTransX(), subPrefsList.GetResetTransY()));
        subPrefsList.SetTransX(subPrefsList.GetResetTransX());
        subPrefsList.SetTransY(subPrefsList.GetResetTransY());

        Managers.Prefs.SetSubPrefsList(subPrefsList);
    }
    */

    // 버튼UI UpdatableUI컴포넌트 생성
    public void SetUpdateComponentAll()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].gameObject.GetComponent<UpdatableUI>() == null)
                buttons[i].gameObject.AddComponent<UpdatableUI>().init();
        }
    }

    public void SetUpdateComponent(Button button)
    {
        button.gameObject.AddComponent<UpdatableUI>().init();
    }

    // 버튼UI UpdatableUI컴포넌트 제거
    public void RemoveUpdateComponentAll()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<UpdatableUI>() != null)
                Destroy(buttons[i].GetComponent<UpdatableUI>());
        }
    }

    public void RemoveUpdateComponent(UpdatableUI updateUI)
    {
        for (int i = 0; i < buttons.Length; i++)
            Destroy(updateUI.GetComponent<UpdatableUI>());
    }

    // UpdatableUI 컴포넌트 보유 체크
    public void UpdateComponentChk(Button button)
    {
        if (!settingPanel.isOpen)
            return;

        if (button.GetComponent<UpdatableUI>() == null)
        {
            Debug.Log("수정 불가능 : UpdatableUI Component None");
            return;
        }
            
        settingPanel.PushKey(button.GetComponent<UpdatableUI>());
    }
}
