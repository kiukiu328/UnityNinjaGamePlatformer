using UnityEngine;

public class FireBall : MonoBehaviour
{
    public Vector2 Direction;
    private float _speed;
    private int _damage;

    void Start()
    {
        _speed = GameManager.FireBallSpeed;
        _damage = GameManager.FireBallDamage;
        GetComponent<AudioSource>().Play();
    }

    // fire ball flying
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = Direction * _speed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if touch Player or FireBall or AttackChecker do nothing
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("FireBall") || other.gameObject.CompareTag("AttackChecker"))
            return;
        // if it is enemy take damage
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Injure(_damage);
        }
        
        Destroy(gameObject);
    }
}
