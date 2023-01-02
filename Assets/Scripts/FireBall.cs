using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private float _speed;
    public Vector2 Direction;
    private int _damage;
    // Start is called before the first frame update
    void Start()
    {
        _speed = GameManager.FireBallSpeed;
        _damage = GameManager.FireBallDamage;
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Direction * _speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("FireBall") || other.gameObject.CompareTag("AttackChecker"))
            return;
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Injure(_damage);
        }
        Debug.Log("other.gameObject" + other.gameObject.tag, other.gameObject);
        Destroy(gameObject);
    }
}
