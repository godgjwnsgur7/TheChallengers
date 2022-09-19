using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class KeyPanelArea : UIElement
{
    public PlayerCharacter player;
    public UISettingHelper settingHelper;
    [SerializeField] SettingPanel settingPanel;
    [SerializeField] Button[] buttons;

    public ENUM_CHARACTER_TYPE playerType;
    private float sizeRatio;
    private float opacityRatio;

    SubPrefsType subPrefsList;

    bool isMove = false;
    bool isAttack = false;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    // KeyPanelArea 공용
    public void Init()
    {
        playerType = ENUM_CHARACTER_TYPE.Knight;

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
            }

            SetInit(buttons[i]);
        }
    }

    private void SetInit(Button button)
    {
        InitButton(subPrefsList);

        //if (button.GetType() == typeof(SkillUI))
        //    SetSkillImage();

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

    // 아래부터 인게임용 함수--------------------------------------------------------------------
    public void PushKey(Button button)
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
}
