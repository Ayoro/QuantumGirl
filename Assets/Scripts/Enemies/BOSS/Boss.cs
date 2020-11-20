using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    //电系充能的判定分别在160 110
    public GameObject Player;
    public GameObject FireBall;
    public GameObject preFlash;
    public GameObject Flash;
    public GameObject LaserLine;
    public GameObject LaserBall;

    public float Speed=1;
    public float health = 500;//血量

    public AudioClip fireAudio;
    public AudioClip LaserBallExp;//爆炸声
    public AudioClip LaserBallCha;//充能声
    public AudioClip FlashAudio;//闪电声


    private float BigFlashCd=0f;//连续型闪电技能cd 20/15/10
    private float FlashCd = 0f;//小闪电cd 15/12/8
    private int i = 1;
    private int finalChargeNum=0;//大招的充能次数
    private bool isFinalSkilling = false;//放大的时候不放其他技能

    private Animator animatior;

    private float FireCd=0f;//火球技能cd 18/15/12
    private float LaserCd=45f;//激光cd 90/60/40 默认45s为了让boss第一次放快一点
    private float dis;//距离
    //bool isAttacking = false;

    private Vector2 FlashPos1;
    private Vector2[] FlashPos2 = new Vector2[12];
    private Vector2 offset;//相对距离

    private RaycastHit2D rayhit1, rayhit2;//普通的闪电攻击

    void Start()
    {
        animatior = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        FlashCd += Time.deltaTime;
        LaserCd += Time.deltaTime;
        offset = Player.transform.position - transform.position;
        if (offset.x < 10)
        {
            BigFlashCd += Time.deltaTime;
        }
        else if (offset.x >= 10)
        {
            FireCd += Time.deltaTime;
        }//cd计时
        if (offset.x >= 15 && !isFinalSkilling)//距离15格且不在放大的时候
        {
            GetComponent<Rigidbody2D>().MovePosition(Vector2.Lerp(transform.position,transform.position+ new Vector3(1,0,0), Speed * Time.deltaTime));
        }//移动
        if (offset.x <= 10f)
        {
            MovePlayerAway();
        }
        if (health >= 0.7f* health && !isFinalSkilling)
        {
            AttackMode1();
        }
        else if (health >= 0.3* health && health < 0.7* health && !isFinalSkilling)
        {
            AttackMode2();
        }
        else if (health < 0.3* health && !isFinalSkilling)
        {
            AttackMode3();
        }
    }
    public void FlashAttack(float Damege, float AttackLength)//闪电的攻击方法,可将攻击力和攻击距离作为参数传递
    {
        FlashPos1 = Player.transform.position + new Vector3(0, 3.75f, 0);//记录玩家现在的位置为闪电开始点
        Instantiate(preFlash, FlashPos1, Quaternion.identity);//创建闪电前摇提醒玩家闪电落点
        rayhit1 = Physics2D.Raycast(FlashPos1, new Vector2(0, -10f));
        if (rayhit1)
        {
            dis = Mathf.Abs(rayhit1.transform.position.y - FlashPos1.y);
            StartCoroutine(Flash1(Damege));//进行伤害判定
        }
        FlashCd = 0f;
    }
    IEnumerator Flash1(float Damege)//敌人的攻击力作参数
    {
        yield return new WaitForSeconds(1.5f);//从闪电开始点进行射线扫描
        rayhit2 = Physics2D.Raycast(FlashPos1, new Vector2(0, -10f));
        FlashPos1 = FlashPos1 + new Vector2(0, -rayhit2.distance +3.75f);
        Instantiate(Flash, FlashPos1, Quaternion.identity);//在之前闪电前摇的落点处创建落雷动画
        animatior.SetTrigger("FlashAttack");
        Debug.Log("打到了" + rayhit2.collider);
        if (rayhit2)//开始进行伤害判定
        {
            if (rayhit2.transform.tag == "Player")
            {
                Debug.Log("玩家掉血-" + Damege);
            }
            else if (rayhit2.transform.tag == "AbsorbCube")
            {
                Debug.Log("吸收能量+" + Damege);
            }
        }
    }

    IEnumerator ContinuousFlash(float Damege)
    {
        yield return new WaitForSeconds(0.3f);
        BigFlashAttack(Damege);
        if (i == 11)
        {
            animatior.SetTrigger("BigFlashAttackOver");
        }
        if (i <= 10)
        {
            StartCoroutine(ContinuousFlash(Damege));
        }
    }//连环闪电递归
    public void BigFlashAttack(float Damege)
    {

        FlashPos2[i] = transform.position + new Vector3(5 + i, -2, 0);//boos前的6.5格开始
        //Debug.Log(FlashPos2[i]);
        Instantiate(preFlash, FlashPos2[i], Quaternion.identity);//创建闪电前摇提醒玩家闪电落点
        StartCoroutine(Flash2(Damege,i));//进行伤害判定
        i += 1;
    }

    IEnumerator Flash2(float Damege,int i)//敌人的攻击力作参数
    {
        //Vector2 FlashPos = FlashPos2[i];
        yield return new WaitForSeconds(1.5f);//从闪电开始点进行射线扫描
        RaycastHit2D rayhitB = Physics2D.Raycast(FlashPos2[i], new Vector2(0, -10f));
        Debug.Log(FlashPos2[i]);
        Debug.Log("rayhitB:"+rayhitB.transform.position);
        FlashPos2[i] = FlashPos2[i] + new Vector2(0, -rayhitB.distance) + new Vector2(0, 3.75f);
        Instantiate(Flash, FlashPos2[i], Quaternion.identity);//在之前闪电前摇的落点处创建落雷动画
        if (i == 1)
        {
            animatior.SetTrigger("BigFlashAttack");
        }
        if (rayhitB)//开始进行伤害判定
        {
            if (rayhitB.transform.tag == "Player")
            {
                Player.GetComponent<Player>().player_health -= Damege;
                Debug.Log("玩家掉血-" + Damege);
            }
            else if (rayhitB.transform.tag == "AbsorbCube")
            {
                Debug.Log("吸收能量+" + Damege);
            }
        }
    }

    public void FireAttack(int num)
    {
        StartCoroutine(IniFireBall(num));
    }
    IEnumerator IniFireBall(int num)
    {
        yield return new WaitForSeconds(0.8f);
        animatior.SetTrigger("FireAttack");
        Instantiate(FireBall, transform.position + new Vector3(3.7f, 0.3f, 0), Quaternion.identity);
        AudioManager.Instance.AudioPlay(fireAudio, 2);
        //动画
        i += 1;
        if (i <= num)
        {
            StartCoroutine(IniFireBall(num));
        }
    }

    public void FinalSkill(int chargeNum)//爆炸的伤害和充能判定次数 充能判定次数越高爆炸伤害越高
    {
        isFinalSkilling = true;
        GameObject laserline= Instantiate(LaserLine, transform.position + new Vector3(4.3f, -1.1f, 0), Quaternion.identity);//条形激光.只是一个动画
        GameObject laserball= Instantiate(LaserBall, transform.position + new Vector3(7.2f, -3.7f, 0), Quaternion.identity);//终爆炸的能量球
        animatior.SetTrigger("LaserAttack");
        laserball.transform.Rotate(new Vector3(0, 0, 45*Time.deltaTime));
        StartCoroutine(FinalSkillCharge(chargeNum,laserball,laserline));
    }

    IEnumerator FinalSkillCharge(int chargeNum,GameObject laserball,GameObject laserline)
    {
        if (i <chargeNum+1 )
        {
            AudioManager.Instance.AudioPlay(LaserBallCha, 1);
            yield return new WaitForSeconds(1);
            GetComponent<PolygonCollider2D>().enabled = false;
            RaycastHit2D laserhit = Physics2D.Linecast(transform.position + new Vector3(4.3f, -1.1f, 0), laserball.transform.position);
            GetComponent<PolygonCollider2D>().enabled = true;
            Debug.Log(laserhit.collider);
            if (laserhit.collider.tag == "LaserBall")
            {
                finalChargeNum += 1;
                laserball.transform.localScale = LaserBall.transform.localScale *(1+ 0.3f * finalChargeNum);
            }
            if (laserhit.collider.tag == "energy_block")
            {
                Debug.Log("能量块抵消了一次充能");
            }
            i += 1;
            if (i <= chargeNum+1)
            {
                StartCoroutine(FinalSkillCharge(chargeNum, laserball, laserline));
            }
        }
        else
        {
            Destroy(laserball);
            AudioManager.Instance.AudioPlay(LaserBallExp, 1);
            Destroy(laserline);
            animatior.SetTrigger("LaserAttackOver");
            isFinalSkilling = false;
            Debug.Log("造成了" + finalChargeNum * 10 + "点伤害");
            finalChargeNum = 0;
        }
    }

    void MovePlayerAway()
    {
        Player.GetComponent<Rigidbody2D>().AddForce(new Vector3(1,0,0)*200);
    }

    void AttackMode1()
    {
        
        if (FlashCd >= 15 && offset.x <= 13)//小闪电攻击 13为第一个阶段攻击距离(13/17/20).第一阶段cd15(15/12/8)
        {
            Debug.Log("小闪电攻击");
            FlashAttack(10, 13);//10为攻击力(10/15/20) 13为攻击距离
        }
        else if (BigFlashCd >= 20)//连环闪电cd 20/15/10
        {
            StartCoroutine(ContinuousFlash(10));//10为伤害(10/15/20)
            Debug.Log("连续闪电攻击");
            i = 1;
            BigFlashCd = 0f;
        }
        else if (FireCd >= 18)//火球cd18/15/12
        {
            FireAttack(3);//第一阶段吐3个球3/5/7
            Debug.Log("火焰攻击");
            i = 1;
            FireCd = 0f;
        }
        else if (LaserCd >= 50)//大招cd90/60/40
        {
            FinalSkill(5);//最大充能数5/7/9
            Debug.Log("激光攻击");
            i = 1;
            LaserCd = 0;
        }
    }

    void AttackMode2()
    {

        if (FlashCd >= 12 && offset.x <= 17)//小闪电攻击 17为第二个阶段攻击距离(13/17/20).第二阶段12cd(15/12/8)
        {
            Debug.Log("小闪电攻击");
            FlashAttack(15, 17);//10为攻击力
        }
        else if (BigFlashCd >= 15)//20/15/10
        {
            StartCoroutine(ContinuousFlash(15));//每个闪电的伤害10/15/20
            Debug.Log("连续闪电攻击");
            i = 1;
            BigFlashCd = 0f;
        }
        else if (FireCd >= 15)//18/15/12
        {
            FireAttack(5);//第二阶段吐5个球
            Debug.Log("火焰攻击");
            i = 1;
            FireCd = 0f;
        }
        else if (LaserCd >= 60)//90/60/40
        {
            FinalSkill(7);//5/7/9
            Debug.Log("激光攻击");
            i = 1;
            LaserCd = 0;
        }
    }

    void AttackMode3()
    {

        if (FlashCd >= 8 && offset.x <= 20)//小闪电攻击 20为第三个阶段攻击距离(13/17/20).第三阶段12cd(15/12/8)
        {
            Debug.Log("小闪电攻击");
            FlashAttack(20, 20);//10为攻击力
        }
        else if (BigFlashCd >= 10)//20/15/10
        {
            StartCoroutine(ContinuousFlash(20));//每个闪电的伤害10/15/20
            Debug.Log("连续闪电攻击");
            i = 1;
            BigFlashCd = 0f;
        }
        else if (FireCd >= 12)//18/15/12
        {
            FireAttack(7);//第三阶段吐7个球
            Debug.Log("火焰攻击");
            i = 1;
            FireCd = 0f;
        }
        else if (LaserCd >= 40)//90/60/40
        {
            FinalSkill(9);//5/7/9
            Debug.Log("激光攻击");
            i = 1;
            LaserCd = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Player"&&collision.transform.tag!="ground"&&collision.transform.tag!="Enemy")
        {
            Destroy(collision.gameObject);
        }
    }
}


