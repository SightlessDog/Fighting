using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float mag;
    [SerializeField] private bool punch;
    [SerializeField] private bool heavyPunch;
    [SerializeField] private bool feetKick;
    [SerializeField] private bool block;
    [SerializeField] private bool run;
    [SerializeField] private int health = 100;
    public CharacterController characterController;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Moving 
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            run = true;
            heavyPunch = true;
        } else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            run = false;
            heavyPunch = false;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            feetKick = true;
        } else if (Input.GetKeyUp(KeyCode.I))
        {
            feetKick = false;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            mag += 0.2f;
            animator.SetFloat("Mag", mag);
            animator.SetBool("Sprint", run);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }
        // Punching and Blocking
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (heavyPunch) 
            {
                animator.SetBool("HeavyPunch", heavyPunch);         
            }
            else if (feetKick)
            {
                animator.SetBool("FeetKick", feetKick);
            }
            else
            {
                punch = true;  
                animator.SetBool("Punch", punch);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            block = true;
            animator.SetBool("Block", block);
        } else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            block = false;
            animator.SetBool("Block", block);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Jump", true);
        }
        
        if (!Input.anyKey)
        {
            mag = 0;
            punch = false;
            animator.SetFloat("Mag", mag);
            animator.SetBool("Punch", punch);
            animator.SetBool("HeavyPunch", heavyPunch);
            animator.SetBool("Jump", false);
            animator.SetBool("FeetKick", feetKick);
            animator.SetBool("Jump", false); 
        }
    }
}
