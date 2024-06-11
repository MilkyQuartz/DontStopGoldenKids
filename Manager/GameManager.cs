using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneName
{
    IntroScene,
    LoadingScene,
    LobbyScene,
    MainGameScene
}
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if(instance == null)
                {
                    GameObject gameManager = new GameObject("GameManager");
                    gameManager.AddComponent<GameManager>();
                    instance = gameManager.GetComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #region LoadingSceneLogic

    private SceneName nextLoadSceneName;
    public SceneName NextScene
    {
        get => nextLoadSceneName;
    }

    public void AsyncLoadNextScene(SceneName nextName)
    {
        nextLoadSceneName = nextName;

        SceneManager.LoadScene(SceneName.LoadingScene.ToString());
    }
    #endregion
}
