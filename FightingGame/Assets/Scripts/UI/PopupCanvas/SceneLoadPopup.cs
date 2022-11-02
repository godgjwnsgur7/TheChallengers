using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using UnityEngine.SceneManagement;

public class SceneLoadPopup : PopupUI
{
    [SerializeField] Image progressBar;

    public void Open(ENUM_SCENE_TYPE _sceneType)
    {
        this.gameObject.SetActive(true);
        
        
        StartCoroutine(SyncLoadScene(_sceneType));
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator SyncLoadScene(ENUM_SCENE_TYPE _sceneType)
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(_sceneType.ToString());
        op.allowSceneActivation = false; // 90% 로드하고 대기상태로 들어감
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                // 일단 90% 로드가 완료되면 1초에 걸쳐 100%를 만들고 로드 (임시)
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    Close();
                    yield break;
                }
            }
        }
    }
}
