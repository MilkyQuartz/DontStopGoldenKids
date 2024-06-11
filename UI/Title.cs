using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Transform contentTransform;
    private PlayerManager playerManager;
    private AchievementList achievementList;
    private RewardList rewardList;
    public TextMeshProUGUI playerTitleText;

    void Start()
    {
        playerManager = PlayerManager.Instance;
        LoadAchievements();
        LoadRewards();
        DisplayTitles();
    }

    void LoadAchievements()
    {
        TextAsset achievementJson = Resources.Load<TextAsset>("Achievements");
        if (achievementJson != null)
        {
            achievementList = JsonUtility.FromJson<AchievementList>(achievementJson.text);
        }
        else
        {
            achievementList = new AchievementList();
        }
    }

    void LoadRewards()
    {
        TextAsset rewardJson = Resources.Load<TextAsset>("Rewards");
        if (rewardJson != null)
        {
            rewardList = JsonUtility.FromJson<RewardList>(rewardJson.text);
        }
        else
        {
            rewardList = new RewardList();
        }
    }

    // ��ũ�Ѻ信 ������ Īȣ ����� �Լ�
    public void DisplayTitles()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        List<int> userAchievements = playerManager.LoggedInUser.achievements;

        foreach (int achievementNum in userAchievements)
        {
            Achievement achievement = achievementList.achievement.Find(a => a.num == achievementNum);
            if (achievement != null && achievement.rewardType == "Title")
            {
                Reward reward = rewardList.reward.Find(r => r.rewardType == "Title" && r.rewardNum == achievement.rewardNum);
                if (reward != null)
                {
                    GameObject textObject = new GameObject("TitleText");
                    RectTransform rectTransform = textObject.AddComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(400, 50);

                    Button buttonComponent = textObject.AddComponent<Button>();
                    TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
                    textComponent.text = reward.reward;

                    textComponent.fontSize = 40;
                    textComponent.color = Color.black;

                    textComponent.font = Resources.Load<TMP_FontAsset>("Fonts/NEXONLv1GothicRegular SDF");

                    textObject.transform.SetParent(contentTransform, false);

                    int num = achievement.num;
                    buttonComponent.onClick.AddListener(() => OnTitleButtonClick(num));
                }
            }
        }
    }

    // ��ũ�Ѻ信 ��ư���� �� Īȣ Ŭ�� �̺�Ʈ
    void OnTitleButtonClick(int num)
    {
        Achievement achievement = achievementList.achievement.Find(a => a.num == num);
        if (achievement != null)
        {
            Reward reward = rewardList.reward.Find(r => r.rewardType == "Title" && r.rewardNum == achievement.rewardNum);
            if (reward != null)
            {
                UpdateUserTitle(reward.reward);
                playerTitleText.text = reward.reward;
            }
        }
    }

    // Īȣ Ŭ�������� Json ������Ʈ
    void UpdateUserTitle(string newTitle)
    {
        string userFilePath = Path.Combine(Application.persistentDataPath, "PlayerInfo.json");
        if (File.Exists(userFilePath))
        {
            string json = File.ReadAllText(userFilePath);
            UserList userList = JsonUtility.FromJson<UserList>(json);

            User currentUser = userList.users.Find(u => u.id == playerManager.LoggedInUser.id);
            if (currentUser != null)
            {
                currentUser.currentTitle = newTitle;
                playerManager.LoggedInUser.currentTitle = newTitle; 
                string updatedJson = JsonUtility.ToJson(userList, true);
                File.WriteAllText(userFilePath, updatedJson);
            }
        }
    }
}