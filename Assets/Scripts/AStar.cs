using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    //private BoardManager boardmanager=null;
    [HideInInspector] public List<Node> nowList ;      //当前队列中有哪些元素
    private int xStart, yStart;
    private int yEnd, xEnd;
    public void Get(int x1, int y1, int x2, int y2)     //初始化
    {
        xStart = x1;
        yStart = y1;
        //nowList.Add(new Node(x1, y1, 0, 0));
        yEnd = y2;
        xEnd = x2;
    }
    public Node GetMin()       //出堆，挑选最小的
    {                   //这里不用判断，直接在入堆时进行判断
        Node temp = nowList[0];
        //出堆
        int max = nowList.Count - 1;
        nowList[0] = nowList[max];
        nowList.RemoveAt(max);
        if (max == 0)
            return temp;
        siftdown(0);

        return temp;
    }
    public void Next(int[,] book,int x,int y,out int xDir,out int yDir)
    {
        nowList = new List<Node>();
        nowList.Add(new Node(x, y, 0, 0));
        bool flag = false;              //只希望得到一个位置然后返回
        while(nowList.Count>=0)
        {
            Node temp = GetMin();
            if(flag==true)
            {
                xDir = temp.X;
                yDir = temp.Y;
                return;
            }
            flag = true;
            if (temp.X >= 0 && temp.X < BoardManager.columns && temp.Y + 1 < BoardManager.rows && temp.Y + 1 >= 0)
            {
                if (book[temp.X, temp.Y + 1] == 1)
                {
                    nowList.Add(new Node(temp.X, temp.Y + 1, temp.G + 1, MhD(temp.X, temp.Y + 1)));
                    //book[temp.X, temp.Y + 1] = 3;
                }
            }
            if (temp.X + 1 < BoardManager.columns && temp.X + 1 >= 0 && temp.Y >= 0 && temp.Y < BoardManager.rows)
            {
                if (book[temp.X + 1, temp.Y] == 1)
                {
                    nowList.Add(new Node(temp.X + 1, temp.Y, temp.G + 1, MhD(temp.X + 1, temp.Y)));
                    //book[temp.X + 1, temp.Y] = 3;
                }
            }
            if (temp.X >= 0 && temp.X < BoardManager.columns && temp.Y - 1 < BoardManager.rows && temp.Y - 1 >= 0)
            {
                if (book[temp.X, temp.Y - 1] == 1)
                {
                    nowList.Add(new Node(temp.X, temp.Y - 1, temp.G + 1, MhD(temp.X, temp.Y - 1)));
                    //book[temp.X, temp.Y - 1] = 3;
                }
            }
            if (temp.X - 1 < BoardManager.columns && temp.X - 1 >= 0 && temp.Y >= 0 && temp.Y < BoardManager.rows)
            {
                if (book[temp.X - 1, temp.Y] == 1)
                {
                    nowList.Add(new Node(temp.X - 1, temp.Y, temp.G + 1, MhD(temp.X - 1, temp.Y)));
                    //book[temp.X - 1, temp.Y] = 3;
                }
            }
            for (int i = nowList.Count / 2; i >= 0; i--)
            {
                siftdown(i);
            }
        }
        xDir = 0;yDir = 0;
    }
    /// <summary>
    /// 传入下标，判断是否需要调整(自定向下)
    /// </summary>
    /// <param name="index">表示需要调整的下标</param>
    public void siftdown(int index)        //需要调整的下标 只能针对儿子进行调整
    {
        int t;
        bool flag = false;                //t表示需要交换的下标，flag表示是否还需要继续调整

        while (index * 2 + 1 < nowList.Count && !flag)            //至少有左儿子
        {
            if (nowList[index].F < nowList[index * 2 + 1].F)         //小于左儿子
            {
                t = index;
            }
            else
            {
                if (nowList[index].F == nowList[index * 2 + 1].F)    //相等，进一步判断
                {
                    if (nowList[index].H <= nowList[index * 2 + 1].H)    //小于或等于目标代价
                    {
                        t = index;
                    }
                    else
                    {
                        t = index * 2 + 1;
                    }
                }
                else                                                //大于左儿子
                {
                    t = index * 2 + 1;
                }
            }
            if (index * 2 + 2 < nowList.Count)         //存在右儿子，进行判断  当前t为更小的下标
            {
                if (nowList[t].F <= nowList[index * 2 + 2].F)
                {
                    if (nowList[t].F == nowList[index * 2 + 2].F)        //相等时
                    {
                        if (nowList[t].H <= nowList[index * 2 + 2].H)    //小于或等于目标代价
                        {
                        }
                        else
                        {
                            t = index * 2 + 2;
                        }
                    }
                    else                    //小于
                    {
                    }
                }
                else                    //大于右儿子
                {
                    t = index * 2 + 2;
                }
            }
            if (t != index)                     //说明有儿子比他小
            {
                Swap(index, t);
                index = t;
            }
            else flag = true;                   //没有儿子比他小，则返回
        }
    }


    /// <summary>
    /// 交换下标为a和b的元素
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    void Swap(int a, int b)
    {
        Node temp = nowList[a];
        nowList[a] = nowList[b];
        nowList[b] = temp;
    }
    /// <summary>
    /// 获取从当前点到终点的曼哈顿距离
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    public int MhD(int x1, int y1)
    {
        return Mathf.Abs(x1 - xEnd) + Mathf.Abs(y1 - yEnd);
    }
}
