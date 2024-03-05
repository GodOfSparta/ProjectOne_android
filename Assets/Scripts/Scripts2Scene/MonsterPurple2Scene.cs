using UnityEngine;

public class MonsterPurple2Scene : MonoBehaviour
{
    private Animator enemyAnimator;
    private Rigidbody enemyRigidbody;
    float jumpForce = 3f;
    private float destroyDelay = 7f;

    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyRigidbody = GetComponent<Rigidbody>();
        // Заставляем врага прыгать каждые 3 секунды
        InvokeRepeating("Jump", 1f, 3f);
        // Запускаем таймер для уничтожения препятствия через заданное время
        Invoke("DestroyObstacle", destroyDelay);
    }

    public void Jump()
    {
        // Применяем силу прыжка
        enemyRigidbody.AddForce(new Vector3(-jumpForce, jumpForce, 0), ForceMode.Impulse);
        // Включаем анимацию прыжка
        enemyAnimator.SetTrigger("Jump");
    }
    private void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
