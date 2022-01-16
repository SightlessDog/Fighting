using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float mag;
    [SerializeField] private bool punch; 
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            mag += 0.2f;
            animator.SetFloat("Mag", mag);
        }

        if (Input.GetMouseButtonDown(0))
        {
            punch = true;
            animator.SetBool("Punch", punch);
        }

        if (!Input.anyKey)
        {
            mag = 0;
            punch = false;
            animator.SetFloat("Mag", mag);
            animator.SetBool("Punch", punch);
        }
    }
}
