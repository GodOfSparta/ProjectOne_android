using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController2Scene : MonoBehaviour
{
    private bool canJump = true;
    private bool isRolling = false;
    private Rigidbody playerRb;
    float jumpForce = 500;
    float moveSpeed = 7;
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
    public AudioClip crashSound;
    public AudioClip healthSound;
    public AudioClip immortalitySound;
    private AudioSource playerAudio;
    public Collider rollCollider;
    private const float swipeThreshold = 50f;
    private Vector2 swipeUpStartPos;
    private const float swipeUpThreshold = 100f;
    public Image fadeOutImage; // —сылка на изображение дл€ затемнени€ экрана

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
            playerAudio.PlayOneShot(crashSound, 0.7f);
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
    }

    void IncreaseMoveSpeed()
    {
        moveSpeed += 0.1f;
    }
}