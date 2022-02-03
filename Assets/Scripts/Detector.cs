using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public bool EnemyHit;
    public bool PlayerHit;
    public GameObject playerObject; 
    public GameObject enemyObject;
    private Move player;
    private Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<Move>();
        enemy = enemyObject.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().CompareTag("enemy") && (enemy.punch || enemy.heavyPunch || enemy.feetKick) && !player.block)
        {
            EnemyHit = true;
        }
        else
        {
            EnemyHit = false;
        }

        if (col.GetComponent<Collider>().CompareTag("Player") && (player.punch || player.heavyPunch || player.feetKick) && !enemy.block)
        {
            PlayerHit = true;
        }
        else
        {
            PlayerHit = false;
        }
    }
}
