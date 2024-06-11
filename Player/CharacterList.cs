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
            string characterName = characterButtons[i].name; // ĳ���� �̸��� ��ư �̸��� ����
            Image buttonImage = characterButtons[i].GetComponentInChildren<Image>();

            // ���� ����ڰ� �ش� ĳ���͸� �����ϰ� �ִ��� Ȯ��
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
                buttonImage.color = Color.green; // ���õ� ĳ���ʹ� �ʷϻ�
            }
            else
            {
                buttonImage.color = Color.white; // �ٸ� ĳ���ʹ� ���
            }
        }
    }
}
