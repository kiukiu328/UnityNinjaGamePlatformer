using UnityEngine;

public class Enemy_2 : Enemy
{
    private float _flipTime;
    private void Start()
    {
        base.Init();
        _facing = Vector2.left;
        _attackDistance = 2f;

    }

    private void Update()
    {
        // if cant detect any player then just fly around
        if (!TrackPlayer())
            Fly();
    }
    // every 5 sec flip to another direction
    private void Fly()
    {
        if (_flipTime < Time.time)
        {
            _flipTime = Time.time + 5;
            Flip(_facing.x * -1);
        }
        _rb.linearVelocity = new Vector2(_facing.x * MovingSpeed, _rb.linearVelocity.y);

    }
    // override the TrackPlayer() to adding the fly check function
    protected override bool TrackPlayer()
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
            Vector2 moveTo = collider.transform.position - transform.position;
            float distance = collider.transform.position.x - transform.position.x;
            Vector2 direction;

            Flip(distance);
            direction = (distance > 0) ? Vector2.right : Vector2.left;

            if (Vector2.Distance(collider.transform.position, transform.position) < _attackDistance)
            {
                //GetComponent<CircleCollider2D>().isTrigger = false;
                StartCoroutine(Attack());
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                return true;
            }
            _rb.linearVelocity = moveTo * MovingSpeed;
            _rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            return true;
        }
       // GetComponent<CircleCollider2D>().isTrigger = true;
        return false;
    }


}
