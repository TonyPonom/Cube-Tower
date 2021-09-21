using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;
    public void Start()
    {
        if(PlayerPrefs.GetString("music") == "No" && gameObject.name == "music")//����� ������������ ����� ���������� ���� ���� �������� 
        // � ����������� ������ �� ������ �����
            GetComponent<Image>().sprite = musicOff;
    }
    public void RestartGame()//�������� ���� ������ �������� ��������� �����
    {
        if (PlayerPrefs.GetString("music") != "No" )// �������� ����
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadVk()//�������� �� ������ �� �� �������� �� � ��������
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        Application.OpenURL("https://vk.com/");
    }
    public void MusicWork()
    {
        
        if (PlayerPrefs.GetString("music") == "No")//������� � ���������������� ������������������ �� ������
        {
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<AudioSource>().Play();
            GetComponent<Image>().sprite = musicOn;// ���� ������������ �������� ������ �� �����������
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
    public void LoadShop()
    {//��������� �������
        if (PlayerPrefs.GetString("music") != "No")// �������� ����
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Shop");
    }
    public void CloseShop()
    {//��������� �������
        if (PlayerPrefs.GetString("music") != "No")// �������� ����
            GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Main");
    }
}
