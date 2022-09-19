using System.Collections;
using UnityEngine;
using FGDefine;
using UnityEngine.UI;

public class KeyPanelArea : UIElement
{
    public PlayerCharacter player;
    public UISettingHelper settingHelper;
    [SerializeField] SettingPanel settingPanel;

    // Panel 안의 버튼들
    // 0 LeftMoveBtn, 1 RightMoveBtn, 2 AttackBtn, 3 JumpBtn, 4 5 6 SkillBtn
    [SerializeField] Button[] buttons;

    private float sizeRatio;
    private float opacityRatio;

    public ENUM_CHARACTER_TYPE playerType;
    public Sprite[] skillImage;

    bool isMove = false;
    bool isAttack = false;

    // 0 Exist, 1 Size, 2 Opacity, 3 ResetSize, 4 ResetOpacity, 5 TransX, 6 TransY, 7 ResetTransX, 8 ResetTransY, 9 Isinit
    private float[] prefsList;
    public override void Open(UIParam param = null)
    {
        SetSkillImage();
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void Init()
    {
        playerType = ENUM_CHARACTER_TYPE.Knight;

        // Base Slider Value Call
        for (int i = 0; i < buttons.Length; i++)
        {
            ENUM_BTNPREFS_TYPE bpType = (ENUM_BTNPREFS_TYPE)i;

            // settingHelper에 버튼 등록
            settingHelper.SetBtnInit(buttons[i]);

            // 해당 버튼의 Prefs 정보 불러오기
            prefsList = Managers.Prefs.GetPrefsList(bpType);

            // 등록한 버튼의 위치 값을 Prefs에 갱신
            if (prefsList[9] == 0)
            {
                prefsList[5] = settingHelper.GetTransform().x;
                prefsList[6] = settingHelper.GetTransform().y;
                prefsList[7] = settingHelper.GetTransform().x;
                prefsList[8] = settingHelper.GetTransform().y;
                prefsList[9] = 1;

                Managers.Prefs.SetPrefsList(bpType, prefsList);
            }

            SetInit(buttons[i]);
        }
    }

    // UI 초기배치
    private void SetInit(Button button)
    {
        InitButton(prefsList);

/*        if (button.GetType() == typeof(SkillUI))
            SetSkillImage();*/

        settingHelper.Clear();
    }

    // UI 크기, 투명도, 위치 배치
    private void InitButton(float[] prefsList)
    {
        if (prefsList == null)
            return;

        sizeRatio = (prefsList[1] + 50) / 100;
        settingHelper.SetSize(sizeRatio, true);

        opacityRatio = (prefsList[2] / 200);
        settingHelper.SetOpacity(opacityRatio, true);

        settingHelper.SetTransform(new Vector2(prefsList[5], prefsList[6]));
    }

    // 전체 리셋
    public void SliderResetAll()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Reset((ENUM_BTNPREFS_TYPE)i);
        }

        settingHelper.Clear();
        Managers.UI.CloseUI<BottomPanel>();
    }

    // 리셋
    public void Reset(ENUM_BTNPREFS_TYPE bpType)
    {
        prefsList = Managers.Prefs.GetPrefsList(bpType);

        sizeRatio = (prefsList[3] + 50) / 100; // Size Scale 0.5 ~ 1.5배
        settingHelper.SetBtnInit(buttons[(int)bpType]);
        settingHelper.SetSize(sizeRatio);
        prefsList[1] = prefsList[3];

        opacityRatio = (prefsList[4] / 200); // 투명도 최소 값은 0.5, 추가 비율은 0 ~ 0.5f
        settingHelper.SetOpacity(opacityRatio, true);
        prefsList[2] = prefsList[4];

        settingHelper.SetTransform(new Vector2(prefsList[7], prefsList[8]));
        prefsList[5] = prefsList[7];
        prefsList[6] = prefsList[8];

        Managers.Prefs.SetPrefsList((ENUM_BTNPREFS_TYPE)prefsList[0], prefsList);
    }

    public void SetSkillImage()
    {
        Debug.Log(playerType);
        //skillImage = Managers.Resource.LoadAll<Sprite>("");
        //buttons[4].GetComponent<Image>().sprite = ;
        //buttons[5].GetComponent<Image>().sprite = ;
        //buttons[6].GetComponent<Image>().sprite = ;
    }

    public void PushKey(Button button)
    {
        if (settingPanel.isOpen)
        {
            UpdateComponentChk(button);
        }
        else
        {
            switch (button.name)
            {
                case "LeftMoveBtn":
                    isMove = true;
                    StartCoroutine(MoveState(-1.0f));
                    break;
                case "RightMoveBtn":
                    isMove = true;
                    StartCoroutine(MoveState(1.0f));
                    break;
                case "AttackBtn":
                    isAttack = true;
                    StartCoroutine(AttackState());
                    break;
                case "JumpBtn":
                    player.PlayerCommand(ENUM_PLAYER_STATE.Jump);
                    break;
                case "SkillBtn1":
                    CharacterSkillParam skillParam1 = new CharacterSkillParam(0);
                    player.PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam1);
                    break;
                case "SkillBtn2":
                    CharacterSkillParam skillParam2 = new CharacterSkillParam(1);
                    player.PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam2);
                    break;
                case "SkillBtn3":
                    CharacterSkillParam skillParam3 = new CharacterSkillParam(2);
                    player.PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam3);
                    break;
            }
        }
    }

    // UpdatableUI 컴포넌트 보유 체크
    public void UpdateComponentChk(Button button)
    {
        if (settingPanel.isOpen && button.GetComponent<UpdatableUI>() == null)
        {
            SetUpdateComponent(button);
            settingPanel.PushKey(button.GetComponent<UpdatableUI>());
        }
        else if(settingPanel.isOpen && button.GetComponent<UpdatableUI>() != null)
            settingPanel.PushKey(button.GetComponent<UpdatableUI>());
    }

    public void BackIdle()
    {
        if (player == null || player.activeCharacter.currState == ENUM_PLAYER_STATE.Idle)
            return;

        if (isMove)
            isMove = false;

        if (isAttack)
        {
            player.activeCharacter.Change_AttackState(false);
            isAttack = false;
        }

        if (player.moveDir != 0)
            player.moveDir = 0f;

        player.PlayerCommand(ENUM_PLAYER_STATE.Idle);
    }

    IEnumerator MoveState(float direction)
    {
        while (isMove)
        {
            player.moveDir = direction;
            player.PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(player.moveDir));
            yield return null;
        }
    }

    IEnumerator AttackState()
    {
        while (isAttack)
        {
            CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_ATTACKOBJECT_NAME.Knight_Attack1, player.activeCharacter.reverseState);
            player.PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            player.activeCharacter.Change_AttackState(true);
            yield return null;
        }
    }

    public void SetUpdateComponentAll()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if(buttons[i].gameObject.GetComponent<UpdatableUI>() == null)
                buttons[i].gameObject.AddComponent<UpdatableUI>().init();
        }
    }

    public void SetUpdateComponent(Button button)
    {
        button.gameObject.AddComponent<UpdatableUI>().init();
    }

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
}
