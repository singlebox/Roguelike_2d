using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    //private BoardManager boardmanager=null;
    [HideInInspector] public List<Node> nowList ;      //��ǰ����������ЩԪ��
    private int xStart, yStart;
    private int yEnd, xEnd;
    public void Get(int x1, int y1, int x2, int y2)     //��ʼ��
    {
        xStart = x1;
        yStart = y1;
        //nowList.Add(new Node(x1, y1, 0, 0));
        yEnd = y2;
        xEnd = x2;
    }
    public Node GetMin()       //���ѣ���ѡ��С��
    {                   //���ﲻ���жϣ�ֱ�������ʱ�����ж�
        Node temp = nowList[0];
        //����
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
        bool flag = false;              //ֻϣ���õ�һ��λ��Ȼ�󷵻�
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
    /// �����±꣬�ж��Ƿ���Ҫ����(�Զ�����)
    /// </summary>
    /// <param name="index">��ʾ��Ҫ�������±�</param>
    public void siftdown(int index)        //��Ҫ�������±� ֻ����Զ��ӽ��е���
    {
        int t;
        bool flag = false;                //t��ʾ��Ҫ�������±꣬flag��ʾ�Ƿ���Ҫ��������

        while (index * 2 + 1 < nowList.Count && !flag)            //�����������
        {
            if (nowList[index].F < nowList[index * 2 + 1].F)         //С�������
            {
                t = index;
            }
            else
            {
                if (nowList[index].F == nowList[index * 2 + 1].F)    //��ȣ���һ���ж�
                {
                    if (nowList[index].H <= nowList[index * 2 + 1].H)    //С�ڻ����Ŀ�����
                    {
                        t = index;
                    }
                    else
                    {
                        t = index * 2 + 1;
                    }
                }
                else                                                //���������
                {
                    t = index * 2 + 1;
                }
            }
            if (index * 2 + 2 < nowList.Count)         //�����Ҷ��ӣ������ж�  ��ǰtΪ��С���±�
            {
                if (nowList[t].F <= nowList[index * 2 + 2].F)
                {
                    if (nowList[t].F == nowList[index * 2 + 2].F)        //���ʱ
                    {
                        if (nowList[t].H <= nowList[index * 2 + 2].H)    //С�ڻ����Ŀ�����
                        {
                        }
                        else
                        {
                            t = index * 2 + 2;
                        }
                    }
                    else                    //С��
                    {
                    }
                }
                else                    //�����Ҷ���
                {
                    t = index * 2 + 2;
                }
            }
            if (t != index)                     //˵���ж��ӱ���С
            {
                Swap(index, t);
                index = t;
            }
            else flag = true;                   //û�ж��ӱ���С���򷵻�
        }
    }


    /// <summary>
    /// �����±�Ϊa��b��Ԫ��
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
    /// ��ȡ�ӵ�ǰ�㵽�յ�������پ���
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
