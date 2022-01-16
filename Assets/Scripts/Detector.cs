using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public bool hit;
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
            Debug.Log("Hitting");
            hit = true;
        }
        else
        {
            hit = false;
        }
    }
}
