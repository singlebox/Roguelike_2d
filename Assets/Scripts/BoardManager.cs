using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
[Serializable]
public class BoardManager : MonoBehaviour       //�������ɵ�ͼ
{
    public class Count      //�������ɵ�ͼ�ϵ�ĳ��Ԫ�ص�����
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
    public static int columns = 8;     //��ͼ����
    public static int rows=8;          //��ͼ����
    public Count wallCount = new Count(5, 9);       //��ͼ��ǽ�������
    public Count foodCount = new Count(1, 5);       //��ͼ�ϵĲ�������
    public GameObject exit;                         //���ڶ���
    public GameObject[] floorTiles;                 //�洢�ϰ������
    public GameObject[] foodTiles;                  //�洢����food����
    public GameObject[] wallTiles;                  //�洢ǽ��
    public GameObject[] enemyTiles;                 //�洢����
    public GameObject[] outerWallTiles;             //�洢��ǽ    
    //[HideInInspector] 
    public int[,] book;           //���б�� 
    private Transform boardHolder;  //ʹ������ͼ���� ����������������ΪboardHolder���Ӷ��󣬿����۵�����
    private List<Vector3> gridPositions = new List<Vector3>();      //��¼��ͼ�ϵ�λ�ã��Լ���λ���Ƿ�������

    void InitiaList()           //��ʼ������
    {
        gridPositions.Clear();      //���������
        for(int i=1;i<columns-1;i++)
        {
            for(int j=1;j<rows-1;j++)
            {
                gridPositions.Add(new Vector3(i, j, 0f));       //������������ɵ���/�ϰ���/����
            }
        }
    }

    void BoardSetup()       //������ǽ�͵ذ�
    {
        book = new int[columns, rows];
        boardHolder = new GameObject("Board").transform;
        for (int i = -1; i < columns+1; i++)
        {
            for (int j = -1; j < rows + 1; j++)
            {
                GameObject toInstantiate;
                if (i == -1 || j == -1 || i == columns  || j == rows )
                {       //�������һ����ǽ
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                else
                {       //�������һ������
                    book[i, j] = 1;
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }
                //�Ѿ�ȷ����Ҫ���ɵ�����
                //ʵ�������صĶ�����Object���ͣ���Ҫ����ת��
                GameObject instance = Instantiate(toInstantiate, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }   
            }
    }

    Vector3 RandomPosition()        //���һ�����λ��
    {
        int randomIndex = Random.Range(0, gridPositions.Count);     //���һ������±�
        Vector3 randomPosition = gridPositions[randomIndex];
        //ͬʱ��ֹ��һ��λ����������Ԫ�أ�ѡ��֮���Ƴ�
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }
    void LayoutObjectAtRandom(GameObject[] tileArray,int minmum,int maxmum)   //�ڵ�ͼ����������ϰ����ʳ�� 
    {
        int objectCount = Random.Range(minmum, maxmum+1);       //Ҫ���ɵ��������
        for(int i=0;i<objectCount;i++)
        {
            Vector3 randomPosition = RandomPosition();      //���һ�����λ��
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    void EnemyObjectAtRandom(GameObject[] tileArray, int minmum, int maxmum)   //�ڵ�ͼ����������ϰ����ʳ�� 
    {
        int objectCount = Random.Range(minmum, maxmum + 1);       //Ҫ���ɵ��������
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();      //���һ�����λ��
            int xDir = (int)randomPosition.x;
            int yDir = (int)randomPosition.y;
            book[xDir, yDir] = 2;
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    public void SetupScene(int level)
    {
        BoardSetup();       //���ȴ�����ǽ
        InitiaList();       //������ɵ���/�ϰ���/ʳ�������
        LayoutObjectAtRandom(foodTiles, foodCount.Min, foodCount.Max);
        EnemyObjectAtRandom(wallTiles, wallCount.Min, wallCount.Max);
        int enemyCount = (int)Mathf.Log(level,2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
    
}
