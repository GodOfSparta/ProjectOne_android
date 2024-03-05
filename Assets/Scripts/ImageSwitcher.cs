using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSwitcher : MonoBehaviour
{
    public Image[] images; // ������ �����������, ������� �� ������ �����������
    private int currentIndex = 0; // ������ �������� �����������

    void Start()
    {
        // ����� ������ � �����
        Button switchButton = GameObject.Find("SwitchButton").GetComponent<Button>();

        // ��������� ����� ��� ��������� ������� ������
        switchButton.onClick.AddListener(SwitchImage);

        // ���������� ������ ����������� ��� ������
        ShowCurrentImage();
    }

    void SwitchImage()
    {
        currentIndex = (currentIndex + 1) % images.Length;

        // ���� ��� ��������� �����������, ��������� �� ��������� �����
        if (currentIndex == 0)
        {
            LoadNextScene();
        }
        else
        {
            ShowCurrentImage();
        }
    }

    void ShowCurrentImage()
    {
        // ��������� ��� �����������
        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }

        // �������� ������� �����������
        images[currentIndex].gameObject.SetActive(true);
    }

    void LoadNextScene()
    {
        // �������� ������ ������� �����
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ��������� ��������� ����� (��������� �� ������ �����, ���� ��������� �����)
        SceneManager.LoadScene((currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
