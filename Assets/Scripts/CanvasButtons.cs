using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;
    public void Start()
    {
        if(PlayerPrefs.GetString("music") == "No" && gameObject.name == "music")//чтобы включенность звука оставалась даже если свернуть 
        // и действовало только на кнопку звука
            GetComponent<Image>().sprite = musicOff;
    }
    public void RestartGame()//начинаем игру заново загружая начальную сцену
    {
        if (PlayerPrefs.GetString("music") != "No" )// включаем звук
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadVk()//проходим по кнопке вк на страницу вк в браузере
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        Application.OpenURL("https://vk.com/");
    }
    public void MusicWork()
    {
        
        if (PlayerPrefs.GetString("music") == "No")//смотрим в пользовательских настройкахвключена ли музыка
        {
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<AudioSource>().Play();
            GetComponent<Image>().sprite = musicOn;// если пользователь включает музыку то проигрываем
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
    public void LoadShop()
    {//загружаем машазин
        if (PlayerPrefs.GetString("music") != "No")// включаем звук
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Shop");
    }
    public void CloseShop()
    {//загружаем машазин
        if (PlayerPrefs.GetString("music") != "No")// включаем звук
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Main");
    }
}
