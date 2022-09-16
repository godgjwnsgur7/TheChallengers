using System.Collections;
using UnityEngine;
using FGDefine;

public class KeyPanelArea : UIElement
{
    public PlayerCharacter player;
    public UISettingHelper settingHelper;

    // Panel 안의 버튼들
    // 0 LeftMoveBtn, 1 RightMoveBtn, 2 AttackBtn, 3 JumpBtn, 4 5 6 SkillBtn
    [SerializeField] UpdatableUI[] buttons;

    private float sizeRatio;
    private float opacityRatio;
    private Vector2 tempVector;

    private float tempX;
    private float tempY;
    private object[] initPrefsValue;

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
        settingHelper.SetBtnInit(updateUI);

        // PlayerPrefs 초기 값 세팅
        initPrefsValue = new object[(int)ENUM_PLAYERPREFS_TYPE.Max] 
        {
            0f, 50f, 100f, 50f, 100f, settingHelper.GetTransform().x, settingHelper.GetTransform().y
        };

        Managers.Prefs.SetInitValue(initPrefsValue);
        Managers.Prefs.SetPrefsAll(updateUI.name);

        // UI Init Value Accept
        InitSize(updateUI);
        InitOpactiy(updateUI);
        InitTransform(updateUI);

        PlayerPrefs.Save();
        settingHelper.Clear();
    }

    // 초기 UI size 설정
    private void InitSize(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        sizeRatio = (Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.Size, updateUI.name) + 50) / 100;

        settingHelper.SetSize(sizeRatio, true);
    }

    // 초기 UI Opacity 설정
    private void InitOpactiy(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        opacityRatio = (Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.Opacity, updateUI.name) / 200);

        settingHelper.SetOpacity(opacityRatio, true);
    }

    private void InitTransform(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        tempX = Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.TransX, updateUI.name);
        tempY = Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.TransY, updateUI.name);

        settingHelper.SetTransform(new Vector2(tempX, tempY));
    }

    public void Reset(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        SetSize(updateUI);
        SetOpactiy(updateUI);
        SetTransform(updateUI);
    } 

    // Reseting Not Saved Slider Value of All Btn
    public void SliderResetAll()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Reset(buttons[i]);
        }

        settingHelper.Clear();
        Managers.UI.CloseUI<BottomPanel>();
    }

    // Size 리셋
    private void SetSize(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        if (Managers.Prefs.HasKey(ENUM_PLAYERPREFS_TYPE.ResetSize, updateUI.name))
        {
            sizeRatio = (Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.ResetSize, updateUI.name) + 50) / 100;

            settingHelper.SetBtnInit(updateUI);
            settingHelper.SetSize(sizeRatio);

            Managers.Prefs.SetPlayerPrefs<float>(Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.ResetSize, updateUI.name)
                ,ENUM_PLAYERPREFS_TYPE.Size, updateUI.name);
        }
    }

    // Opacity 리셋
    private void SetOpactiy(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        if (Managers.Prefs.HasKey(ENUM_PLAYERPREFS_TYPE.ResetOpacity, updateUI.name))
        {
            opacityRatio = (Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.ResetOpacity, updateUI.name) / 200);

            settingHelper.SetBtnInit(updateUI);
            settingHelper.SetOpacity(opacityRatio, true);

            Managers.Prefs.SetPlayerPrefs<float>(Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.ResetOpacity, updateUI.name)
                ,ENUM_PLAYERPREFS_TYPE.Opacity, updateUI.name);
        }
    }

    // Transform 리셋
    private void SetTransform(UpdatableUI updateUI)
    {
        if (updateUI == null)
            return;

        tempVector = new Vector2(Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.TransX, updateUI.name),
            Managers.Prefs.GetPrefsFloat(ENUM_PLAYERPREFS_TYPE.TransY, updateUI.name));

        settingHelper.SetBtnInit(updateUI);
        settingHelper.SetTransform(tempVector);
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
            CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_ATTACKOBJECT_NAME.Knight_Attack1, player.activeCharacter.reverseState);
            player.PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            player.activeCharacter.Change_AttackState(true);
            yield return null;
        }
    }
}
