using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float mag;
    [SerializeField] private bool punch;
    [SerializeField] private bool block;
    public CharacterController characterController;
    public Transform cam;
    public float speed = 6f;
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
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     mag += 0.2f;
        //     animator.SetFloat("Mag", mag);
        // }
        
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
        }

        if (Input.GetMouseButtonDown(0))
        {
            punch = true;
            animator.SetBool("Punch", punch);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            block = true;
            animator.SetBool("Block", block);
        } else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            block = false;
            animator.SetBool("Block", block);
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
