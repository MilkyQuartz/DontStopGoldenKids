using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterList : MonoBehaviour
{
    public Button[] characterButtons;
    private PlayerManager playerManager;

    void Start()
    {
        playerManager = PlayerManager.Instance;
        InitializeCharacterButtons();
        UpdateButtonColors();
    }

    void InitializeCharacterButtons()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            string characterName = characterButtons[i].name; // 캐릭터 이름이 버튼 이름과 동일
            Image buttonImage = characterButtons[i].GetComponentInChildren<Image>();

            // 현재 사용자가 해당 캐릭터를 소유하고 있는지 확인
            if (playerManager.LoggedInUser.ownedCharacters.Contains(characterName))
            {
                characterButtons[i].onClick.AddListener(() => OnCharacterButtonClick(characterName));
                characterButtons[i].interactable = true;
                buttonImage.color = Color.white;
            }
            else
            {
                characterButtons[i].interactable = false;
                buttonImage.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }

    public void OnCharacterButtonClick(string characterName)
    {
        playerManager.SetCharacter(characterName);
        InitializeCharacterButtons();
    }

    void UpdateButtonColors()
    {
        string selectedCharacter = playerManager.LoggedInUser.selectedCharacter;

        for (int i = 0; i < characterButtons.Length; i++)
        {
            Image buttonImage = characterButtons[i].GetComponentInChildren<Image>();

            if (characterButtons[i].name == selectedCharacter)
            {
                buttonImage.color = Color.green; // 선택된 캐릭터는 초록색
            }
            else
            {
                buttonImage.color = Color.white; // 다른 캐릭터는 흰색
            }
        }
    }
}
