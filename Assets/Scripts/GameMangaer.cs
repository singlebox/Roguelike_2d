using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMangaer : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;          //每回合时间
    public int playerFoodPoints = 100;
    [HideInInspector] public bool PlayerTurn=true;      //玩家和敌人的回合
    public static GameMangaer instance=null;
    public BoardManager boardScript;

    [HideInInspector] public int level = 1;
    private GameObject quit;
    private bool enemiesMoving;
    private List<Enemy> enemies;
    private Text levelText;
    private GameObject Restart;
    private GameObject levelImage;
    private bool doingSetup;            //检测是否在关卡加载界面，并禁止玩家移动

    // Start is called before the first frame update
    void Awake()        //懒汉模型
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
        boardScript = GetComponent<BoardManager>();     //获取组件
        InitGame();
    }

    private void OnLevelWasLoaded(int index)
    {
        //level++;
        InitGame();
    }

    public void GameOver()     //结束游戏
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
