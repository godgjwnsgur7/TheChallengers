using System.Collections;
using UnityEngine;
using FGDefine;

public class KeyPanelArea : UIElement
{
    public PlayerCharacter player;

    // Panel 안의 버튼들
    // 0 LeftMoveBtn, 1 RightMoveBtn, 2 AttackBtn, 3 JumpBtn, 4 5 6 SkillBtn
    [SerializeField] UpdatableUI[] buttons;

    private float sizeRatio;
    private float opacityRatio;
    private Vector2 tempVector;

    private float tempX;
    private float tempY;

    public ENUM_CHARACTER_TYPE playerType;
    public Sprite[] skillImage;

    bool isMove = false;
    bool isAttack = false;

    public override void Open(UIParam param = null)
    {
        SetSkillImage();
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void init()
    {
        playerType = ENUM_CHARACTER_TYPE.Knight;

        // Base Slider Value Call
        for (int i = 0; i < buttons.Length; i++)
            SetInit(buttons[i]);
    }

    // 초기 PlayerPrefs 값 설정 및 UI 초기배치
    private void SetInit(UpdatableUI updateUI)
    {
        updateUI.init();

        // Size init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size, 50);

        // Opacity init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, 100);

        // RectTransform init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX, updateUI.GetTransform().x);
        else
            tempX = PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransX);

        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY, updateUI.GetTransform().y);
        else
            tempY = PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.TransY);

        if (tempX != 0 && tempY != 0)
            updateUI.SetTransform(new Vector2(tempX, tempY));

        // ResetSize init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize, PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size));

        // ResetOpacity init
        if (!PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity,
                PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity));

        // UI Init Value Accept
        InitSize(updateUI);
        InitOpactiy(updateUI);

        PlayerPrefs.Save();
    }

    // 초기 UI size 설정
    private void InitSize(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        sizeRatio = (PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size) + 50) / 100;

        updateUI.SetSize(sizeRatio, true);
    }

    // 초기 UI Opacity 설정
    private void InitOpactiy(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        opacityRatio = (PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity) / 200);

        updateUI.SetOpacity(opacityRatio, true);
    }

    // Reset Not Saved Slider Value
    public void SliderReset()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            SetSize(buttons[i]);
            SetOpactiy(buttons[i]);
            SetTransform(buttons[i]);
        }

        Managers.UI.CloseUI<BottomPanel>();
    }

    // Size 리셋
    private void SetSize(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        sizeRatio = (PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize) + 50) / 100;

        if (PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize))
        {
            updateUI.SetSize(sizeRatio);

            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Size, PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetSize));
        }
    }

    // Opacity 리셋
    private void SetOpactiy(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        if (PlayerPrefs.HasKey($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity))
        {
            opacityRatio = (PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity) / 200);

            updateUI.SetOpacity(opacityRatio, true);

            PlayerPrefs.SetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.Opacity, PlayerPrefs.GetFloat($"{updateUI.name}" + ENUM_PLAYERPREFS_TYPE.ResetOpacity));
        }
    }

    // Transform 리셋
    private void SetTransform(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        tempVector = new Vector2(PlayerPrefs.GetFloat($"{updateUI.thisRect.name}" + ENUM_PLAYERPREFS_TYPE.TransX),
            PlayerPrefs.GetFloat($"{updateUI.thisRect.name}" + ENUM_PLAYERPREFS_TYPE.TransY));

        updateUI.SetTransform(tempVector);
    }

    public void SetSkillImage()
    {
        Debug.Log(playerType);
        //skillImage = Managers.Resource.LoadAll<Sprite>("");
        //buttons[4].GetComponent<Image>().sprite = ;
        //buttons[5].GetComponent<Image>().sprite = ;
        //buttons[6].GetComponent<Image>().sprite = ;
    }

    public void PushKey(UpdatableUI updateUI)
    {
        Debug.Log(updateUI);

        switch (updateUI.name) 
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
                updateUI.OnCoolTime();
                break;
            case "SkillBtn2":
                CharacterSkillParam skillParam2 = new CharacterSkillParam(1);
                player.PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam2);
                updateUI.OnCoolTime();
                break;
            case "SkillBtn3":
                CharacterSkillParam skillParam3 = new CharacterSkillParam(2);
                player.PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam3);
                updateUI.OnCoolTime();
                break;
        }
    }

    public void BackIdle()
    {
        if (player == null || player.activeCharacter.currState == ENUM_PLAYER_STATE.Idle)
            return;

        if(isMove)
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
            CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_SKILL_TYPE.Knight_Attack1, player.activeCharacter.reverseState);
            player.PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            player.activeCharacter.Change_AttackState(true);
            yield return null;
        }
    }
}
