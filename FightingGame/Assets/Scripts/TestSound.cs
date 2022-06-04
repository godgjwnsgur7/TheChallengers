using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TestSound : MonoBehaviour
{
    public AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // AudioSource audio = GetComponent<AudioSource>();
        // audio.PlayOneShot(audioClip);

        Managers.Sound.Play(ENUM_BGM_TYPE.TestBGM);
    }
}
