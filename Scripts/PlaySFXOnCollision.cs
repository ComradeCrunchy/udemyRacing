using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFXOnCollision : MonoBehaviour
{
    private AudioSource crashSFX;

    private void Start()
    {
        crashSFX = GetComponentInChildren<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Structor") || other.collider.CompareTag("Opponent"))
        {
            crashSFX.pitch = Random.Range(.03f, 3f);
            crashSFX.Play();
        }
    }
}
