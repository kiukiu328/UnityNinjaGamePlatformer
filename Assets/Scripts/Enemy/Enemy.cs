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
    /// <summary>
    /// Check The attack cool down time if not yet end the method.
    /// 
    /// If the attack checking area get a collider have a player tag 
    /// then call the player's injure method.
    /// </summary>
    protected IEnumerator Attack()
    {
        if (_nextAttack > Time.time)
            yield break;
        _nextAttack = Time.time + AttackCoolDown;
        GetComponent<Animator>().SetTrigger("Attack");

        _stopMoving = true;
        yield return new WaitForSeconds(0.5f);
        Collider2D[] results = new Collider2D[10];
        _edgeCollider.Overlap(new ContactFilter2D(), results);

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
    /// <summary>
    /// If the player inside the checking circle then move to the player and attack
    /// </summary>
    /// <returns>The circle area have a player</returns>
    protected virtual bool TrackPlayer()
    {
        //when attacking stop move
        if (_stopMoving)
            return true;
        // get all object inside the circle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, TrackPlayerRadius);
        
        // if didn't get any then do nothing and return
        if (colliders.Length <= 0)
            return false;

        // check all inside the circle if there is player
        foreach (Collider2D collider in colliders)
        {
            // if this collider is not player then check next
            if (!collider.CompareTag("Player"))
                continue;
            // get direction to filp this enemy
            float distance = collider.transform.position.x - transform.position.x;
            Vector2 direction;
            Flip(distance);
            direction = (distance > 0) ? Vector2.right : Vector2.left;
            // if near player then attack
            if (Vector2.Distance(collider.transform.position, transform.position) < _attackDistance)
            {
                // start attack and freeze
                StartCoroutine(Attack());
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                return true;
            }
            _rb.linearVelocity = new Vector2(direction.x * MovingSpeed, _rb.linearVelocity.y);
            _rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            return true;
        }
        return false;
    }
    // for others to call take damage
    public virtual void Injure(int damage)
    {
        GetComponent<Animator>().SetTrigger("Injure");
        CurrentHealth -= damage;
        HealthBar.SetHealth(CurrentHealth);
        // if dont have hp kill this enemy
        if (CurrentHealth <= 0)
        {
            Debug.Log("Die:" + name);
            Destroy(gameObject);
        }
    }
    // method for flip this enemy with movement
    protected void Flip(float inputX)
    {
        if (!((inputX > 0 && _facing != Vector2.right) || (inputX < 0 && _facing != Vector2.left)))
            return;
        _facing = (inputX > 0) ? Vector2.right : Vector2.left;
        Vector3 theScale = FilpObj.transform.localScale;
        theScale.x *= -1;
        FilpObj.transform.localScale = theScale;
    }
    // only for development to show the track player circle
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TrackPlayerRadius);
    }
}
