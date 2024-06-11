using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 회원가입 시 Json파일에 정보를 저장할 클래스
// 저장된 Json 경로는 사용자>AppData>LocalLow>(다 다르겠지만 전 이거입니다..)DefaultCompany> Don'tStopGoldenKids
// 클래스들 어디있는지 까먹어서 모아둘겸 하나 만들었습니다.. ㅎㅎ

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
