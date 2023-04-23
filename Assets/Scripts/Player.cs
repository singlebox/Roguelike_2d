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
    int wallDamage=1;       //��ǽ����ɵ��˺�
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
        food = GameMangaer.instance.playerFoodPoints;       //����ģʽ����̬����ͨ��������ʽ����
        base.Start();
    }

    private void OnDisable()        //��player�ű�������ʱ���ã�������ҵ�foodֵ
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

    protected override void OnCantMove<T>(T component)          //����ײ����ǽ���д���
    {
        Wall hitWall = component as Wall;                       //����ת��
        hitWall.DamageWall(wallDamage);                         //��ǽ���й���
        animator.SetTrigger("playerChop");                      //��������
    }

    private void Restart()          //������һ��֮�����¼��س���
    {
        SceneManager.LoadScene("MyScene");
        GameMangaer.instance.level++;
    }

    private void OnTriggerEnter2D(Collider2D other)     //�ж�����������
    {
        if(other.tag=="Exit")
        {
            Invoke("Restart", restartLevelDelay);       //��ʾ�ӳ�֮�����Restart
            enabled = false;                            //������һ���ؿ������Խ������                            
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

    public void LoseFood(int loss)          //����ʳ��
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
        int horizontal = 0;         //x��
        int vertical = 0;           //y��
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        //if(horizontal==0)
        vertical = (int)Input.GetAxisRaw("Vertical");
        if ( vertical!= 0)            //����Խ����ƶ�
            horizontal = 0;                //����Ҫ����ͬʱ���֣�����ֱ���ж�һ�μ���
        if(horizontal!=0||vertical!=0)
        {
            AttemptMove<Wall>(horizontal, vertical);

        }
    }
}
