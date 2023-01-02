using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float FallingTime;
    // check when the player stand on
    private void OnCollisionEnter2D(Collision2D other)
    {
        // if fall out side delect this platform
        if (other.gameObject.CompareTag("OutSide"))
            Destroy(gameObject);
        // if not player do nothing

        if (!other.gameObject.CompareTag("Player"))
            return;
        //else start count down
        StartCoroutine(StartFalling());
        

    }

    private IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(FallingTime);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }


}
