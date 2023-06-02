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
    float maxVolume; // 설정에 따른 최대 볼륨

    Coroutine listenerCheckCoroutine = null;

    Transform targetTr = null;
    bool isFollowing = false;

    private void OnEnable()
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        if (listenerCheckCoroutine != null)
            StopCoroutine(listenerCheckCoroutine);

        isFollowing = false;
    }

    public void Play_SFX(AudioClipVolume audioClipVolume, Vector3 worldPosVec)
    {
        if(!isFollowing)
            gameObject.transform.position = worldPosVec;

        audioSource.clip = audioClipVolume.audioClip;
        maxVolume = audioClipVolume.volume;

        audioSource.volume = maxVolume;
        audioSource.Play();

        listenerCheckCoroutine = StartCoroutine(IListenerCheck_FollowingCheck());

        UnityEngine.Object.Destroy(gameObject, audioClipVolume.audioClip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void PlaySFX_FollowingSound(AudioClipVolume _audioClipVolume, Transform _target)
    {
        targetTr = _target;
        if(targetTr != null)
            isFollowing = true;

        Play_SFX(_audioClipVolume, Vector3.zero);
    }

    protected IEnumerator IListenerCheck_FollowingCheck()
    {
        while (audioSource != null)
        {
            // 팔로윙 오브젝트인지 체크
            if (isFollowing && targetTr != null)
                transform.position = targetTr.position;

            Vector2 listenerPosVec = Managers.Sound.Get_AudioListenerWorldPosVec();

            // 거리에 따른 볼륨 조절
            float distance = Vector2.Distance(transform.position, listenerPosVec);

            if (distance <= 5.0f)
                audioSource.volume = maxVolume;
            else if (distance <= 20.0f)
            {
                float subVolume = 1.0f - ((distance - 5f) / 18.75f);
                audioSource.volume = maxVolume * subVolume;
            }
            else
                audioSource.volume = maxVolume * 0.2f;

            // 거리에 따른 스테레오 조절
            float distanceX = transform.position.x - listenerPosVec.x;
            float absDistanceX = Math.Abs(distanceX);

            if (absDistanceX <= 5)
                audioSource.panStereo = 0;
            else if (absDistanceX <= 10)
            {
                float tempPanStereoValue = (absDistanceX - 5) / 5f / 2f;
                audioSource.panStereo = (distanceX < 0) ? tempPanStereoValue * -1f : tempPanStereoValue;
            }
            else
            {
                float tempPanStereoValue = 0.5f + ((absDistanceX - 10) / 10f / 2f);
                audioSource.panStereo = (distanceX < 0) ? tempPanStereoValue * -1f : tempPanStereoValue;
            }
    
            yield return null;
        }

        isFollowing = false;
        listenerCheckCoroutine = null;
    }
}
