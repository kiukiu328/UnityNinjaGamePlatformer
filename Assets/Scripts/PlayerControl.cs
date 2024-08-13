using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // for checking on ground
    public LayerMask GroundLayerMask;
    public float InjureCoolDown;
    public GameObject FireBall;
    public AudioClip InjureSound;
    public AudioClip AttackSound;
    public AudioClip GetScrollSound;
    public AudioClip JumpSound;
    private Vector2 _groundCheck;
    private float JumpingCoolDown;
    private float AttackCoolDown;
    private float FireBallCoolDown;


    private float _runSpeed;
    private int _jumping;
    private int _jumpTimes;
    private bool _onGround;
    private bool _onMovingPlatform;
    private bool _onFreeze;

    private Dictionary<string, float> coolDownTime = new Dictionary<string, float>();
    private Dictionary<string, float> nextTime = new Dictionary<string, float>();

    public PlayerControl()
    {
        nextTime.Add("Attack", 0);
        nextTime.Add("Jump", 0);
        nextTime.Add("FireBall", 0);
        nextTime.Add("KnockBack", 0);
        nextTime.Add("Injure", 0);
        coolDownTime.Add("Attack", 0);
        coolDownTime.Add("Jump", 0);
        coolDownTime.Add("FireBall", 0);
        coolDownTime.Add("KnockBack", 0);
        coolDownTime.Add("Injure", 0);
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
        _runSpeed = GameManager.gameSetting.PlayerRunSpeed;
        _jumpTimes = GameManager.gameSetting.PlayerJumpTimes;
        _jumpForce = GameManager.gameSetting.PlayerJumpForce;
        FireBallCoolDown = GameManager.gameSetting.FireBallCoolDown;
        AttackCoolDown = GameManager.gameSetting.AttackCoolDown;
        JumpingCoolDown = GameManager.gameSetting.JumpingCoolDown;
        _health = GameManager.gameSetting.PlayerHP;
        _jumping = _jumpTimes;
    }
    // take damage
    public IEnumerator Injure(bool freeze = true, bool knockBack = false)
    {
        if (nextTime["Injure"] > Time.time)
        {
            yield break;
        }
        nextTime["Injure"] = Time.time + InjureCoolDown;
        _onFreeze = freeze;
        _health--;
        UIController.instance.Injure();
        if (knockBack)
            KnockBack();
        PlaySoundEffect(InjureSound);

        // if dont have hp then game over
        if (_health <= 0)
        {
            _animator.SetTrigger("Dead");
            yield return new WaitForSeconds(0.6f);
            UIController.instance.GameOver();

        }
        else
        {
            _animator.SetTrigger("Injure");
            Debug.Log("Health:" + _health);
            yield return new WaitForSeconds(0.8f);

        }
        _onFreeze = false;
    }


    void Update()
    {

        // when freeze do nothing e.g. when knockback
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
        if (isCrouching)
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }

        // ========================= check jump
        if (
            (Input.GetButton("Jump") || (inputY == 1)) // get Jump key or W or up arrow
            && (!isCrouching) // if not Crouching
            && (_jumping < _jumpTimes) // if arrive max jumptimes
            && (nextTime["Jump"] < Time.time) // if cool down not yet
        )
        {
            PlaySoundEffect(JumpSound);
            nextTime["Jump"] = Time.time + JumpingCoolDown; // set next cool down
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _jumpForce * 10);// set jump force
            _jumping++;
        }
        // ========================= check move
        if (!isCrouching) // if not Crouching then move
            _rb.linearVelocity = new Vector2(inputX * _runSpeed, _rb.linearVelocity.y);
        Flip(inputX); // flip by user input
        _animator.SetFloat("Speed", Mathf.Abs(_rb.linearVelocity.x)); // set animator speed for control the animation speed


        // ========================= On Land checking
        float colliderWidth = _collider.size.x;
        Collider2D collider = Physics2D.OverlapCircle(_groundCheck, colliderWidth, GroundLayerMask);
        bool check = (collider != null);
        _groundCheck = (Vector2)transform.position + Vector2.down;
        if (check && (check != _onGround))
        {
            _jumping = 0;
        }
        _onGround = check;// set previous state to current
        // if touching the moving platform then follow it
        if (check && _onMovingPlatform && _jumping == 0 && collider.CompareTag("MovingPlatform"))
        {
            float colliderX = collider.GetComponent<Rigidbody2D>().linearVelocity.x;
            float colliderY = collider.GetComponent<Rigidbody2D>().linearVelocity.y;
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x + colliderX, colliderY);
        }

        _animator.SetBool("IsJumping", _jumping != 0);

        //========================= check attack
        Attack();
    }
    private void Attack()
    {
        // normal attack if onclick and cool down
        if (Input.GetButton("Fire1") && (nextTime["Attack"] < Time.time))
        {
            // set cool down
            nextTime["Attack"] = Time.time + AttackCoolDown;
            PlaySoundEffect(AttackSound);
            _animator.SetTrigger("Attack");
            // check the attack area
            Collider2D attacked = Physics2D.OverlapArea(
                (Vector2)transform.position + _collider.size + _facing,
                (Vector2)transform.position - _collider.size + _facing
            );
            if (attacked != null && attacked.gameObject.CompareTag("Enemy"))
            {
                attacked.gameObject.GetComponent<Enemy>().Injure(GameManager.gameSetting.AttackDamage);
            }

        }
        // fireball attack if onclick and cool down
        if (Input.GetButton("Fire2") && (nextTime["FireBall"] < Time.time))
        {
            // set cool down
            nextTime["FireBall"] = Time.time + FireBallCoolDown;
            //clone a fireball object
            GameObject fireball = Instantiate(FireBall, transform.position, Quaternion.identity);
            fireball.GetComponent<FireBall>().Direction = _facing;
            if (_facing == Vector2.left)
                fireball.transform.localScale *= -1;
        }
    }
    // for knockBack checking
    private Vector2 _vel;
    private void FixedUpdate()
    {
        _vel = _rb.linearVelocity;
    }

    private void KnockBack(float knockBackForce = 1)
    {
        _rb.linearVelocity = _vel.normalized * -knockBackForce;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if go outside gameover
        if (other.gameObject.CompareTag("OutSide"))
        {
            UIController.instance.GameOver();
        }

        if (other.gameObject.CompareTag("MovingPlatform"))
            _onMovingPlatform = true;
        if (other.gameObject.CompareTag("Spikes"))
        {
            KnockBack(5);
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
            PlaySoundEffect(GetScrollSound);
            UIController.instance.GetScroll();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("HP"))
        {
            if (_health < GameManager.gameSetting.PlayerHP)
            {
                UIController.instance.GetHP();
                _health++;
            }

            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Shuriken"))
        {
            StartCoroutine(Injure(false));
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
    private void PlaySoundEffect(AudioClip effect)
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = effect;
        audio.Play();
    }


}
