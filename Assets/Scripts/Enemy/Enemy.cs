using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth = 100;
    public int CurrentHealth;
    public float AttackCoolDown;
    public float MovingSpeed;
    public float TrackPlayerRadius;
    public GameObject FilpObj;
    public HealthBar HealthBar;


    protected Vector2 _facing = Vector2.right;
    protected bool _stopMoving;
    protected float _nextAttack;
    protected float _attackDistance = 1.1f;
    protected float _attackW = 0.2f;
    protected Rigidbody2D _rb;
    protected EdgeCollider2D _edgeCollider;


    protected void Init()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
        _rb = GetComponent<Rigidbody2D>();
        if (FilpObj != null)
        {

            _edgeCollider = FilpObj.GetComponent<EdgeCollider2D>();
        }

    }

    private void Start()
    {
        Init();
    }

    protected IEnumerator Attack()
    {
        if (_nextAttack > Time.time)
            yield break;
        _nextAttack = Time.time + AttackCoolDown;
        GetComponent<Animator>().SetTrigger("Attack");

        _stopMoving = true;
        yield return new WaitForSeconds(0.5f);
        Collider2D[] results = new Collider2D[10];
        _edgeCollider.OverlapCollider(new ContactFilter2D(), results);

        foreach (Collider2D collider in results)
        {
            if (collider == null)
                break;
            if (!collider.CompareTag("Player"))
                continue;

            StartCoroutine(collider.GetComponent<PlayerControl>().Injure());
        }
        _stopMoving = false;
    }

    protected virtual bool TrackPlayer()
    {
        if (_stopMoving)
            return true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, TrackPlayerRadius);
        if (colliders.Length <= 0)
            return false;

        foreach (Collider2D collider in colliders)
        {
            if (!collider.CompareTag("Player"))
                continue;

            float distance = collider.transform.position.x - transform.position.x;
            Vector2 direction;

            Flip(distance);
            direction = (distance > 0) ? Vector2.right : Vector2.left;

            if (Vector2.Distance(collider.transform.position, transform.position) < _attackDistance)
            {
                StartCoroutine(Attack());
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                return true;
            }
            _rb.velocity = new Vector2(direction.x * MovingSpeed, _rb.velocity.y);
            _rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            return true;
        }
        return false;
    }
    public virtual void Injure(int damage)
    {
        GetComponent<Animator>().SetTrigger("Injure");
        CurrentHealth -= damage;
        HealthBar.SetHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Debug.Log("Die:" + name);
            Destroy(gameObject);
        }
    }

    protected void Flip(float inputX)
    {
        if (!((inputX > 0 && _facing != Vector2.right) || (inputX < 0 && _facing != Vector2.left)))
            return;
        _facing = (inputX > 0) ? Vector2.right : Vector2.left;
        Vector3 theScale = FilpObj.transform.localScale;
        theScale.x *= -1;
        FilpObj.transform.localScale = theScale;
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TrackPlayerRadius);
    }
}
