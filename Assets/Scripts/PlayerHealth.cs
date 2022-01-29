using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 1000;
    public Image healthBar;
    Animator animator;
    Player player;
    public AudioSource audioSource;
    public AudioClip manHurting;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponent<Player>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (player && player.hit)
        {
            health -= 10;
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
