
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
     

    private void Start()
    {
        base.Init();
        _facing = Vector2.left;
        _attackDistance = 2f;
    }
    private void Update()
    {
        TrackPlayer();
    }
    public override void Injure(int damage)
    {
        GetComponent<Animator>().SetTrigger("Injure");
        CurrentHealth -= damage;
        HealthBar.SetHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {

            SceneManager.LoadScene("EntryScene");
        }
    }
}
