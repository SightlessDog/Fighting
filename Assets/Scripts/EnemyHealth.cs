using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public HealthBar healthBar;
    Animator animator;
    EnemyHit enemyHit;
    public AudioClip manHurting;
    public AudioSource audioSource;
    public GameObject endgameUI;
    public Text winnerText;


    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = gameObject.GetComponent<Animator>();
        enemyHit = gameObject.GetComponent<EnemyHit>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (enemyHit && enemyHit.hit)
        {
            currentHealth -= 10;
            healthBar.SetHealth(currentHealth);
            //new WaitForSeconds(1);
        }

        if (currentHealth < 100)
        {
            audioSource.clip = manHurting;
            audioSource.Play();
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            endGame();
        }

        animator.SetInteger("Health", currentHealth);
    }

    void endGame()
    {
        endgameUI.SetActive(true);
        winnerText.text = "Not Beauty";
    }
}