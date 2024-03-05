using System.Collections;
using UnityEngine;
using TMPro;

public class ManualPlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float jumpForce = 4f;
    private Rigidbody rb;
    private bool isGrounded = true;
    private Animator anim;
    public HealthBar1BossScene healthBar;
    public int maxHealth = 100;
    private int currentHealth;
    AudioSource playerAudio;
    public AudioClip rollSound;
    public AudioClip hurtSound;
    public AudioClip stepSound;
    public AudioClip bossKillerSound;
    private bool isRolling = false;
    public Collider rollCollider;
    public bool isBonusActive = false;
    private float bonusTime = 3f;
    public ParticleSystem immortalityParticle;
    public ParticleSystem hitParticle;
    public AudioClip jumpSound;
    public bool gameOver = false;
    public GameObject gameOverCanvas;
    public bool isDead = false;
    public AudioClip hurtBossSound;
    public float timeRemaining;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeInvincibleText;
    private float lastDamageTime;
    private float damageCooldown = 2f; // Время в секундах между ударами по боссу
    private bool canMove = true;
    public CameraShake cameraShake;
    public Joystick joystick;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        gameOverCanvas.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Если игрок не может двигаться, просто выходим из функции Update
        if (!canMove)
        {
            return;
        }

        if (currentHealth > 0)
        {
            float horizontalInput = joystick.Horizontal;

            // Создаем вектор скорости по оси X
            Vector3 velocity = new Vector3(horizontalInput * speed, 0, 0);
            // Устанавливаем только горизонтальную составляющую скорости, чтобы избежать вертикального движения
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

            UpdateAnimator(horizontalInput);

            // Если бонус активен и время истекло
            if (isBonusActive && bonusTime <= 0)
            {
                // Деактивируем бонус
                isBonusActive = false;
            }
            // Если бонус активен
            else if (isBonusActive)
            {
                // Уменьшаем оставшееся время
                bonusTime -= Time.deltaTime;
            }

        }
        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetTrigger("Death");
            // Запускаем корутину для вызова метода после 4 секунд
            StartCoroutine(ShowGameOverAfterDelay(4f));
        }
    }

    void Update()
    {
        if (Time.timeScale > 0f) // Проверка, что игра не находится в состоянии паузы
        {
            // Если игрок не может двигаться, просто выходим из функции Update
            if (!canMove)
            {
                return;
            }
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                else
                {
                    timeRemaining = 0;
                    timerIsRunning = false;
                    timeInvincibleText.enabled = false; // Скрываем текст, когда время истекло
                }
            }
        }
    }
    public void OnJumpButtonDown()
    {
        // Если игрок нажимает пробел и находится на земле
        if (isGrounded && !isRolling)
        {
            // Создаем вектор силы по оси Y
            Vector3 jump = new Vector3(0, jumpForce, 0);
            // Применяем силу к Rigidbody
            rb.AddForce(jump, ForceMode.Impulse);
            // Устанавливаем флаг на земле в false
            isGrounded = false;
            // Устанавливаем значение параметра IsJumping в true
            anim.SetBool("IsJumping", true);
            playerAudio.PlayOneShot(jumpSound, 0.5f);

        }
    }

    public void OnRollButtonDown()
    {
        if (!isRolling)
        {
            anim.SetTrigger("Roll");
            playerAudio.PlayOneShot(rollSound, 0.5f);
        }
        if (rollCollider != null)
        {
            rollCollider.enabled = false;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeInvincibleText.text = string.Format("{0}", seconds);
    }

    public void ShowGameOverScreen()
    {
        gameOverCanvas.SetActive(true);

        // Восстанавливаем игровое время
        Time.timeScale = 1f;
    }

    IEnumerator ShowGameOverAfterDelay(float delay)
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(3);

        // Вызываем метод отображения экрана Game Over
        ShowGameOverScreen();
        Time.timeScale = 0f;
    }
    public void OnRollStart()
    {
        isRolling = true;
    }

    public void OnRollEnd()
    {
        isRolling = false;

        if (!isRolling && isGrounded && rollCollider != null)
        {
            rollCollider.enabled = true;
        }
    }

    // Функция для получения урона
    public void TakeDamage(int damage)
    {
        // Если бонус активен, игрок не получает урон
        if (isBonusActive)
        {
            return;
        }
        StartCoroutine(cameraShake.Shake(0.1f, 0.25f));
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        anim.SetTrigger("FellBack");
        playerAudio.PlayOneShot(hurtSound, 0.3f);
        // Устанавливаем флаг, который предотвращает дальнейшее движение игрока
        canMove = false;
    }

    void UpdateAnimator(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            // Устанавливаем значение параметра IsMoving в true
            anim.SetBool("IsMoving", true);
            // Устанавливаем значение параметра Direction в соответствии с направлением
            anim.SetFloat("Direction", Mathf.Sign(horizontalInput));
            // Создаем вектор направления взгляда персонажа
            Vector3 lookDirection = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
            // Поворачиваем персонажа в направлении взгляда
            transform.LookAt(transform.position + lookDirection);
        }
        // Если игрок не движется
        else
        {
            // Устанавливаем значение параметра IsMoving в false
            anim.SetBool("IsMoving", false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Boss1Controller boss = collision.gameObject.GetComponent<Boss1Controller>();
        if (collision.gameObject.CompareTag("Boss") && isBonusActive)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                playerAudio.PlayOneShot(hurtBossSound, 0.4f);
                {
                    boss.TakeDamage(20);
                    lastDamageTime = Time.time; // Обновляем время последнего урона
                }

            }


        }
        if (collision.gameObject.CompareTag("Boss") && !isBonusActive)
        {
            hitParticle.Play();
            TakeDamage(25);
        }
        // Если игрок получает урон от BossBullet и бонус не активен
        if (collision.gameObject.CompareTag("BossBullet") && !isBonusActive)
        {
            hitParticle.Play();
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Boss"))
        {
            // Устанавливаем флаг на земле в true
            isGrounded = true;
            // Устанавливаем значение параметра IsJumping в false
            anim.SetBool("IsJumping", false);
        }

        if (collision.gameObject.CompareTag("BossKiller"))
        {
            timerIsRunning = true;
            timeInvincibleText.enabled = true;
            // Обнуляем отсчёт времени
            timeRemaining = 3f;
            isBonusActive = true;
            bonusTime = 3f;
            immortalityParticle.Play();
            Destroy(collision.gameObject);
            playerAudio.PlayOneShot(bossKillerSound, 0.5f);
            timerIsRunning = true;
            timeInvincibleText.enabled = true; // Убедитесь, что текст видим, когда таймер запущен
        }
    }
    // Корутина для восстановления возможности движения
    IEnumerator EnableMovementAfterDelay(float delay)
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(delay);

        // Восстанавливаем возможность движения
        canMove = true;
    }
}
