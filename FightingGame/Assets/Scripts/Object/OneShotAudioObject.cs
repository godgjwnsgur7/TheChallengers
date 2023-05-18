using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// 3D 사운드 출력을 위한 스크립트
/// </summary>
public class OneShotAudioObject : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    Coroutine followingTargerCoroutine = null;

    bool isFollowing = false;

    private void OnEnable()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();

        Managers.Sound.Set_SFXSoundSetting(audioSource);
    }

    private void OnDisable()
    {
        if (followingTargerCoroutine != null)
            StopCoroutine(followingTargerCoroutine);

        isFollowing = false;
    }

    public void Play_SFX(ENUM_SFX_TYPE sfxType, AudioClipVolume audioClipVolume, Vector3 worldPosVec)
    {
        if(!isFollowing)
            gameObject.transform.position = worldPosVec;

        audioSource.clip = audioClipVolume.audioClip;

        float listenerPosX = Managers.Sound.Get_AudioListenerWorldPosX();
        float currDistance = worldPosVec.x - listenerPosX; // 거리

        if (Math.Abs(currDistance) > 3)
            audioSource.panStereo = currDistance / 10.0f;
        else
            audioSource.panStereo = 0;

        audioSource.volume = audioClipVolume.volume;
        audioSource.Play();

        UnityEngine.Object.Destroy(gameObject, audioClipVolume.audioClip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void PlaySFX_FollowingSound(ENUM_SFX_TYPE sfxType, AudioClipVolume audioClipVolume, Transform target)
    {
        isFollowing = true;

        followingTargerCoroutine = StartCoroutine(IFollowingTarget(target));

        Play_SFX(sfxType, audioClipVolume, Vector3.zero);
    }

    protected IEnumerator IFollowingTarget(Transform target)
    {
        while (isFollowing)
        {
            transform.position = target.position;
            yield return null;
        }

        isFollowing = false;
        followingTargerCoroutine = null;
    }
}
