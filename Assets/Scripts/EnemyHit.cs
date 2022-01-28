using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    Animator animator;
    public Detector detector;
    public AudioClip manHit;
    public AudioSource audioSource;


    public bool hit;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        detector = gameObject.GetComponent<Detector>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detector.PlayerHit)
        {
            hit = true;
            animator.SetBool("hit", true);
            audioSource.clip = manHit;
            audioSource.Play();
        }
        else
        {
            hit = false;
            animator.SetBool("hit", false);
        }
    }
}