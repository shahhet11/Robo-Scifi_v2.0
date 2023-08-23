using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage;
    public float attackTime;
    public bool playerInRange;

    private float timerT;
    private PlayerHealth PlayerHealth;
    private Vector3 playerpos;

    void Start()
    {
        attackTime = GameManager.Instance.timeBetweenAttackAI;
        PlayerHealth = GameManager.Instance.PlayerHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerpos = other.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        timerT += Time.unscaledDeltaTime;

        if (timerT >= attackTime && playerInRange)
        {
            Attack();    
        }
    }

    private void Attack()
    {
        timerT = 0f;

        if(PlayerHealth.HealthPoints > 0)
            PlayerHealth.UpdateHealth(attackDamage, playerpos);
    }
}
