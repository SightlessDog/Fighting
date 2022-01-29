using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 1000;
    public Image healthBar;
    Animator animator;
    EnemyHit enemyHit;
    public AudioClip manHurting;
    public AudioSource audioSource;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        enemyHit = gameObject.GetComponent<EnemyHit>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (enemyHit && enemyHit.hit)
        {
            health -= 10;
            new WaitForSeconds(1);
        }

        if (health < 50)
        {
            audioSource.clip = manHurting;
            audioSource.Play();
        }
        if (health <= 0)
        {
            health = 0;
        }

        animator.SetInteger("Health", health);
    }

    public void damage(int damage)
    {
        health -= damage;
        healthBar.fillAmount = (float)health / 100;
        Debug.Log("Health: " + (float)health / 100);
    }
}