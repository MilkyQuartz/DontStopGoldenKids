using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuest
{
    void Complete();
    RewardList GetRewards();
}

public class SettingsQuest : IQuest
{
    private int questNum;

    public SettingsQuest(int questNum)
    {
        this.questNum = questNum;
    }

    public void Complete()
    {
        RewardList rewards = GetRewards();
        QuestManager.Instance.CompleteQuest(questNum);
    }

    public RewardList GetRewards()
    {
        return new RewardList();
    }
}
public class ClearCountQuest : IQuest
{
    private int questNum; 

    public ClearCountQuest(int questNum)
    {
        this.questNum = questNum;
    }

    public void Complete()
    {
        User currentUser = PlayerManager.Instance.LoggedInUser;
        if (currentUser != null)
        {
            currentUser.clearCount++;
            PlayerManager.Instance.SaveUserData();
        }

        // 퀘스트 완료 처리
        RewardList rewards = GetRewards();
        QuestManager.Instance.CompleteQuest(questNum);
    }

    public RewardList GetRewards()
    {
        // 클리어 횟수가 1일 때 업적 달성
        if (PlayerManager.Instance.LoggedInUser.clearCount == 1)
        {
            return new RewardList();
        }
        else
        {
            return new RewardList(); 
        }
    }
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public GameObject popupPrefab;
    public float displayDuration = 3f;
    private GameObject popupObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteQuest(int questNum)
    {
        User currentUser = PlayerManager.Instance.LoggedInUser;

        if (currentUser != null && !currentUser.achievements.Contains(questNum))
        {
            currentUser.achievements.Add(questNum);
            Achievement achievement = GetAchievementByNum(questNum);

            if (achievement != null && achievement.rewardType == "Character")
            {
                string characterName = GetCharacterByNumber(achievement.rewardNum);

                if (!string.IsNullOrEmpty(characterName))
                {
                    currentUser.ownedCharacters.Add(characterName);
                    Debug.Log("보상으로 추가된 캐릭터: " + characterName);
                }
                else
                {
                    Debug.LogWarning("캐릭터 이름이 없습니다.");
                }
            }

            PlayerManager.Instance.SaveUserData();
            ShowPopup();
        }
    }

    private Achievement GetAchievementByNum(int questNum)
    {
        TextAsset achievementJson = Resources.Load<TextAsset>("Achievements");
        if (achievementJson != null)
        {
            AchievementList achievementList = JsonUtility.FromJson<AchievementList>(achievementJson.text);
            return achievementList.achievement.Find(a => a.num == questNum);
        }
        else
        {
            Debug.LogError("업적 파일 오류");
            return null;
        }
    }

    private string GetCharacterByNumber(int num)
    {
        TextAsset rewardJson = Resources.Load<TextAsset>("Rewards");

        if (rewardJson != null)
        {
            string jsonText = rewardJson.text;
            RewardList rewardList = JsonUtility.FromJson<RewardList>(jsonText);

            // 보상 리스트에서 해당 번호의 캐릭터 보상을 찾음
            Reward reward = rewardList.reward.Find(r => r.rewardType == "Character" && r.rewardNum == num);

            if (reward != null)
            {
                return reward.reward;
            }
            else
            {
                return "Character"; 
            }
        }
        else
        {
            return "Character";
        }
    }


    public void CompleteQuest(IQuest quest)
    {
        quest.Complete();
        ShowPopup();
    }

    public void ShowPopup()
    {
        GameObject backgroundUI = GameObject.FindGameObjectWithTag("Background");

        if (backgroundUI != null)
        {
            if (popupObject == null)
            {
                popupObject = Instantiate(popupPrefab, backgroundUI.transform);
                popupObject.transform.localPosition = new Vector3(730f, -465f, 0f);
            }
            popupObject.SetActive(true);
            StartCoroutine(ClosePopup());
        }
    }

    private IEnumerator ClosePopup()
    {
        yield return new WaitForSeconds(displayDuration);
        popupObject.SetActive(false);
    }
}
