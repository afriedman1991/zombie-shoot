using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public Animator animator;

    public float UpdateSpeed = 0.1f;

    private const string isWalking = "isWalking";

    private Coroutine FollowCoroutine;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(isWalking, enemy.velocity.magnitude > 0.01f);

    }

    public void StartChasing()
    {
        if (FollowCoroutine == null)
        {
            FollowCoroutine = StartCoroutine(FollowTarget());
        }
        else
        {
            Debug.LogWarning("Called StartChasing on Enemy that is already chasing! This is likely a bug in some calling class!");
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(UpdateSpeed);

        while (enabled)
        {
            enemy.SetDestination(player.position);

            yield return wait;
        }
    }
}
