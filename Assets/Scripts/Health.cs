using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
 [SerializeField] private int health = 100;
 public Image healthBar;
 Animator animator;

 private void Start()
 {
  animator = gameObject.GetComponent<Animator>();  
 }
 private void Update()
 {
  if (health <= 0)
  {
   health = 0; 
  }

  animator.SetInteger("Health", health); 
 }

 public void damage(int damage)
 {
  health -= damage;
  healthBar.fillAmount = (float) health / 100;
  Debug.Log("Health: " + (float) health / 100);
 }
}
