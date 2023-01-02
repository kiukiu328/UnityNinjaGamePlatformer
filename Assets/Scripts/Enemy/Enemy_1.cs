using UnityEngine;

public class Enemy_1 : Enemy
{
    public LayerMask GroundLayerMask;


    private void Start()
    {
        base.Init();
    }

    private void Update()
    {
        // if cant detect any player then just walk around
        if (!TrackPlayer())
            Walk();
    }
    // walk method only for enemy 1 to move around the platform when cant detect any player
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




}
