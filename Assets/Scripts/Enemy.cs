using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;
    public int playerDamage;
    private Animator animator;
    private Transform target;
    private bool skipMove=false;
    // Start is called before the first frame update
    protected override void Start()
    {
        GameMangaer.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if(skipMove)
        {
            skipMove = false;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }
    public void MoveEnemy(int[,] book)
    {
        int xDir = 0;int yDir = 0;
        AStar astar = new AStar();
        astar.Get((int)transform.position.x, (int)transform.position.y, (int)target.position.x, (int)target.position.y);
        astar.Next(book,(int)transform.position.x, (int)transform.position.y,out xDir,out yDir);
        AttemptMove<Player>(xDir - (int)transform.position.x, yDir - (int)transform.position.y);
    }
   


    protected override void OnCantMove<T>(T component)
    {
        animator.SetTrigger("enemyAttack");
        Player hitPlayer = component as Player;
        hitPlayer.LoseFood(playerDamage);
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }
}
