using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private Vector3 oldPosition;
    private bool isTurn = false;

    private int moveCnt = 0;
    private int turnCnt = 0;
    private int spawnCnt = 0;

    private bool isDie = false;

    private AudioSource sound;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();

        startPosition = transform.position;
        Init();           
    }

    // Update is called once per frame


    private void Init()
    {
        anim.SetBool("Die",false);
        transform.position = startPosition ;
        startPosition = startPosition;
        oldPosition = transform.localPosition;
        moveCnt = 0;
        spawnCnt = 0;
        turnCnt = 0;    
        isTurn = false;
        spriteRenderer.flipX = isTurn;
        isDie = false;  
    }
    public void CharTurn()
    {
        isTurn = isTurn == true ? false : true; 

        spriteRenderer.flipX = isTurn;   
    }
    public void CharMove()
    {
        if (isDie)
            return;

        sound.Play();

        moveCnt++;  

        MoveDirection();   
        
        if(isFailTurn())
        {
            CharDie();           
            return;
        }

        if (moveCnt > 5)
        {
            RespawnStair();
        }

        GameManager.instance.AddScore();
    }

    private void MoveDirection()
    {
        if (isTurn)  //left
        {
            oldPosition += new Vector3(-0.8f, 0.5f, 0);
        }
        else
        {
            oldPosition += new Vector3(0.8f, 0.5f, 0);
        }

        transform.position = oldPosition;
        anim.SetTrigger("Move");
    }

    private bool isFailTurn()
    { 
        bool resurt = false;

        if (GameManager.instance.isTrue[turnCnt] != isTurn)
        {
            resurt = true;
        }
        turnCnt++;

        if(turnCnt > GameManager.instance.Stairs.Length -1)
        {
            turnCnt = 0;
        }
        return resurt;
    }

    private void RespawnStair()
    {
        GameManager.instance.SpawnStair(spawnCnt);
        spawnCnt = 0;
    }

    private void CharDie()
    {
        GameManager.instance.GameOver();
        anim.SetBool("Die",true);
        isDie = true;
    }

    public void ButtonRestart()
    {
        Init();
        GameManager.instance.Init();
        GameManager.instance.InitStairs();
    }
}
