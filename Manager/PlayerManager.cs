using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public User LoggedInUser { get; private set; }

    public GameObject selectedCharacterPrefab;
    public GameObject currentCharacterInstance;

    public Transform characterParentTransform;

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

    // 유저 정보 저장
    public void SetLoggedInUser(User user)
    {
        LoggedInUser = user;
        SetCharacter(user.selectedCharacter);
    }

    // 캐릭터 정보
    public void SetCharacter(string characterName)
    {
        LoggedInUser.selectedCharacter = characterName;
        string prefabPath = $"CharacterPrefabs/{characterName}";
        selectedCharacterPrefab = Resources.Load<GameObject>(prefabPath);
        if (selectedCharacterPrefab != null)
        {
            if (currentCharacterInstance != null)
            {
                Destroy(currentCharacterInstance);
            }
            if (characterParentTransform != null)
            {
                currentCharacterInstance = Instantiate(selectedCharacterPrefab, characterParentTransform);
                currentCharacterInstance.transform.localPosition = Vector3.zero; 
                DontDestroyOnLoad(currentCharacterInstance); 
            }
        }
        SaveUserData();
    }


    //  유저 정보 저장
    public void SaveUserData() 
    {
        string userFilePath = Path.Combine(Application.persistentDataPath, "PlayerInfo.json");
        if (File.Exists(userFilePath))
        {
            string json = File.ReadAllText(userFilePath);
            UserList userList = JsonUtility.FromJson<UserList>(json);

            User currentUser = userList.users.Find(u => u.id == LoggedInUser.id);
            if (currentUser != null)
            {
                currentUser.achievements = LoggedInUser.achievements;
                currentUser.currentTitle = LoggedInUser.currentTitle;
                currentUser.ownedCharacters = LoggedInUser.ownedCharacters;

                string updatedJson = JsonUtility.ToJson(userList, true);
                File.WriteAllText(userFilePath, updatedJson);
            }
        }
    }

    public void CreateCharacterInGame(Transform parentTransform)
    {
        characterParentTransform = parentTransform;
        if (LoggedInUser != null)
        {
            SetCharacter(LoggedInUser.selectedCharacter);
        }

        parentTransform.gameObject.SetActive(true);
    }
}
