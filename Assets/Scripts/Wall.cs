using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public AudioClip chopSound1;
    public AudioClip chopSound2;
    public Sprite dmgSprite;        //һ���ƻ�ǽ���Ч��
    public int hp = 4;              //ǽ�������ֵ
    private SpriteRenderer spriteRenderer;          //������Ⱦ��
    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void DamageWall(int loss)        //������ͼ��Ч��
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        if (hp <= 0) gameObject.SetActive(false);
    }
    
}
