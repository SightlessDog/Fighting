using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
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
    public StaminaBar staminaBar;

    private int requiredPunchStamina = 3;
    private int requiredHeavyPunchStamina = 1;
    private int requiredFeetKickStamina = 1;
    private int requiredJumpStamina = 2;
    private int requiredBlockStamina = 2;

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
        if (value.started && (staminaBar.currentStamina > requiredFeetKickStamina))
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
        if (value.started && staminaBar.currentStamina > requiredBlockStamina)
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
        if (value.started && staminaBar.currentStamina > requiredJumpStamina)
        {
            animator.SetBool("Jump", true);
        }
    }

    public void punchAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (heavyPunch && (staminaBar.currentStamina > requiredHeavyPunchStamina))
            {
                animator.SetBool("HeavyPunch", heavyPunch);
            }
            else if (feetKick && (staminaBar.currentStamina > requiredFeetKickStamina))
            {
                animator.SetBool("FeetKick", feetKick);
            }
            else
            {
                if (staminaBar.currentStamina > requiredJumpStamina)
                {
                    punch = true;
                    animator.SetBool("Punch", punch);

                }
            }
        }
    }

    public void updateStamina()
    {
        if (punch) staminaBar.UseStamina(requiredPunchStamina);
        else if (heavyPunch) staminaBar.UseStamina(requiredHeavyPunchStamina);
        else if (feetKick) staminaBar.UseStamina(requiredFeetKickStamina);
        else if (animator.GetBool("Jump")) staminaBar.UseStamina(requiredJumpStamina);
        else if (block) staminaBar.UseStamina(requiredBlockStamina);
    }

    public void insultAction(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            playSound();
        }
    }
}