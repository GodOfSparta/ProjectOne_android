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
    public HealthBar1BossScene healthBar; // ������ �� ������ HealthBar
    public int maxHealth = 100; // ������������ ��������
    private int currentHealth; // ������� ��������
    public int damage = 20; // ����, ������� ������� ����
    public Transform player; // ������ �� ������
    public GameObject bulletPrefab; // ������ �� ������ ����
    public Transform firePoint; // �����, ������ ����� �������� ����
    float bulletForce = 800f; // ����, � ������� ����� �������� ����
    private bool isShooting = false; // ����, ������������, �������� �� ���� � ������ ������
    public AudioSource audioSource; // ������ �� ��������� AudioSource
    public AudioClip shootSound; // ������ �� ��������� � ������ ��������
    public AudioClip footstepSound; // ������ �� ��������� � ������ ���
    public AudioClip attackSound; // ������ �� ��������� � ������ �����
    public AudioClip deathSound;
    public Image fadeOutImage; // ������ �� ����������� ��� ���������� ������
    public GameObject[] bulletPrefabs;
    public ParticleSystem hitParticle;

    void Start()
    {
        bossRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Invoke("StopBoss", 3.5f);
        // ��������� ���������� ��������
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
            // ��������� ���������� ����� ������ � �������
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // ���� ���� ��������� �� ���������� 4 �� ������
            if (distanceToPlayer <= 4)
            {
                anim.SetTrigger("SwordAttack");
            }
            // ���� ���� ��������� �� ���������� ������ 7 �� ������ � �� �������� � ������ ������
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
        // ������������� ���� � true
        isShooting = true;
        // �������� ��������� ������ ����
        GameObject bulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Length)];
        // ������� ��������� ���������� ������� ����
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // �������� ��������� Rigidbody ����
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        // �������� ��������� ���� ��� ����
        bulletForce = Random.Range(200f, 1200f);
        // ��������� ���� � Rigidbody ����, ����� ��� �������� � ����������� ������
        rb.AddRelativeForce(transform.forward * bulletForce, ForceMode.Force);
        // �������� ������� ResetShooting ����� ��������� �����
        Invoke("ResetShooting", 0.1f); // �������� �� �����, ������� ������ ������ ����� ����������
        // ������������� ���� ��������
        audioSource.PlayOneShot(shootSound, 0.4f);
    }

    void ResetShooting()
    {
        // ������������� ���� � false
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
            PlayerPrefs.SetInt("Boss1Defeated", 1); // ��������� ���� ������ ��� ������
            PlayerPrefs.Save(); // ��������� ���������
            StartCoroutine(FadeOutAndLoadNextScene());
        }
    }
    IEnumerator FadeOutAndLoadNextScene()
    {
        // ��������� ����� � ������� 7 ������
        for (float i = 0; i <= 1; i += Time.deltaTime / 7)
        {
            // ������������� ������������ �����������
            fadeOutImage.color = new Color(0, 0, 0, i);
            yield return null;
        }
        // �������� ������� �������� �����
        Scene currentScene = SceneManager.GetActiveScene();
        // ��������� ������ ��������� �����
        int nextSceneIndex = currentScene.buildIndex + 1;
        // ���� ��������� ����� ��������� ����� ���������� ����, ������������ � ������ ����� (������ 0)
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
