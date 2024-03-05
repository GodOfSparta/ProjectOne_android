using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSwitcher : MonoBehaviour
{
    public Image[] images; // Массив изображений, которые вы хотите переключать
    private int currentIndex = 0; // Индекс текущего изображения

    void Start()
    {
        // Найти кнопку в сцене
        Button switchButton = GameObject.Find("SwitchButton").GetComponent<Button>();

        // Назначить метод для обработки нажатия кнопки
        switchButton.onClick.AddListener(SwitchImage);

        // Показываем первое изображение при старте
        ShowCurrentImage();
    }

    void SwitchImage()
    {
        currentIndex = (currentIndex + 1) % images.Length;

        // Если это последнее изображение, переходим на следующую сцену
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
        // Отключаем все изображения
        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }

        // Включаем текущее изображение
        images[currentIndex].gameObject.SetActive(true);
    }

    void LoadNextScene()
    {
        // Получаем индекс текущей сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Загружаем следующую сцену (переходим на первую сцену, если достигнут конец)
        SceneManager.LoadScene((currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
