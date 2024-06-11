using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameSceneManager : MonoBehaviour
{
    public GameObject player;
    private PlayerController controller;
    public MainGameUI gameUI;
    public int clearScore;
    [Header("ExtraScore")]
    public int perSec;
    public int perBossHealth;
    public int perDistance;

    [Header("Estimated Time")]
    public int min;
    [Range(0, 60)]
    public int sec;

    [Header("GoalPosition")]
    public List<Transform> goalTransforms;

    private float minDistance = 0;

    private void Awake()
    {
        if(!player.TryGetComponent<PlayerController>(out controller))
        {
            Debug.Log("MainGameSceneManager.cs - Awake() - controller 참조실패");
        }
    }

    private void Start()
    {
        InitializePlayer();
        Invoke("InitGame", 5);
    }

    private void InitGame()
    {
        controller.InitController();
        gameUI.StartStopWatch();
    }

    public void InitializePlayer()
    {
        PlayerManager.Instance.CreateCharacterInGame(player.transform);
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        gameUI.OnGameOver();
    }

    public void GamePause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Time.timeScale = 0;
            gameUI.OnGamePause();
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        gameUI.OnGamePauseFalse();
    }

    public void GameClear()
    {
        Time.timeScale = 0;
        gameUI.OnGameClear();
    }
    public void OnBackLobbyButton()
    {
        //SaveJson?
        GameManager.Instance.AsyncLoadNextScene(SceneName.LobbyScene);
        Time.timeScale = 1;
    }
    public float CalculateGoalDistance()
    {
        foreach(Transform transforms in goalTransforms)
        {
            float calcualtedDistance = (player.transform.position - transforms.position).magnitude;
            if (calcualtedDistance < minDistance || minDistance == 0)
            {
                minDistance = calcualtedDistance;
            }
        }
        return minDistance;
    }
}
