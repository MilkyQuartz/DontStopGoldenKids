using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ȸ������ �� Json���Ͽ� ������ ������ Ŭ����
// ����� Json ��δ� �����>AppData>LocalLow>(�� �ٸ������� �� �̰��Դϴ�..)DefaultCompany> Don'tStopGoldenKids
// Ŭ������ ����ִ��� ��Ծ ��ƵѰ� �ϳ� ��������ϴ�.. ����

[System.Serializable]
public class Reward
{
    public string rewardType;
    public int rewardNum;
    public string reward;
}

[System.Serializable]
public class RewardList
{
    public List<Reward> reward;
}

[System.Serializable]
public class User
{
    public string id;
    public string password;
    public string nickname;
    public List<int> achievements;
    public string currentTitle;
    public string selectedCharacter;
    public List<string> ownedCharacters;
    public int clearCount;
}

[System.Serializable]
public class UserList
{
    public List<User> users;
}

[System.Serializable]
public class Achievement
{
    public int num;
    public string name;
    public string description;
    public string rewardType;
    public int rewardNum;
}

[System.Serializable]
public class AchievementList
{
    public List<Achievement> achievement;
}


public class JsonClass : MonoBehaviour
{

}
