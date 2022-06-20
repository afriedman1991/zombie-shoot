using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public EnemyFollow movement;
    public GameObject player;
    public GameObject[] bloodAnimations;
    public float health;
    public float pointsToGive;
    public float TimeDelayToReactivate;
    public float xOffset;
    public float yOffset;
    public float zOffset;

    private void Awake()
    {
        movement.StartChasing();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            //player.GetComponent<Player>().points += pointsToGive;
            //pointsToGive = 0;
            StartCoroutine(AdjustCapsuleCollider());
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        print("Enemy " + this.gameObject.name + " has died");
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    public IEnumerator AdjustCapsuleCollider()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<CapsuleCollider>().height = .05f;
        GetComponent<CapsuleCollider>().radius = .42f;
    }
}
