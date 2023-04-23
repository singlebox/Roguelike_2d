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
        Instantiate(gameManager);       //没有的话，则实例化那个预制体
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
