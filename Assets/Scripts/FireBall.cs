using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float _speed;
    public Vector2 Direction;

    // Start is called before the first frame update
    void Start()
    {
        _speed = GameManager.FireBallSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Direction * _speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("FireBall"))
            return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Injure(10);
        }
        Destroy(gameObject);
    }
}
