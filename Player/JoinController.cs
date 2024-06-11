using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;


public class JoinController : MonoBehaviour
{
    public GameObject joinMenu;

    [Header("Field")]
    public TMP_InputField idField;
    public TMP_InputField pwField;
    public TMP_InputField nameField;

    [Header("WarningText")]
    public TextMeshProUGUI idWarningText;
    public TextMeshProUGUI pwWarningText;
    public TextMeshProUGUI nameWarningText;

    [Header("Button")]
    public Button joinBtn;
    public Button joinCloseBtn;

    [Header("LoginController")]
    public GameObject loginMenu;
    public TMP_InputField loginIdField;
    public TMP_InputField loginPWField;
    public TextMeshProUGUI loginIdWarningText;
    public TextMeshProUGUI loginPWWarningText;
    

    private string userInfoFilePath;
    private AchievementList achievementList;

    void Start()
    {
        // �α��� ��Ʈ�ѷ� Ŭ���� �����ϱ�
        GameObject LoginControllerObject = GameObject.Find("LoginController");

        if (LoginControllerObject != null)
        {
            LoginController LoginController = LoginControllerObject.GetComponent<LoginController>();

            if (LoginController != null)
            {
                loginMenu = LoginController.loginMenu;
                loginIdField = LoginController.idField;
                loginPWField = LoginController.pwField;
                loginIdWarningText = LoginController.idWarningTxt;
                loginPWWarningText = LoginController.pwWarningTxt;
            }

        }

        userInfoFilePath = Path.Combine(Application.persistentDataPath, "PlayerInfo.json");

        if (!File.Exists(userInfoFilePath))
        {
            UserList emptyUserList = new UserList { users = new List<User>() };
            string emptyJson = JsonUtility.ToJson(emptyUserList, true);
            File.WriteAllText(userInfoFilePath, emptyJson);
        }
        LoadAchievements();
        joinBtn.onClick.AddListener(OnJoinBtnClick);
        joinCloseBtn.onClick.AddListener(OnJoinCloseBtnClick);

    }

    // ���� �ε�
    void LoadAchievements()
    {
        string achievementFilePath = Path.Combine(Application.persistentDataPath, "Achievements.json");
        if (File.Exists(achievementFilePath))
        {
            string json = File.ReadAllText(achievementFilePath);
            achievementList = JsonUtility.FromJson<AchievementList>(json);
        }
        else
        {
            achievementList = new AchievementList();
        }
    }

    // ȸ������ ��ư ������ �ʵ�� ���� �ʱ�ȭ�ǰ�
    public void OnJoinSetActive()
    {
        loginMenu.SetActive(false);
        joinMenu.SetActive(true);
        idField.text = "";
        pwField.text = "";
        nameField.text = "";
        loginIdField.text = "";
        loginPWField.text = "";
        loginIdWarningText.gameObject.SetActive(false);
        loginPWWarningText.gameObject.SetActive(false);
        idWarningText.gameObject.SetActive(false);
        pwWarningText.gameObject.SetActive(false);
        nameWarningText.gameObject.SetActive(false);
    }

    void OnJoinBtnClick()
    {
        string id = idField.text;
        string password = pwField.text;
        string nickname = nameField.text;

        bool isIdValid = !IdCheck(id);
        bool isPasswordValid = PasswordCheck(password);
        bool isNameValid = NameCheck(nickname);

        idWarningText.gameObject.SetActive(!isIdValid);
        pwWarningText.gameObject.SetActive(!isPasswordValid);
        nameWarningText.gameObject.SetActive(!isNameValid);

        if (isIdValid && isPasswordValid && isNameValid)
        {
            SaveUser(id, password, nickname);
            joinMenu.SetActive(false);
            loginMenu.SetActive(true);
        }
    }

    // ȸ������ ��� ��ư������ �ʵ�� ��� �ʱ�ȭ
    void OnJoinCloseBtnClick()
    {
        joinMenu.SetActive(false);
        loginMenu.SetActive(true);
        loginIdField.text = "";
        loginPWField.text = "";
        loginIdWarningText.gameObject.SetActive(false);
        loginPWWarningText.gameObject.SetActive(false);
    }

    // ��ȿ�� �˻�
    bool IdCheck(string id)
    {
        if (File.Exists(userInfoFilePath))
        {
            string json = File.ReadAllText(userInfoFilePath);
            UserList userList = JsonUtility.FromJson<UserList>(json);
            if (userList != null && userList.users != null)
            {
                foreach (var user in userList.users)
                {
                    if (user.id == id)
                    {
                        idWarningText.text = "�̹� �����ϴ� ���̵��Դϴ�";
                        return true;
                    }
                }
            }
        }

        if (id.Length < 4 || id.Length > 12)
        {
            idWarningText.text = "���̵�� 4~12�� ���̷� �Է��ϼ���.";
            return true;
        }

        return false;
    }

    bool PasswordCheck(string password)
    {
        return password.Length >= 4 && password.Length <= 12;
    }

    bool NameCheck(string name)
    {
        return name.Length >= 2 && name.Length <= 6;
    }

    // Json�� User �����ϱ�
    void SaveUser(string id, string password, string name)
    {
        if (string.IsNullOrEmpty(userInfoFilePath))
        {
            return;
        }

        User newUser = new User
        {
            id = id,
            password = password,
            nickname = name,
            currentTitle = "���� ������",
            achievements = new List<int> { 0 },
            selectedCharacter = "Treestor_Small",
            ownedCharacters = new List<string> { "Treestor_Small" },
            clearCount = 0
        };
        UserList userList;

        if (File.Exists(userInfoFilePath))
        {
            string json = File.ReadAllText(userInfoFilePath);

            if (string.IsNullOrEmpty(json))
            {
                userList = new UserList { users = new List<User>() };
            }
            else
            {
                userList = JsonUtility.FromJson<UserList>(json);
            }
        }
        else
        {
            userList = new UserList { users = new List<User>() };
        }

        if (userList.users == null)
        {
            userList.users = new List<User>();
        }

        userList.users.Add(newUser);
        string newJson = JsonUtility.ToJson(userList, true);
        File.WriteAllText(userInfoFilePath, newJson);
    }
}
