using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    
    void Awake()
    {
        if (GameMangaer.instance == null)
            Instantiate(gameManager);       //没有的话，则实例化那个预制体
    }

    
}
