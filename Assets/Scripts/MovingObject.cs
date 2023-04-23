using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour      //�����࣬ʵ���ƶ�
{
    public float moveTime = 0.1f;          //��ʾ�����ƶ���ʱ��
    public LayerMask blockingLayer;        //ͼ����󣬵��ˡ���ǽ����Ҷ���һ��ͼ�㣬���Լ���Ƿ���ײ

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;
    private float inverseMoveTime;          //��߼���Ч��
    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;        //ʹ�ó˷����ǳ��������Ч��
                                                //�����ƶ��ٶ�֮����Խ��г���
    }
    //�ж��Ƿ����ƶ�
    protected bool Move(int xDir,int yDir,out RaycastHit2D hit)   //ʹ��out�߼����������ض��ֵ
    {                                                             //RaycastHit2D �Թ��߽��м�⣬�����ж��Ƿ���ײ
        Vector2 start = transform.position;                       //��ʽ����ת��
        Vector2 end = start + new Vector2(xDir, yDir);            //Ŀ��λ��

        boxCollider.enabled = false;                              //������ʱ����ײ���Լ�
        hit = Physics2D.Linecast(start, end, blockingLayer);      //��start��end֮��Ͷ��һ��ֱ��(��blockingLayer��)
                                                                  //������ײ���Ķ���
        boxCollider.enabled = true;
        if(hit.transform==null)             //˵��û���ϰ�
        {
            StartCoroutine(SmoothMovement(end));            //������ƶ�
            return true;
        }
        return false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component       //����T��ʾ������ײʱ�ĵ�λ
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir,out hit);
        if(hit.transform==null)     //��ʾû����ײ��
        {
            return;
        }
        //������
        T hitComponent = hit.transform.GetComponent<T>();
        if(!canMove&&hitComponent!=null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected IEnumerator SmoothMovement(Vector3 end)       //ʹ�����Э���ƶ�
                                                            //ƽ���ƶ����ƶ��ľ���/�ƶ���ʱ�� ����ÿ��λʱ����ƶ����룬����ĵ�λʱ����ÿ֡�����Կ���ʵ��ƽ���ƶ���Ч��
    {                   
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;       //��ʾ��ǰλ�õ��յ�λ�õ�ƽ��
        while(sqrRemainingDistance>float.Epsilon)       //float.Epsilon��ʾС��0����С����������Ϊʵ����ʹ�ø���������������ʹ��Epsilon���бȽ�
        {   //��ʾ��rb2d.position��end�����ƶ����ƶ��ľ���ΪinverseMoveTime * Time.deltaTime���ƶ�֮���λ�ý��з���
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);         //ʹ��������ƶ������λ��
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;        //���¼������
            yield return null;      //���´�ִ��ǰ�ȴ�һ֡
        }
    }
    //�����������ִ�и÷���
    protected abstract void OnCantMove<T> (T component) where T:Component;       //���ͳ��󷽷����޶�����ΪComponen������
    
}
