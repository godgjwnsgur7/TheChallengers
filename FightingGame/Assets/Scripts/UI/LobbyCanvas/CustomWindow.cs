using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWindow : MonoBehaviour
{
    [SerializeField] LobbyList lobbyList;

    public void OnClick_UpdateList()
    {

    }

    public void OnClick_Exit() => this.gameObject.SetActive(false);
}
