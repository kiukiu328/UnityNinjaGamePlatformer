using UnityEngine;
// for scence 3 move the player
public class TouchThenMove : MonoBehaviour
{
    public GameObject MoveTo;
    public GameObject Canvas;

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if not player then do noting
        if (!other.gameObject.CompareTag("Player"))
            return;

        // else move player and the camera
        other.transform.position = MoveTo.transform.position;
        Camera.main.transform.position = MoveTo.transform.position;
        Camera.main.GetComponent<MainCamera>().ChangeCanvas(Canvas);
    }
}
