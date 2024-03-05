using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool canJump = true;
    private bool isRolling = false;
    private Rigidbody playerRb;
    public float jumpForce;
    public float moveSpeed;
    public bool isOnGround = true;
    public bool gameOver = false;
    private Animator anim;
    public bool isMoving = false;
    public GameObject gameOverCanvas;
    private HealthManager healthManager;
    public bool isDead = false;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public ParticleSystem healthParticle;
    public ParticleSystem immortalityParticle;
    public AudioClip jumpSound;
    public AudioClip rollSound;
    
    public AudioClip healthSound;
    public AudioClip immortalitySound;
    private AudioSource playerAudio;
    public Collider rollCollider;
    private const float swipeThreshold = 50f;
    private Vector2 swipeUpStartPos;
    private const float swipeUpThreshold = 100f;
    public Image fadeOutImage; // Ссылка на изображение для затемнения экрана

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        healthManager = GetComponent<HealthManager>();
        healthManager.gameOverCanvas.SetActive(false);
        InvokeRepeating("IncreaseMoveSpeed", 1f, 1f);
    }

    void Update()
    {
        if (healthManager.gameOver)
        {
            ShowGameOverScreen();
        }

        if (!isRolling && IsSwipeUp() && canJump)
        {
            Jump();
        }
        else if (!isRolling && IsSwipeDown() && isOnGround)
        {
            Roll();
        }
        if (Input.GetKeyDown(KeyCode.Space) && canJump && !isRolling)
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && isOnGround && !isRolling)
        {
            Roll();
        }

        // Если игрок достигает по X
        if (transform.position.x >= 1800)
        {
            // Останавливаем движение игрока
            isMoving = false;
            anim.SetBool("IsRunning", false);

            // Запускаем анимацию прыжка
            BigJump();
            // Начинаем корутину для плавного затемнения и перехода на следующую сцену
            StartCoroutine(FadeOutAndLoadNextScene());

            PlayerPrefs.SetInt("Level2Unlocked", 1); // Сохраняем факт разблокировки второго уровня
            PlayerPrefs.SetInt("Level3Unlocked", 1); // Сохраняем факт разблокировки второго уровня
            PlayerPrefs.Save(); // Сохраняем изменения
        }
    }

    void BigJump()
    {
        // Устанавливаем силу прыжка по диагонали
        Vector3 jumpForce = new Vector3(1, 1.3f, 0) * 13f;

        // Применяем силу к Rigidbody игрока
        playerRb.AddForce(jumpForce, ForceMode.Impulse);
        anim.SetTrigger("BigJump");
        dirtParticle.Stop();
    }

    bool IsSwipeUp()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                swipeUpStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                float swipeDistance = touch.position.y - swipeUpStartPos.y;
                return swipeDistance > swipeUpThreshold;
            }
        }
        return false;
    }

    bool IsSwipeDown()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).deltaPosition.y < -swipeThreshold;
    }

    void FixedUpdate()
    {
        if (isDead)
            return;
        Move();

        if (isOnGround)
        {
            canJump = true;
            isOnGround = true;
        }
        // Ограничиваем движение персонажа по оси Y
        if (transform.position.y > 7.5f)
        {
            Vector3 newPosition = new Vector3(transform.position.x, 7.5f, transform.position.z);
            transform.position = newPosition;
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

        // Загружаем следующую сцену
        SceneManager.LoadScene(nextSceneIndex);
    }

    void Move()
    {
        Vector3 forwardMovement = new Vector3(1, 0, 0);
        transform.Translate(forwardMovement * moveSpeed * Time.fixedDeltaTime);
        isMoving = Mathf.Abs(forwardMovement.x) > 0.1f;
        anim.SetBool("IsRunning", isMoving);
    }

    void Jump()
    {
        playerAudio.PlayOneShot(jumpSound, 0.2f);
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        canJump = false;
        anim.SetTrigger("Jump");
        dirtParticle.Stop();
    }

    void JumpRock()
    {
        playerAudio.PlayOneShot(jumpSound, 0.2f);
        playerRb.AddForce(Vector3.up * 700f, ForceMode.Impulse);
        isOnGround = false;
        canJump = false;
        anim.SetTrigger("Jump");
        dirtParticle.Stop();
    }


    void Roll()
    {
        dirtParticle.Stop();
        anim.SetTrigger("Roll");

        if (rollCollider != null)
        {
            rollCollider.enabled = false;
        }
    }

    public void OnRollStart()
    {
        isRolling = true;
        dirtParticle.Stop();
        playerAudio.PlayOneShot(rollSound, 0.5f);
    }

    public void OnRollEnd()
    {
        isRolling = false;

        if (!isRolling && isOnGround && rollCollider != null)
        {
            rollCollider.enabled = true;
        }
        dirtParticle.Play();
    }

    void ShowGameOverScreen()
    {
        healthManager.gameOverCanvas.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            
            explosionParticle.Play();
            dirtParticle.Stop();
        }
        else if (collision.gameObject.CompareTag("Health"))
        {
            playerAudio.PlayOneShot(healthSound, 0.4f);
            healthParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Positive"))
        {
            playerAudio.PlayOneShot(immortalitySound, 0.4f);
            immortalityParticle.Play();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Rock"))
        {
            JumpRock();
        }
    }

    void IncreaseMoveSpeed()
    {
        moveSpeed += 0.1f;
    }
}