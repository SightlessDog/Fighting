using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float mag;
    [SerializeField] public bool punch;
    [SerializeField] public bool heavyPunch;
    [SerializeField] public bool feetKick;
    [SerializeField] public bool block;
    [SerializeField] private bool run;
    public CharacterController characterController;
    public Transform cam;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    Animator animator;
    public AudioClip[] audioSources;
    public AudioSource audioSource;
    public AudioClip manPunch;
    public PlayerInput playerInput;
    private InputAction moveAction;

    public void playSound()
    {
        audioSource.clip = audioSources[Random.Range(0, audioSources.Length)];
        audioSource.Play();
    }

    void Awake()
    {
        moveAction = playerInput.actions["Movements"];
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        movementAction();
        updateStamina();
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

    public void movementAction()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y);
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            mag += 0.2f;
            animator.SetFloat("Mag", mag);
            animator.SetBool("Sprint", run);
        }
    }

    public void feetKickAction(InputAction.CallbackContext value)
    {
        if (value.started && (StaminaBar.instance.currentStamina > 5))
        {
            feetKick = true;
        }
        else
        {
            feetKick = false;
        }
    }

    public void blockAction(InputAction.CallbackContext value)
    {
        if (value.started && (StaminaBar.instance.currentStamina > 10))
        {
            block = true;
            animator.SetBool("Block", block);
        }
        else
        {
            block = false;
            animator.SetBool("Block", block);
        }
    }

    public void runAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            run = true;
            heavyPunch = true;
        }
        else
        {
            run = false;
            heavyPunch = false;
        }
    }

    public void jumpAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            animator.SetBool("Jump", true);
        }
    }

    public void punchAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (heavyPunch && (StaminaBar.instance.currentStamina > 20))
            {
                animator.SetBool("HeavyPunch", heavyPunch);
            }
            else if (feetKick && (StaminaBar.instance.currentStamina > 10))
            {
                animator.SetBool("FeetKick", feetKick);
            }
            else
            {
                if (StaminaBar.instance.currentStamina > 5)
                {
                    punch = true;
                    animator.SetBool("Punch", punch);

                }
            }
        }
    }

    public void updateStamina()
    {
        if (punch) StaminaBar.instance.UseStamina(5);
        else if (heavyPunch) StaminaBar.instance.UseStamina(20);
        else if (feetKick) StaminaBar.instance.UseStamina(10);
        else if (animator.GetBool("Jump")) StaminaBar.instance.UseStamina(15);
        else if (block) StaminaBar.instance.UseStamina(5);
    }

    public void insultAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            playSound();
        }
    }
}