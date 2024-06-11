using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollisionPlay : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip audio;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = audio;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionOrTrigger(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        CollisionOrTrigger(other.gameObject);
    }

    private void CollisionOrTrigger(GameObject obj)
    {
        if (obj.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogError("AudioSource component is missing.");
            }
        }
    }
}
