using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // for checking on ground
    public LayerMask GroundLayerMask;
    public GameObject FireBall;
    private Vector2 _groundCheck;
    private float JumpingCoolDown;
    private float AttackCoolDown;
    private float FireBallCoolDown;

    // private float _nextJump,
    //     _nextAttack,
    //     _nextFireBall;
    private float _runSpeed;
    private int _jumping;
    private int _jumpTimes;
    private bool _onGround;
    private bool _onMovingPlatform;
    private bool _onFreeze;

    private Dictionary<string, float> coolDownTime = new Dictionary<string, float>();
    private Dictionary<string, float> nextTime = new Dictionary<string, float>();
    private Dictionary<string, bool> state = new Dictionary<string, bool>();

    public PlayerControl()
    {
        nextTime.Add("Attack", 0);
        nextTime.Add("Jump", 0);
        nextTime.Add("FireBall", 0);
        nextTime.Add("KnockBack", 0);
        coolDownTime.Add("Attack", 0);
        coolDownTime.Add("Jump", 0);
        coolDownTime.Add("FireBall", 0);
        coolDownTime.Add("KnockBack", 0);
        state.Add("onGround", false);
        state.Add("onMovingPlatform", false);
        state.Add("onKnockBack", false);
    }

    private float _jumpForce;
    private Vector2 _facing = Vector2.right;
    private Animator _animator;
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;
    private int _health = 3;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _runSpeed = GameManager.PlayerRunSpeed;
        _jumpTimes = GameManager.PlayerJumpTimes;
        _jumpForce = GameManager.PlayerJumpForce;
        FireBallCoolDown = GameManager.FireBallCoolDown;
        AttackCoolDown = GameManager.AttackCoolDown;
        JumpingCoolDown = GameManager.JumpingCoolDown;
        _jumping = _jumpTimes;
    }

    public IEnumerator Injure(bool knockBack = false)
    {
        _onFreeze = true;
        _health--;
        UIController.instance.Injure();
        if (knockBack)
            KnockBack();
        yield return new WaitForSeconds(0.5f);

        if (_health <= 0)
        {
            _animator.SetTrigger("Dead");
            UIController.instance.GameOver();
        }
        else
        {
            _animator.SetTrigger("Injure");
            Debug.Log("Health:" + _health);
        }
        _onFreeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_onFreeze)
        {
            return;
        }
        bool isCrouching;
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        isCrouching = (_jumping != 0) ? false : (inputY == -1);
        // ========================= set Crouching
        _animator.SetBool("IsCrouching", isCrouching);
        GetComponent<Rigidbody2D>().sharedMaterial.friction = (isCrouching) ? 1f : 0;

        // ========================= check jump
        if (
            (Input.GetButton("Jump") || (inputY == 1))
            && (!isCrouching)
            && (_jumping < _jumpTimes)
            && (nextTime["Jump"] < Time.time)
        )
        {
            nextTime["Jump"] = Time.time + JumpingCoolDown;
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce * 10);
            _jumping++;
        }
        // ========================= check move
        if (!(inputY == -1))
            _rb.velocity = new Vector2(inputX * _runSpeed, _rb.velocity.y);
        Flip(inputX);
        _animator.SetFloat("Speed", Mathf.Abs(_rb.velocity.x));
        float colliderWidth = _collider.size.x;
        Collider2D collider = Physics2D.OverlapCircle(_groundCheck, colliderWidth, GroundLayerMask);
        bool check = (collider != null);

        _groundCheck = (Vector2)transform.position + Vector2.down;
        // ========================= On Land
        if (check && (check != _onGround))
        {
            _jumping = 0;
        }
        _onGround = check;

        if (check && _onMovingPlatform && _jumping == 0 && collider.CompareTag("MovingPlatform"))
        {
            float colliderX = collider.GetComponent<Rigidbody2D>().velocity.x;
            float colliderY = collider.GetComponent<Rigidbody2D>().velocity.y;
            _rb.velocity = new Vector2(_rb.velocity.x + colliderX, colliderY);
        }

        _animator.SetBool("IsJumping", _jumping != 0);

        //========================= check attack
        Debug.DrawLine(
            transform.position,
            (Vector2)transform.position + _collider.size + _facing,
            Color.red
        );
        Debug.DrawLine(
            transform.position,
            (Vector2)transform.position + -_collider.size + _facing,
            Color.green
        );
        if (Input.GetButton("Fire1") && (nextTime["Attack"] < Time.time))
        {
            nextTime["Attack"] = Time.time + AttackCoolDown;
            // if (Input.GetButtonDown("Fire1") && (_nextAttack < Time.time))
            // {
            //     _nextAttack = Time.time + AttackCoolDown;

            _animator.SetTrigger("Attack");
            Collider2D attacked = Physics2D.OverlapArea(
                (Vector2)transform.position + _collider.size + _facing,
                (Vector2)transform.position - _collider.size + _facing
            );
            if (attacked != null && attacked.gameObject.CompareTag("Enemy"))
            {
                attacked.gameObject.GetComponent<Enemy>().Injure(10);
            }
        }
        // check fireball
        if (Input.GetButton("Fire2") && (nextTime["FireBall"] < Time.time))
        {
            nextTime["FireBall"] = Time.time + FireBallCoolDown;
            GameObject fireball = Instantiate(FireBall, transform.position, Quaternion.identity);
            fireball.GetComponent<FireBall>().Direction = _facing;
            if (_facing == Vector2.left)
                fireball.transform.localScale *= -1;
        }
    }

    Vector2 vel;

    private void FixedUpdate()
    {
        vel = _rb.velocity;
    }

    private void KnockBack(float knockBackForce = 1)
    {
        // Vector2 difference = (transform.position - other.transform.position).normalized;

        // rb.AddForce(difference * knockBackForce, ForceMode2D.Impulse);
        _rb.velocity = vel.normalized * -knockBackForce;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("OutSide"))
        {
            UIController.instance.GameOver();
        }
        if (other.gameObject.CompareTag("MovingPlatform"))
            _onMovingPlatform = true;
        if (other.gameObject.CompareTag("Spikes"))
        {
            KnockBack();
            StartCoroutine(Injure());
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
            _onMovingPlatform = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Win"))
        {
            UIController.instance.Won();
            return;
        }
        if (other.gameObject.CompareTag("Scroll"))
        {
            UIController.instance.GetScroll();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Shuriken"))
        {
            StartCoroutine(Injure());
        }
    }

    private void Flip(float inputX)
    {
        if (!((inputX > 0 && _facing != Vector2.right) || (inputX < 0 && _facing != Vector2.left)))
            return;
        _facing = (inputX > 0) ? Vector2.right : Vector2.left;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnDrawGizmos() { }
}
