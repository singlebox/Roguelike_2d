using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour      //抽象类，实现移动
{
    public float moveTime = 0.1f;          //表示对象移动的时间
    public LayerMask blockingLayer;        //图层对象，敌人、外墙、玩家都在一个图层，可以检测是否碰撞

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;
    private float inverseMoveTime;          //提高计算效率
    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;        //使用乘法而非除法来提高效率
                                                //控制移动速度之后可以进行尝试
    }
    //判断是否能移动
    protected bool Move(int xDir,int yDir,out RaycastHit2D hit)   //使用out高级参数，返回多个值
    {                                                             //RaycastHit2D 对光线进行检测，用来判断是否碰撞
        Vector2 start = transform.position;                       //隐式类型转换
        Vector2 end = start + new Vector2(xDir, yDir);            //目标位置

        boxCollider.enabled = false;                              //不禁用时会碰撞到自己
        hit = Physics2D.Linecast(start, end, blockingLayer);      //在start到end之间投射一条直线(在blockingLayer层)
                                                                  //返回碰撞到的对象
        boxCollider.enabled = true;
        if(hit.transform==null)             //说明没有障碍
        {
            StartCoroutine(SmoothMovement(end));            //则进行移动
            return true;
        }
        return false;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component       //泛型T表示发生碰撞时的单位
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir,out hit);
        if(hit.transform==null)     //表示没有碰撞到
        {
            return;
        }
        //碰到了
        T hitComponent = hit.transform.GetComponent<T>();
        if(!canMove&&hitComponent!=null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected IEnumerator SmoothMovement(Vector3 end)       //使用这个协程移动
                                                            //平滑移动，移动的距离/移动的时间 等于每单位时间的移动距离，这里的单位时间是每帧，所以可以实现平滑移动的效果
    {                   
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;       //表示当前位置到终点位置的平方
        while(sqrRemainingDistance>float.Epsilon)       //float.Epsilon表示小于0的最小浮点数，因为实际上使用浮点数会有误差，所有使用Epsilon进行比较
        {   //表示从rb2d.position向end方向移动，移动的距离为inverseMoveTime * Time.deltaTime，移动之后的位置进行返回
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);         //使刚体对象移动到这个位置
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;        //重新计算距离
            yield return null;      //在下次执行前等待一帧
        }
    }
    //如果有碰到则执行该方法
    protected abstract void OnCantMove<T> (T component) where T:Component;       //泛型抽象方法，限定参数为Componen的子类
    
}
