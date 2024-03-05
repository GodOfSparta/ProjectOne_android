using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator enemyAnimator;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyAnimator.SetTrigger("TakeDamage");

            // Наносим урон игроку
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(25);
            Destroy(gameObject, 0.4f);
        }
    }

    public void Die()
    {
        enemyAnimator.SetTrigger("IsDead");
        Destroy(gameObject, 0.6f);
    }
}
