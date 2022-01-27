using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public bool EnemyHit;

    public bool PlayerHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().CompareTag("enemy"))
        {
            Debug.Log("Enemy Hitting");
            EnemyHit = true;
        }
        else
        {
            EnemyHit = false;
        }
        if (col.GetComponent<Collider>().CompareTag("Player"))
        {
            Debug.Log("Player hitting");
            PlayerHit = true;
        }
        else
        {
            PlayerHit = false;
        }
    }
}
