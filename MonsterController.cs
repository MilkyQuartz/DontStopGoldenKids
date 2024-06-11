using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public float perDistance = 15;
    public GameObject target;
    private GameObject player;
    private float distance;
    private NavMeshAgent agent;
    private Coroutine coroutine;
    private float speed = 5;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        distance = 5;
    }
    private void Start()
    {
        transform.position = new Vector3(120, 0, -125);
        agent.enabled = true;
        StartFinding(player);
    }
    private void FixedUpdate()
    {
        float curDistance = Vector3.Distance(transform.position, player.transform.position);
        if(curDistance < distance * perDistance - 1)
        {
            agent.speed = speed * 0.8f;
        }
        else if(curDistance > distance * perDistance+1)
        {
            agent.speed = speed * 5f;
        }
        else
        {
            agent.speed = speed;
        }
    }
    private IEnumerator SetDestination(GameObject target)
    {
        Debug.Log("StartCoroutine");
        while(true)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(target.transform.position, path);
            agent.SetPath(path);
            yield return new WaitForSeconds(1f);
        }
    }
    public void StartFinding(GameObject _target)
    {
        Debug.Log("StartFinding");
        agent.isStopped = false;
        target = _target;
        coroutine = StartCoroutine(SetDestination(_target));
    }
    public void StopFinding()
    {
        Debug.Log("StopFinding");
        agent.isStopped = true;
        StopCoroutine(coroutine);
    }
    public void MinusDistance()
    {
        distance--;
    }
}
