using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    public float AttackCoolDown;
    public float TrackPlayerRadius;
    public float MovingSpeed;
    public LayerMask GroundLayerMask;

    private float _nextAttack;
    private Rigidbody2D _rb;
    private bool _stopMoving;

    private void Start()
    {
        base.Init();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!TrackPlayer())
            Walk();
    }

    private void Walk()
    {
        bool frontIsGround =
            Physics2D.OverlapPoint(
                (Vector2)transform.position + _facing + Vector2.down * 1.1f,
                GroundLayerMask
            ) != null;
        if (!frontIsGround)
        {
            Flip(_facing.x * -1);
        }
        _rb.velocity = new Vector2(_facing.x * MovingSpeed, _rb.velocity.y);
    }

    private IEnumerator Attack()
    {
        if (_nextAttack > Time.time)
            yield break;
        _nextAttack = Time.time + AttackCoolDown;
        GetComponent<Animator>().SetTrigger("Attack");

        _stopMoving = true;
        yield return new WaitForSeconds(0.1f);
        Collider2D[] results = new Collider2D[10];
        GetComponent<EdgeCollider2D>().OverlapCollider(new ContactFilter2D(), results);

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

    private bool TrackPlayer()
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

            if (Vector2.Distance(collider.transform.position, transform.position) < 1.1f)
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

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TrackPlayerRadius);
    }
}
