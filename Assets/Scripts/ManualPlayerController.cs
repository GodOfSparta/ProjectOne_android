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
    private float damageCooldown = 2f; // ����� � �������� ����� ������� �� �����
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
        // ���� ����� �� ����� ���������, ������ ������� �� ������� Update
        if (!canMove)
        {
            return;
        }

        if (currentHealth > 0)
        {
            float horizontalInput = joystick.Horizontal;

            // ������� ������ �������� �� ��� X
            Vector3 velocity = new Vector3(horizontalInput * speed, 0, 0);
            // ������������� ������ �������������� ������������ ��������, ����� �������� ������������� ��������
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

            UpdateAnimator(horizontalInput);

            // ���� ����� ������� � ����� �������
            if (isBonusActive && bonusTime <= 0)
            {
                // ������������ �����
                isBonusActive = false;
            }
            // ���� ����� �������
            else if (isBonusActive)
            {
                // ��������� ���������� �����
                bonusTime -= Time.deltaTime;
            }

        }
        if (currentHealth <= 0)
        {
            isDead = true;
            anim.SetTrigger("Death");
            // ��������� �������� ��� ������ ������ ����� 4 ������
            StartCoroutine(ShowGameOverAfterDelay(4f));
        }
    }

    void Update()
    {
        if (Time.timeScale > 0f) // ��������, ��� ���� �� ��������� � ��������� �����
        {
            // ���� ����� �� ����� ���������, ������ ������� �� ������� Update
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
                    timeInvincibleText.enabled = false; // �������� �����, ����� ����� �������
                }
            }
        }
    }
    public void OnJumpButtonDown()
    {
        // ���� ����� �������� ������ � ��������� �� �����
        if (isGrounded && !isRolling)
        {
            // ������� ������ ���� �� ��� Y
            Vector3 jump = new Vector3(0, jumpForce, 0);
            // ��������� ���� � Rigidbody
            rb.AddForce(jump, ForceMode.Impulse);
            // ������������� ���� �� ����� � false
            isGrounded = false;
            // ������������� �������� ��������� IsJumping � true
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

        // ��������������� ������� �����
        Time.timeScale = 1f;
    }

    IEnumerator ShowGameOverAfterDelay(float delay)
    {
        // ���� ��������� ���������� ������
        yield return new WaitForSeconds(3);

        // �������� ����� ����������� ������ Game Over
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

    // ������� ��� ��������� �����
    public void TakeDamage(int damage)
    {
        // ���� ����� �������, ����� �� �������� ����
        if (isBonusActive)
        {
            return;
        }
        StartCoroutine(cameraShake.Shake(0.1f, 0.25f));
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        anim.SetTrigger("FellBack");
        playerAudio.PlayOneShot(hurtSound, 0.3f);
        // ������������� ����, ������� ������������� ���������� �������� ������
        canMove = false;
    }

    void UpdateAnimator(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            // ������������� �������� ��������� IsMoving � true
            anim.SetBool("IsMoving", true);
            // ������������� �������� ��������� Direction � ������������ � ������������
            anim.SetFloat("Direction", Mathf.Sign(horizontalInput));
            // ������� ������ ����������� ������� ���������
            Vector3 lookDirection = new Vector3(Mathf.Sign(horizontalInput), 0, 0);
            // ������������ ��������� � ����������� �������
            transform.LookAt(transform.position + lookDirection);
        }
        // ���� ����� �� ��������
        else
        {
            // ������������� �������� ��������� IsMoving � false
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
                    lastDamageTime = Time.time; // ��������� ����� ���������� �����
                }

            }


        }
        if (collision.gameObject.CompareTag("Boss") && !isBonusActive)
        {
            hitParticle.Play();
            TakeDamage(25);
        }
        // ���� ����� �������� ���� �� BossBullet � ����� �� �������
        if (collision.gameObject.CompareTag("BossBullet") && !isBonusActive)
        {
            hitParticle.Play();
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Boss"))
        {
            // ������������� ���� �� ����� � true
            isGrounded = true;
            // ������������� �������� ��������� IsJumping � false
            anim.SetBool("IsJumping", false);
        }

        if (collision.gameObject.CompareTag("BossKiller"))
        {
            timerIsRunning = true;
            timeInvincibleText.enabled = true;
            // �������� ������ �������
            timeRemaining = 3f;
            isBonusActive = true;
            bonusTime = 3f;
            immortalityParticle.Play();
            Destroy(collision.gameObject);
            playerAudio.PlayOneShot(bossKillerSound, 0.5f);
            timerIsRunning = true;
            timeInvincibleText.enabled = true; // ���������, ��� ����� �����, ����� ������ �������
        }
    }
    // �������� ��� �������������� ����������� ��������
    IEnumerator EnableMovementAfterDelay(float delay)
    {
        // ���� ��������� ���������� ������
        yield return new WaitForSeconds(delay);

        // ��������������� ����������� ��������
        canMove = true;
    }
}
