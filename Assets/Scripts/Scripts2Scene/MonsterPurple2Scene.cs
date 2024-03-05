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
        // ���������� ����� ������� ������ 3 �������
        InvokeRepeating("Jump", 1f, 3f);
        // ��������� ������ ��� ����������� ����������� ����� �������� �����
        Invoke("DestroyObstacle", destroyDelay);
    }

    public void Jump()
    {
        // ��������� ���� ������
        enemyRigidbody.AddForce(new Vector3(-jumpForce, jumpForce, 0), ForceMode.Impulse);
        // �������� �������� ������
        enemyAnimator.SetTrigger("Jump");
    }
    private void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
