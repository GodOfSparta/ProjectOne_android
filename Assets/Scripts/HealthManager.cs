using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cinemachine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int healthRestoreAmount = 10;
    public Slider healthSlider;
    public Animator anim;
    public GameObject gameOverCanvas;
    public bool gameOver = false;
    private PlayerController playerController;
    private bool isInvincible = false;
    private float invincibilityDuration = 7f; // ������������ ������������ � ��������
    private int enemiesKilled = 0; // ����� ���������� ��� �������� ������ ������
    public bool IsInvincible { get { return isInvincible; } }
    private float lastEnemyKillTime; // ������� ���������� ��� ������������ ������� ���������� �������� �����
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI bestKilledText; // ��������� ��� ����������� ������� ����� �������
    private int bestEnemiesKilled = 0; // ����� ���������� ��� ������������ ������� ����� �������
    public ParticleSystem dirtParticle;
    public float timeRemaining = 7;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeInvincibleText;
    [SerializeField] CinemachineVirtualCamera vCamera;
    [SerializeField] float Amplitude;
    [SerializeField] float Frequency;
    public AudioClip playerHurt;
    private AudioSource playerAudio;
    public AudioClip enemyHurt;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAudio = GetComponent<AudioSource>();
        gameOverCanvas.SetActive(false);

        currentHealth = maxHealth;

        // ��������� ���������� ������ ������ � ������ ���� ������� ��� ������
        bestEnemiesKilled = PlayerPrefs.GetInt("BestEnemiesKilled", 0);

        UpdateHealthUI();
        // ��������� UI ��� ����������� ���������� ������ ������ � ������� ����� �������
        UpdateEnemiesKilledUI();
        UpdateBestKilledUI();
    }

    void Update()
    {
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

    IEnumerator Shake()
    {
        playerAudio.PlayOneShot(playerHurt, 0.4f);
        vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Amplitude;
        vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = Frequency;
        yield return new WaitForSeconds(0.3f);
        vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeInvincibleText.text = string.Format("{0}", seconds);
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Clamp01((float)currentHealth / maxHealth);
        }
    }

    void UpdateBestKilledUI()
    {
        if (bestKilledText != null)
        {
            bestKilledText.text = string.Format("Best Killed: {0}", bestEnemiesKilled);
        }
    }

    void UpdateEnemiesKilledUI()
    {
        if (enemiesKilledText != null)
        {
            enemiesKilledText.text = string.Format("Enemies Killed: {0}", enemiesKilled);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            StartCoroutine(Shake());
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();

            if (currentHealth <= 0 && !gameOver)
            {
                // �������������� ��� ��� ������ ���������
                anim.SetTrigger("IsDead");
                dirtParticle.Stop();

                // ������������� ���� isDead � true � ������ PlayerController
                playerController.isDead = true;

                // ��������� �������� ��� ������ ������ ����� 3 ������
                StartCoroutine(ShowGameOverAfterDelay(3f));
            }
        }
    }

    public void ShowGameOverScreen()
    {
        // �������� Canvas ��� ������ Game Over
        gameOverCanvas.SetActive(true);

        // ��������������� ������� �����
        Time.timeScale = 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(25);
        }

        if (collision.gameObject.CompareTag("Health"))
        {
            RestoreHealth(healthRestoreAmount);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Positive"))
        {
            ActivateInvincibility();
            timerIsRunning = true;
            timeInvincibleText.enabled = true;

            // �������� ������ �������
            timeRemaining = 7f; // ��� ��������, ������� ��� �����
        }
        if (collision.gameObject.CompareTag("Enemy") && isInvincible && Time.time - lastEnemyKillTime > 1f)
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                playerAudio.PlayOneShot(enemyHurt, 0.8f);
                enemy.Die();
                enemiesKilled++; // ����������� ������� ������ ������
                UpdateEnemiesKilledUI(); // ��������� UI ��� ����������� ���������� ������ ������
                lastEnemyKillTime = Time.time; // ��������� ����� ���������� ��������

                // ���������, ���������� �� ����� ������ �� ���������
                if (enemiesKilled > bestEnemiesKilled)
                {
                    bestEnemiesKilled = enemiesKilled;
                    PlayerPrefs.SetInt("BestEnemiesKilled", bestEnemiesKilled);
                    PlayerPrefs.Save();
                    UpdateBestKilledUI();
                }
            }
        }
    }

    void ActivateInvincibility()
    {
        isInvincible = true;
        UpdateHealthUI();

        StartCoroutine(DisableInvincibility());
    }

    IEnumerator DisableInvincibility()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    IEnumerator ShowGameOverAfterDelay(float delay)
    {
        // ���� ��������� ���������� ������
        yield return new WaitForSeconds(2);

        // �������� ����� ����������� ������ Game Over
        ShowGameOverScreen();
        Time.timeScale = 0f;
    }

    void RestoreHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }
}
