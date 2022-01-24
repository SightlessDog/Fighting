using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    public Detector detector;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detector.hit)
        {
            animator.SetBool("killed", true);
        }
        else
        {
            animator.SetBool("killed", false);
        }
    }
}
