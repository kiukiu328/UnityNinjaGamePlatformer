using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchThenMove : MonoBehaviour
{
    public Vector2 MoveTo;
    public GameObject Canvas;

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if not player then do noting
        if (!other.gameObject.CompareTag("Player"))
            return;

        // else move player and the camera
        other.transform.position = MoveTo;
        Camera.main.transform.position = MoveTo;
        Camera.main.GetComponent<MainCamera>().ChangeCanvas(Canvas);
    }
}
