using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour
{
    [SerializeField] private MainGameSceneManager gameSceneManager;
    [Header("Time")]
    [SerializeField] private TextMeshProUGUI timeTxt;
    private float time;
    private int min;
    private int sec;
    private int miliSec;
    private bool isGamePaused = false;
    [Header("GameOverPanel")]
    [SerializeField] private GameObject overPanel;
    [SerializeField] private TextMeshProUGUI overTime;
    [SerializeField] private TextMeshProUGUI leftDistance;

    [Header("GamePausePanel")]
    [SerializeField] private GameObject pausePanel;

    [Header("GameClearPanel")]
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI clearScore;
    [SerializeField] private TextMeshProUGUI timeBonus;
    [SerializeField] private TextMeshProUGUI bossHealthBonus;
    [SerializeField] private TextMeshProUGUI distanceBonus;
    [SerializeField] private TextMeshProUGUI sumScore;

    public void StartStopWatch()
    {
        StartCoroutine(StopWatch());
    }

    IEnumerator StopWatch()
    {
        while (!isGamePaused)
        {
            time += Time.deltaTime;
            if (time >= 60)
            {
                min++;
                time -= 60;
            }
            sec = (int)time;
            miliSec = (int)((time - sec) * 100);
            timeTxt.text = $"{min:00}:{sec:00}:{miliSec:00}";
            yield return null;
        }
    }

    public void OnGameOver()
    {
        overPanel.SetActive(true);
        overTime.text = $"{min:00}:{sec:00}:{miliSec:00}";
        leftDistance.text = gameSceneManager.CalculateGoalDistance().ToString("N2") + "M";
        SaveInfo();
    }
    public void OnGamePause()
    {
        isGamePaused = true;
        pausePanel.SetActive(true);
        SaveInfo();
    }

    public void OnGamePauseFalse()
    {
        isGamePaused = false;
        pausePanel.SetActive(false);
        LoadInfo();
    }

    public void OnGameClear()
    {
        clearPanel.SetActive(true);
        GameObject monster = GameObject.FindWithTag("Monster");
        int _clearScore = gameSceneManager.clearScore;
        int _timeBonus = Mathf.Max(((gameSceneManager.min * 60 + gameSceneManager.sec) - (min * 60 + sec)) * gameSceneManager.perSec, 0);
        int _bossHealthBonus = 0;//monster.GetComponent<BossController>().GetHealthDiff()*gameSceneManager.perBossHealth;
        int _distanceBonus = (int)(monster.transform.position-gameSceneManager.player.transform.position).magnitude*gameSceneManager.perDistance;
        int _sumScore = _clearScore + _timeBonus + _bossHealthBonus + _distanceBonus;

        clearScore.text = _clearScore.ToString() + "점";
        timeBonus.text = _timeBonus > 0 ? _timeBonus.ToString() + "점" : "0점";
        bossHealthBonus.text = _bossHealthBonus.ToString() + "점";
        distanceBonus.text = _distanceBonus.ToString() + "점";
        sumScore.text = _sumScore.ToString() + "점";

        SaveInfo();

    }

    public void SaveInfo()
    {
        PlayerPrefs.SetInt("GameMin", min);
        PlayerPrefs.SetInt("GameSec", sec);
        PlayerPrefs.SetInt("GameSec", miliSec);
        PlayerPrefs.SetFloat("GameDistance", gameSceneManager.CalculateGoalDistance());
    }
    public void LoadInfo()
    {
        min = PlayerPrefs.GetInt("GameMin", min);
        sec = PlayerPrefs.GetInt("GameSec", sec);
        miliSec = PlayerPrefs.GetInt("GameMiliSec", miliSec);
    }
}