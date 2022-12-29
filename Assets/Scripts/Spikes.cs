using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))

            other.gameObject.GetComponent<Rigidbody2D>().sharedMaterial.bounciness = 1;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<Rigidbody2D>().sharedMaterial.bounciness = 0;
    }
}
