using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private int x, y;       //��ʾ����
    private int g, h, f;    //g��ʾ��ǰ������ۣ�h��ʾĿ����ۣ�f��ʾ����֮��
    public int X
    {
        get { return x; }
        set { x = value; }
    }
    public int Y
    {
        get { return y; }
        set { y = value; }
    }
    public int G
    {
        get { return g; }
        set { g = value; }
    }
    public int H
    {
        get { return h; }
        set { h = value; }
    }
    public int F
    {
        get { return f; }
        set { f = value; }
    }
    public Node(int x, int y, int g, int h)
    {
        this.X = x;
        this.Y = y;
        this.G = g;
        this.H = h;
        this.F = G + H;
    }
}
