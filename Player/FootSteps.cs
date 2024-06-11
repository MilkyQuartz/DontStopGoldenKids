using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footStepClips;
    private AudioSource audioSource;
    private Rigidbody rigidbody;
    public float footStepRate;
    private float footStepTime;

    private bool isRun;

    private void Start()
    {
        // Ä³½Ì
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isRun)
        {
            if(!audioSource.isPlaying)
            {
                NolmalFootSteps();
            }
        }
    }

    private void NolmalFootSteps()
    {
        if (Mathf.Abs(rigidbody.velocity.y) < 0.1f)
        {
            if (Time.time - footStepTime > footStepRate)
            {
                footStepTime = Time.time;
                audioSource.PlayOneShot(footStepClips[Random.Range(0, footStepClips.Length)]);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isRun = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isRun = false;
        }
    }
}
