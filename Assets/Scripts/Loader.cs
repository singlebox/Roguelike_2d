using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    
    void Awake()
    {
        if (GameMangaer.instance == null)
            Instantiate(gameManager);       //û�еĻ�����ʵ�����Ǹ�Ԥ����
    }

    
}
