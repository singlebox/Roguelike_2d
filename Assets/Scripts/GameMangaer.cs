using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMangaer : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;          //ÿ�غ�ʱ��
    public int playerFoodPoints = 100;
    [HideInInspector] public bool PlayerTurn=true;      //��Һ͵��˵Ļغ�
    public static GameMangaer instance=null;
    public BoardManager boardScript;

    [HideInInspector] public int level = 1;
    private GameObject quit;
    private bool enemiesMoving;
    private List<Enemy> enemies;
    private Text levelText;
    private GameObject Restart;
    private GameObject levelImage;
    private bool doingSetup;            //����Ƿ��ڹؿ����ؽ��棬����ֹ����ƶ�

    // Start is called before the first frame update
    void Awake()        //����ģ��
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance!=this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();     //��ȡ���
        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        //level++;
        InitGame();
    }

    public void GameOver()     //������Ϸ
    {
        levelText.text = "After " + level + " days,you starved.";
        levelImage.SetActive(true);
        quit.SetActive(true);
        Restart.SetActive(true);
        Destroy(this);
        //enabled = false;
    }
    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        quit = GameObject.Find("Quit");
        quit.SetActive(false);
        Restart = GameObject.Find("Restart");
        Restart.SetActive(false);
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", 1f);
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count==0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for(int i=0;i<enemies.Count;i++)
        {
            enemies[i].MoveEnemy(boardScript.book);
            yield return new WaitForSeconds(turnDelay);
        }
        PlayerTurn = true;
        enemiesMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerTurn||enemiesMoving||doingSetup)
        {
            return;
        }
        StartCoroutine(MoveEnemies());
    }
    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
}
