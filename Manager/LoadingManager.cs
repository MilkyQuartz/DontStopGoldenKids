using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private Image fillImg;
    private GameObject obj;


    private void Awake()
    {       
        obj = GameObject.Find("Fill");
        if (obj != null)
            fillImg = obj.GetComponent<Image>();
        else
            Debug.Log("LoadingManager.cs - Awake() - fillImg 참조 실패");

        StartCoroutine("LoadAsyncScene");
    }

    IEnumerator LoadAsyncScene()
    {

        yield return new WaitForSeconds(2f);

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(GameManager.Instance.NextScene.ToString());
        asyncScene.allowSceneActivation = false;

        float timeC = 0f;

        while (!asyncScene.isDone)
        {
            yield return null;
            timeC += Time.deltaTime;

            if (asyncScene.progress >= 0.9f)
            {
                fillImg.fillAmount = Mathf.Lerp(fillImg.fillAmount, 1f, timeC);
                if (fillImg.fillAmount >= 0.99)
                    asyncScene.allowSceneActivation = true;
            }
            else
            {
                fillImg.fillAmount = Mathf.Lerp(fillImg.fillAmount, asyncScene.progress, timeC);
                if (fillImg.fillAmount >= asyncScene.progress)
                    timeC = 0f;
            }
        }
    }
}
