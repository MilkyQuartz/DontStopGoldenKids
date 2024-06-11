using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public Transform contentTransform;
    public GameObject descriptionMenu;
    public TextMeshProUGUI description;
    public TextMeshProUGUI reward;
    public Button CheckBtn;

    private AchievementList achievementList;
    private RewardList rewardList;
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = PlayerManager.Instance;
        CheckBtn.onClick.AddListener(OnCheckBtn);

        LoadAndDisplayAchievements();
    }

    public void OnCheckBtn()
    {
        descriptionMenu.SetActive(false);
    }

    public void LoadAndDisplayAchievements()
    {
        LoadAchievements();
        LoadRewards();
        DisplayAchievements();
    }

    // 업적 로드 함수
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

    // 보상 로드 함수
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

    void DisplayAchievements()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        List<int> userAchievements = playerManager.LoggedInUser.achievements;

        foreach (var achievement in achievementList.achievement)
        {
            GameObject buttonObject = new GameObject("AchievementButton");
            buttonObject.AddComponent<RectTransform>();
            Button buttonComponent = buttonObject.AddComponent<Button>();
            TextMeshProUGUI textComponent = buttonObject.AddComponent<TextMeshProUGUI>();

            textComponent.text = achievement.num + ". " + achievement.name;
            textComponent.fontSize = 40;
            textComponent.font = Resources.Load<TMP_FontAsset>("Fonts/NEXONLv1GothicRegular SDF");

            if (userAchievements.Contains(achievement.num))
            {
                textComponent.color = Color.green;
            }
            else
            {
                textComponent.color = Color.red;
            }

            buttonObject.transform.SetParent(contentTransform, false);

            int num = achievement.num;
            buttonComponent.onClick.AddListener(() => OnAchievementButtonClick(num));
        }
    }

    void OnAchievementButtonClick(int achievementNum)
    {
        if (descriptionMenu != null)
        {
            descriptionMenu.SetActive(true);
        }
        else
        {
            Debug.LogError("메뉴판 연결 안됨 확인 필요");
            return;
        }

        Achievement achievement = achievementList.achievement.Find(a => a.num == achievementNum);

        if (achievement != null)
        {
            if (description != null)
            {
                description.text = "업적 달성 조건: " + achievement.description;
            }

            Reward rewardInfo = FindReward(achievement.rewardType, achievement.rewardNum);
            if (rewardInfo != null)
            {
                if (reward != null && achievement.rewardType == "Title")
                {
                    reward.text = "보상 - 칭호: " + rewardInfo.reward;
                }
                else
                {
                    reward.text = "보상 - 캐릭터: " + rewardInfo.reward;
                }
            }
        }
    }

    Reward FindReward(string rewardType, int rewardNum)
    {
        return rewardList?.reward?.Find(r => r.rewardType == rewardType && r.rewardNum == rewardNum);
    }
}
