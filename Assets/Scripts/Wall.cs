using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public AudioClip chopSound1;
    public AudioClip chopSound2;
    public Sprite dmgSprite;        //一个破坏墙面的效果
    public int hp = 4;              //墙体的生命值
    private SpriteRenderer spriteRenderer;          //精灵渲染器
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void DamageWall(int loss)        //更换贴图的效果
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        if (hp <= 0) gameObject.SetActive(false);
    }
    
}
