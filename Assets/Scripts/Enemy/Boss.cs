
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
    /// <summary>
    /// Boss get damage method: when boss die will go back to Entry Scene
    /// </summary>
    /// <param name="damage"> How many damage will this boss get</param>
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
