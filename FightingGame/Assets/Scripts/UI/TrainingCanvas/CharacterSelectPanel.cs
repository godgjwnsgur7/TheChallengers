using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPanel : UIElement
{
    public string userType;
    [SerializeField] TrainingScene trainingScene;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void Set_UserType(string _userType)
    {
        userType = _userType;
        this.Open();
    }

    public void Onclick_CallCharacter(int _charType)
    {
        switch (userType)
        {
            case "Player":
                trainingScene.SelectPlayerCharacter(_charType);
                break;
            case "Enemy":
                trainingScene.SelectEnemyCharacter(_charType);
                break;
        }

        this.Close();
    }
}
