using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float FallingTime;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        StartCoroutine(StartFalling());
    }

    private IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(FallingTime);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private void Update()
    {
        if (transform.position.y < -10)
            Destroy(gameObject);
    }
}
