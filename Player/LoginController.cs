using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public GameObject loginMenu;

    [Header("Field")]
    public TMP_InputField idField;
    public TMP_InputField pwField;

    [Header("WarningText")]
    public TextMeshProUGUI idWarningTxt;
    public TextMeshProUGUI pwWarningTxt;

    [Header("Button")]
    public Button loginBtn;

    private string userInfoFilePath;

    void Start()
    {
        loginMenu.SetActive(false);

        userInfoFilePath = Path.Combine(Application.persistentDataPath, "PlayerInfo.json");

        idWarningTxt.gameObject.SetActive(false);
        pwWarningTxt.gameObject.SetActive(false);
        loginBtn.onClick.AddListener(OnLoginBtnClick);

    }

    void Update()
    {
        // 아무 키나 눌렀을 때 오브젝트를 활성화
        if (Input.anyKeyDown)
        {
            if (loginMenu != null)
            {
                loginMenu.SetActive(true);
            }
        }
    }

    void OnLoginBtnClick()
    {
        string id = idField.text;
        string password = pwField.text;

        bool isIdValid = false;
        bool isPasswordValid = false;
        User loggedInUser = null;

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
                        isIdValid = true;
                        if (user.password == password)
                        {
                            isPasswordValid = true;
                            loggedInUser = user;
                            break;
                        }
                    }
                }
            }
        }

        idWarningTxt.gameObject.SetActive(!isIdValid);
        pwWarningTxt.gameObject.SetActive(!isPasswordValid);

        if (isIdValid && isPasswordValid)
        {
            PlayerManager.Instance.SetLoggedInUser(loggedInUser);
            GameManager.Instance.AsyncLoadNextScene(SceneName.LobbyScene);
        }
    }
}

