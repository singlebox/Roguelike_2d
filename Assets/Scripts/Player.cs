using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    public Text foodText;
    int wallDamage=1;       //对墙体造成的伤害
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    private Animator animator;
    private int food=100;
    // Start is called before the first frame update
    protected override void Start()
    {
        foodText.text = "Food:" + food;
        animator = GetComponent<Animator>();
        food = GameMangaer.instance.playerFoodPoints;       //单例模式，静态对象通过类名方式调用
        base.Start();
    }

    private void OnDisable()        //当player脚本被禁用时调用，保存玩家的food值
    {
        if(Button.flag==false)
        GameMangaer.instance.playerFoodPoints = food;
        else
        {
            GameMangaer.instance.playerFoodPoints = 100;
            Button.flag = false;
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = "Food:" + food;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if(Move(xDir,yDir,out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1,moveSound2);
        }
        CheckIfGameOver();
        GameMangaer.instance.PlayerTurn = false;
    }

    private void CheckIfGameOver()
    {
        if(food<=0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameMangaer.instance.GameOver();
        }
    }

    protected override void OnCantMove<T>(T component)          //对碰撞到的墙进行处理
    {
        Wall hitWall = component as Wall;                       //类型转换
        hitWall.DamageWall(wallDamage);                         //对墙进行攻击
        animator.SetTrigger("playerChop");                      //触发条件
    }

    private void Restart()          //进入下一关之后重新加载场景
    {
        SceneManager.LoadScene("MyScene");
        GameMangaer.instance.level++;
    }

    private void OnTriggerEnter2D(Collider2D other)     //判断碰到的物体
    {
        if(other.tag=="Exit")
        {
            Invoke("Restart", restartLevelDelay);       //表示延迟之后调用Restart
            enabled = false;                            //进入下一个关卡，所以禁用玩家                            
        }
        else if(other.tag=="Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food:" + food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag=="Soda")
        {
            food += pointsPerSoda;
            foodText.text ="+"+pointsPerSoda+ " Food:" + food;
            SoundManager.instance.RandomizeSfx(drinkSound1,drinkSound2);
            other.gameObject.SetActive(false);
        }

    }

    public void LoseFood(int loss)          //减少食物
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food:" + food;
        CheckIfGameOver();
    }
    // Update is called once per frame
    void Update()
    {
        if(!GameMangaer.instance.PlayerTurn)
        {
            return;
        }
        int horizontal = 0;         //x轴
        int vertical = 0;           //y轴
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        //if(horizontal==0)
        vertical = (int)Input.GetAxisRaw("Vertical");
        if ( vertical!= 0)            //避免对角线移动
            horizontal = 0;                //不需要他们同时出现，所以直接判断一次即可
        if(horizontal!=0||vertical!=0)
        {
            AttemptMove<Wall>(horizontal, vertical);

        }
    }
}
