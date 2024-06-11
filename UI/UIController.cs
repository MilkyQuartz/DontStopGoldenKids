using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject[] menu;
    public Button[] uiOpenBtn;
    public Button[] uiCloseBtn;
    public Camera subCamera;
    private PlayerManager playerManager;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerTitleText;
    private Quest quest;
    private Title title;
    public Transform characterParentTransform;
    private int settingsQuestCount = 0;
    void Start()
    {
        playerManager = PlayerManager.Instance;
        quest = FindObjectOfType<Quest>();
        title = FindObjectOfType<Title>();

        playerManager.characterParentTransform = characterParentTransform;

        // 버튼에 대한 이벤트 핸들러
        for (int i = 0; i < uiCloseBtn.Length; i++)
        {
            int index = i;
            uiOpenBtn[i].onClick.AddListener(() => OnSetActiveMenu(index));
            uiCloseBtn[i].onClick.AddListener(() => OnButtonClick(index));
        }

        if (playerManager.LoggedInUser != null)
        {
            playerNameText.text = playerManager.LoggedInUser.nickname;
            playerTitleText.text = playerManager.LoggedInUser.currentTitle;

            SpawnSelectedCharacter();
        }
    }

    public void OnButtonClick(int index)
    {
        subCamera.enabled = true;

        switch (index)
        {
            case 0:
                menu[0].SetActive(false);
                break;
            case 1:
                menu[1].SetActive(false);
                break;
            case 2:
                menu[2].SetActive(false);
                break;
        }
    }

    public void OnSetActiveMenu(int index)
    {
        subCamera.enabled = false;

        switch (index)
        {
            case 0:
                menu[0].SetActive(true);
                break;
            case 1:
                menu[1].SetActive(true);
                quest.LoadAndDisplayAchievements();
                break;
            case 2:
                menu[2].SetActive(true);
                title.DisplayTitles();

                // 환경설정 메뉴가 열릴 때 퀘스트 완료
                settingsQuestCount++;
                if (settingsQuestCount == 1)
                {
                    CompleteSettingsQuest(4);
                }
                else if(settingsQuestCount == 10)
                {
                    CompleteSettingsQuest(5);
                }
                break;
        }
    }

    public void OnGameStartClick()
    {
        GameManager.Instance.AsyncLoadNextScene(SceneName.MainGameScene);
    }

    private void SpawnSelectedCharacter()
    {
        playerManager.SetCharacter(playerManager.LoggedInUser.selectedCharacter);
    }
    private void CompleteSettingsQuest(int questNum)
    {
        IQuest quest = new SettingsQuest(questNum); 
        QuestManager.Instance.CompleteQuest(quest);
    }
}
