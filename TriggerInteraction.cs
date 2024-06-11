using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum Interaction
{
    Jumping,
    Teleporting,
    GameOver,
    GameClear,
    Damagable
}
public class TriggerInteraction : MonoBehaviour
{
    public Interaction interaction;
    [Header("Jumping")]
    public int jumpPower = 15;
    [Header("Teleporting")]
    public Vector3 destination;
    public bool isUnicorn;

    private GameObject monster;

    private MainGameSceneManager gameSceneManager;

    PlayerAnimationController playerAnimationController;

    private void Awake()
    {
        gameSceneManager = GameObject.Find("MainGameSceneManager").GetComponent<MainGameSceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        monster = GameObject.FindWithTag("Monster");
        if (isUnicorn&& other.CompareTag("Monster"))
        {
            other.transform.position = destination;
            monster.GetComponent<MonsterController>().StopFinding();
            monster.GetComponent<MonsterController>().StartFinding(GameObject.FindWithTag("Player"));
            return;
        }
        else if (!other.CompareTag("Player")) return;
        switch (interaction)
        {
            case Interaction.Jumping:
                other.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                playerAnimationController = other.GetComponent<PlayerAnimationController>();
                playerAnimationController.Jump();
                break;
            case Interaction.Teleporting:
                other.transform.position = destination;
                other.transform.rotation = Quaternion.identity;
                if (isUnicorn)
                {
                    monster.GetComponent<MonsterController>().StopFinding();
                    monster.GetComponent<MonsterController>().StartFinding(gameObject);
                }
                break;
            case Interaction.GameOver:
                gameSceneManager.GameOver();
                break;
            case Interaction.GameClear:
                gameSceneManager.GameClear();
                IncreaseClearCount();
                break;
            case Interaction.Damagable:
                playerAnimationController = other.GetComponent<PlayerAnimationController>();
                playerAnimationController.Hit();
                monster.GetComponent<MonsterController>().MinusDistance();
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        switch (interaction)
        {
            case Interaction.Damagable:
                playerAnimationController.HitEnd();
                break;
        }
    }

    private void IncreaseClearCount()
    {
        int questNum = 2; // ù��° Ŭ���� ����Ʈ ��ȣ
        IQuest clearCountQuest = new ClearCountQuest(questNum); // ClearCountQuest Ŭ���� ����
        QuestManager.Instance.CompleteQuest(clearCountQuest);
    }

}
