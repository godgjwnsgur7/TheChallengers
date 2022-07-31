using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMatchPanel : UIElement
{
    // public bool isReady = false;
    [SerializeField] Text readyText1;
    [SerializeField] Text readyText2;
    [SerializeField] Button user1;
    [SerializeField] Button user2;
    public void UserReady()
    {
        // if 문 같은걸로 버튼 누른 유저확인 후 해당 유저의 버튼을 비활성화 시키면 될 거같은데...
        user1.interactable = !user1.interactable;
        user2.interactable = !user2.interactable;

        readyText1.gameObject.SetActive(!readyText1.IsActive());
        readyText2.gameObject.SetActive(!readyText2.IsActive());

        // 이 뒤에 전부 레디했는지 확인하면 되지 않을까 싶다...
        
    }
}
