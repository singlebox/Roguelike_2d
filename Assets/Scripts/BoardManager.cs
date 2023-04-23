using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
[Serializable]
public class BoardManager : MonoBehaviour       //用来生成地图
{
    public class Count      //用来生成地图上的某种元素的区间
    {
        public int Max
        {
            get;
            set;
        }
        public int Min
        {
            get;
            set;
        }

        public Count(int min,int max)
            {
            Max = max;
            Min = min;
            }
    }
    public static int columns = 8;     //地图的列
    public static int rows=8;          //地图的行
    public Count wallCount = new Count(5, 9);       //地图上墙体的数量
    public Count foodCount = new Count(1, 5);       //地图上的补给数量
    public GameObject exit;                         //出口对象
    public GameObject[] floorTiles;                 //存储障碍物对象
    public GameObject[] foodTiles;                  //存储两个food对象
    public GameObject[] wallTiles;                  //存储墙面
    public GameObject[] enemyTiles;                 //存储敌人
    public GameObject[] outerWallTiles;             //存储外墙    
    //[HideInInspector] 
    public int[,] book;           //进行标记 
    private Transform boardHolder;  //使窗口视图整洁 将产生的物体设置为boardHolder的子对象，可以折叠隐藏
    private List<Vector3> gridPositions = new List<Vector3>();      //记录地图上的位置，以及该位置是否有物体

    void InitiaList()           //初始化程序
    {
        gridPositions.Clear();      //首先先清空
        for(int i=1;i<columns-1;i++)
        {
            for(int j=1;j<rows-1;j++)
            {
                gridPositions.Add(new Vector3(i, j, 0f));       //在这个区间生成敌人/障碍物/补给
            }
        }
    }

    void BoardSetup()       //创建外墙和地板
    {
        book = new int[columns, rows];
        boardHolder = new GameObject("Board").transform;
        for (int i = -1; i < columns+1; i++)
        {
            for (int j = -1; j < rows + 1; j++)
            {
                GameObject toInstantiate;
                if (i == -1 || j == -1 || i == columns  || j == rows )
                {       //随机生成一个外墙
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                else
                {       //随机生成一个地面
                    book[i, j] = 1;
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }
                //已经确定好要生成的物体
                //实例化返回的对象是Object类型，需要进行转换
                GameObject instance = Instantiate(toInstantiate, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }   
            }
    }

    Vector3 RandomPosition()        //获得一个随机位置
    {
        int randomIndex = Random.Range(0, gridPositions.Count);     //获得一个随机下标
        Vector3 randomPosition = gridPositions[randomIndex];
        //同时防止在一个位置上有两个元素，选中之后移除
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }
    void LayoutObjectAtRandom(GameObject[] tileArray,int minmum,int maxmum)   //在地图上随机生成障碍物和食物 
    {
        int objectCount = Random.Range(minmum, maxmum+1);       //要生成的物体个数
        for(int i=0;i<objectCount;i++)
        {
            Vector3 randomPosition = RandomPosition();      //获得一个随机位置
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    void EnemyObjectAtRandom(GameObject[] tileArray, int minmum, int maxmum)   //在地图上随机生成障碍物和食物 
    {
        int objectCount = Random.Range(minmum, maxmum + 1);       //要生成的物体个数
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();      //获得一个随机位置
            int xDir = (int)randomPosition.x;
            int yDir = (int)randomPosition.y;
            book[xDir, yDir] = 2;
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    public void SetupScene(int level)
    {
        BoardSetup();       //首先创建外墙
        InitiaList();       //获得生成敌人/障碍物/食物的区间
        LayoutObjectAtRandom(foodTiles, foodCount.Min, foodCount.Max);
        EnemyObjectAtRandom(wallTiles, wallCount.Min, wallCount.Max);
        int enemyCount = (int)Mathf.Log(level,2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
    
}
