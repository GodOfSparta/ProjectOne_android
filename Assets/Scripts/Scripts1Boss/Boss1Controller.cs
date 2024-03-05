using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Boss1Controller : MonoBehaviour
{
    public float speed = 5f;
    private bool hasReachedStopPoint = false;
    private Animator anim;
    private Rigidbody bossRb;
    public HealthBar1BossScene healthBar; // Ссылка на скрипт HealthBar
    public int maxHealth = 100; // Максимальное здоровье
    private int currentHealth; // Текущее здоровье
    public int damage = 20; // Урон, который наносит босс
    public Transform player; // Ссылка на игрока
    public GameObject bulletPrefab; // Ссылка на префаб пули
    public Transform firePoint; // Точка, откуда будет вылетать пуля
    float bulletForce = 800f; // Сила, с которой будет вылетать пуля
    private bool isShooting = false; // Флаг, показывающий, стреляет ли босс в данный момент
    public AudioSource audioSource; // Ссылка на компонент AudioSource
    public AudioClip shootSound; // Ссылка на аудиофайл с звуком выстрела
    public AudioClip footstepSound; // Ссылка на аудиофайл с звуком шаг
    public AudioClip attackSound; // Ссылка на аудиофайл с звуком атаки
    public AudioClip deathSound;
    public Image fadeOutImage; // Ссылка на изображение для затемнения экрана
    public GameObject[] bulletPrefabs;
    public ParticleSystem hitParticle;

    void Start()
    {
        bossRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Invoke("StopBoss", 3.5f);
        // Установка начального здоровья
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (!hasReachedStopPoint)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            // Вычисляем расстояние между боссом и игроком
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Если босс находится на расстоянии 4 от игрока
            if (distanceToPlayer <= 4)
            {
                anim.SetTrigger("SwordAttack");
            }
            // Если босс находится на расстоянии больше 7 от игрока и не стреляет в данный момент
            else if (distanceToPlayer > 7 && !isShooting)
            {
                anim.SetTrigger("Shoot");
            }
        }
    }
    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSound, 1f);
    }

    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound, 1f);
    }

    void Shoot()
    {
        // Устанавливаем флаг в true
        isShooting = true;
        // Выбираем случайный префаб пули
        GameObject bulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];
        // Создаем экземпляр случайного префаба пули
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // Получаем компонент Rigidbody пули
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        // Выбираем случайную силу для пули
        bulletForce = Random.Range(200f, 1200f);
        // Применяем силу к Rigidbody пули, чтобы она вылетела в направлении игрока
        rb.AddRelativeForce(transform.forward * bulletForce, ForceMode.Force);
        // Вызываем функцию ResetShooting через некоторое время
        Invoke("ResetShooting", 0.1f); // Замените на время, которое должно пройти между выстрелами
        // Воспроизводим звук выстрела
        audioSource.PlayOneShot(shootSound, 0.4f);
    }

    void ResetShooting()
    {
        // Устанавливаем флаг в false
        isShooting = false;
    }

    public void TakeDamage(int damage)
    {

        hitParticle.Play();
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Die();
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            audioSource.PlayOneShot(deathSound, 1f);
            anim.SetTrigger("Death");
            PlayerPrefs.SetInt("Boss1Defeated", 1); // Сохраняем факт победы над боссом
            PlayerPrefs.Save(); // Сохраняем изменения
            StartCoroutine(FadeOutAndLoadNextScene());
        }
    }
    IEnumerator FadeOutAndLoadNextScene()
    {
        // Затемняем экран в течение 7 секунд
        for (float i = 0; i <= 1; i += Time.deltaTime / 7)
        {
            // Устанавливаем прозрачность изображения
            fadeOutImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
        // Получаем текущую активную сцену
        Scene currentScene = SceneManager.GetActiveScene();
        // Вычисляем индекс следующей сцены
        int nextSceneIndex = currentScene.buildIndex + 1;
        // Если следующая сцена превышает общее количество сцен, возвращаемся к первой сцене (индекс 0)
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BossKiller"))
        {
            Destroy(collision.gameObject);
        }

    }
    void StopBoss()
    {
        anim.SetTrigger("Shoot");
        hasReachedStopPoint = true;
    }
}
