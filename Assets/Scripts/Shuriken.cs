using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public GameObject[] ShurikenObjs;
    public float _speed;

    private void Update()
    {
        ShurikenObjs[0].transform.Rotate(0, 0, _speed * Time.deltaTime * 100);
        ShurikenObjs[1].transform.Rotate(0, 0, -_speed * Time.deltaTime * 100);
        ShurikenObjs[2].transform.Rotate(0, 0, _speed * Time.deltaTime * 100);


        // move sprite towards the target location
        transform.position = new Vector2(
            transform.position.x,
            transform.position.y - Time.deltaTime * _speed
        );
        if (transform.position.y < -6)
            transform.position = new Vector2(transform.position.x, transform.position.y * -1);
    }
}
