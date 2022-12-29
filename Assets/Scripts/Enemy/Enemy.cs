using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth = 100;
    public int CurrentHealth;
    protected Vector2 _facing = Vector2.right;
    public HealthBar HealthBar;

    protected void Init()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    private void Start()
    {
        Init();
    }

    public void Injure(int damage)
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
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
