using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchBtn : MonoBehaviour
{
    public void UnInterctable() 
    {
        gameObject.GetComponent<Button>().interactable = false;
    }

    public void Interctable() 
    {
        gameObject.GetComponent<Button>().interactable = true;
    }
}
