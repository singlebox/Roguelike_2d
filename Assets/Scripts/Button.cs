using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public static bool flag=false;
    public GameObject gameManager;
    public void Restart()
    {
        if (GameMangaer.instance == null)
        Instantiate(gameManager);       //û�еĻ�����ʵ�����Ǹ�Ԥ����
        GameMangaer.instance.level = 1;
        SoundManager.instance.musicSource.Play();
        Button.flag = true;
        SceneManager.LoadScene("MyScene");

    }

    public void Quit()
    {
        Application.Quit();
    }
}
