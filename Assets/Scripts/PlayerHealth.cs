using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public HealthBar healthBar;

    Animator animator;
    Player player;
    public AudioSource audioSource;
    public AudioClip manHurting;
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponent<Player>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (player && player.hit)
        {
            currentHealth -= 10;
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth < 50)
        {
            audioSource.clip = manHurting;
            audioSource.Play();
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }

        animator.SetInteger("Health", currentHealth);
    }
}
