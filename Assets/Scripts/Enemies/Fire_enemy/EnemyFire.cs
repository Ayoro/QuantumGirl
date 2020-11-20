using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {
    public GameObject Player;
    public GameObject FireBall;//火球
    private GameObject obj;
    

    public float Speed;//移动速度默认1
    public float health;

    private float checkTime = 0;//每两秒发射一次射线检查主角是否进入追逐范围
    private float checkTime2 = 0;//每1秒检测是否撞墙
    private float CD = 5;//技能cd
    private float dis;//主角和敌人的距离
    private float x = -1;//敌人得移动距离,x=-1向左走

    private Animator animator;

    private bool isFind = false;//是否发现主角

    private Vector2 OriPos;//敌人出现的位置
    private Vector2 offset;//相对位置
    private Vector3 offsetcheck;

    private Quaternion quaternion;

    int layermask;


    //private Quaternion roration;

    RaycastHit2D rayhit1, hit;//rayhit1检查主角是否进入范围,hit用于上升中检查前方
    void Start () {
        animator = GetComponent<Animator>();
        OriPos = transform.position;//记录生成位置
        Player = GameObject.FindGameObjectWithTag("Player");
        
    }
	
	void Update () {
        checkTime += Time.deltaTime;
        checkTime2 += Time.deltaTime;
        CD += Time.deltaTime;
        offset = Player.transform.position - transform.position;
        dis = Mathf.Sqrt((offset).sqrMagnitude);
        layermask = 1 << 8;
        if (checkTime >= 1 && isFind == false&&dis<=30f)//寻找主角
        {
                GetComponent<PolygonCollider2D>().enabled = false;
                rayhit1 = Physics2D.Raycast(transform.position, Player.transform.position - transform.position,30);
                GetComponent<PolygonCollider2D>().enabled = true;
                Debug.DrawRay(transform.position, Player.transform.position - transform.position);
                Debug.Log(rayhit1.collider);
                checkTime = 0;
                if (rayhit1)
                {
                    if (rayhit1.transform.tag == "Player")
                    {
                    Debug.Log(1);    
                    isFind = true;
                    }
                }  
        }
        if (!isFind)
        {
            MoveFree();
            //Debug.Log("free");
        }
        else
        {
            Move2Player();
            //Debug.Log("unfree");
        }
        if (isFind && dis <= 20f&&CD>=4)
        {
            FireAttack();
            CD = 0;
        }
	}
    public void MoveFree()//没找到主角前的移动方式
    {
        transform.rotation = Quaternion.Euler(0, (x + 1) * 90, 0);//改变角色朝向.
        GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, transform.position + new Vector3(x,0,0), Speed * Time.deltaTime));
        if (checkTime2 >= 1)
        {
            CubeCheckup();//检查障碍,有就飞过去
            checkTime2 = 0;
        }
        if (transform.position.x - OriPos.x <= -5f)
        {
            x = 1;
        }
        if (transform.position.x - OriPos.x >= 5f)
        {
            x = -1;
        }
    }

    public void Move2Player()
    {
        x = (Player.transform.position.x - transform.position.x) / Mathf.Abs(Player.transform.position.x - transform.position.x);
        transform.rotation = Quaternion.Euler(0, (x + 1) * 90, 0);//改变角色朝向.
        offsetcheck = (Player.transform.position - transform.position) / dis;
        if (dis >= 20f)//再五格以外一直追玩家
        {
            //GetComponent<Rigidbody2D>().velocity = offsetcheck * Speed;
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, transform.position + offsetcheck, Speed * Time.deltaTime));
        }
        if(dis < 15f)//3格以内保持距离
        {
            //GetComponent<Rigidbody2D>().velocity = offsetcheck * -Speed;
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, transform.position -offsetcheck, Speed * Time.deltaTime));
        }
        if (checkTime2 >= 1)
        {
            if (Player.transform.position.y - transform.position.y >= 0)
            {
                CubeCheckup();
            }
            if (Player.transform.position.y - transform.position.y <= 0)
            {
                CubeCheckdown();
            }

            checkTime2 = 0;
        }
    }
    public void CubeCheckup()//检查前进路上得方块,如果有就跳跃
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        hit = Physics2D.Linecast(transform.position, transform.position+new Vector3(x,0,0));
        GetComponent<PolygonCollider2D>().enabled = true;
        //Debug.Log("hit"+hit.collider);
        if (hit.collider)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, transform.position + new Vector3(0, 150*hit.transform.localScale.y, 0), Speed * Time.deltaTime));//跳跃
        }
    }

    public void CubeCheckdown()//检查前进路上的方块,如果有就下降
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        hit = Physics2D.Linecast(transform.position, transform.position + new Vector3(x, 0, 0));
        GetComponent<PolygonCollider2D>().enabled = true;
        Debug.Log(hit.collider);
        if (hit.collider)
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position, transform.position + new Vector3(0, -150 * hit.transform.localScale.y, 0), Speed * Time.deltaTime));//跳跃
        }
    }


    public void FireAttack()
    {
        animator.SetTrigger("Attack");
        Instantiate(FireBall, transform.position+new Vector3(x,0,0), Quaternion.identity);
    }
}
