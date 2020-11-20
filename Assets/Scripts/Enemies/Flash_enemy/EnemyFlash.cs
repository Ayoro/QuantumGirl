using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlash : MonoBehaviour {
    public GameObject Player;
    public GameObject Flash;//闪电
    public GameObject preFlash;//闪电前摇动画
    public float health = 30;//怪物血量
    public float Damege = 10.0f;//伤害数值
    private float CD = 5f;//技能cd
    private float AttackLength=18.0f;//攻击距离
    private Animator animator;//动画
    private Vector2 FlashPos;//闪电开始位置
    private float dis;//闪电长度
    private Vector2 offset;//敌人和玩家的相对位置
    private float x = -1;//角色朝向-1朝左边


    RaycastHit2D rayhit1, rayhit2;

    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }
	void Update () {
        offset = Player.transform.position - transform.position;
        x = (Player.transform.position.x - transform.position.x) / Mathf.Abs(Player.transform.position.x - transform.position.x);
        transform.rotation = Quaternion.Euler(0, (x + 1) * 90, 0);//改变角色朝向.
        if (Mathf.Abs(offset.x) < AttackLength && CD >= 5)//攻击距离之内进行攻击,先有0.5s的落雷前摇再落雷
        {
            FlashAttack(Damege, AttackLength);
        }

        CD += Time.deltaTime;
    }

    public void FlashAttack(float Damege,float AttackLength)//闪电的攻击方法,可将攻击力和攻击距离作为参数传递
    {
            FlashPos = Player.transform.position + new Vector3(0, 3.75f, 0);//记录玩家现在的位置为闪电开始点
            //animator.SetTrigger("Attack");
            Instantiate(preFlash, FlashPos, Quaternion.identity);//创建闪电前摇提醒玩家闪电落点
            rayhit1 = Physics2D.Raycast(FlashPos, new Vector2(0, -10f));
            StartCoroutine(Example(Damege));//进行伤害判定
            CD = 0f;
    }
     IEnumerator Example(float Damege)//敌人的攻击力作参数
    {
        yield return new WaitForSeconds(1.5f);//从闪电开始点进行射线扫描
        rayhit2 = Physics2D.Raycast(FlashPos, new Vector2(0, -10f));
        FlashPos = FlashPos + new Vector2(0, -rayhit2.distance) + new Vector2(0, 3.75f);
        Instantiate(Flash, FlashPos, Quaternion.identity);//在之前闪电前摇的落点处创建落雷动画
        animator.SetTrigger("Attack");
        Debug.Log("打到了"+rayhit2.collider);
        if (rayhit2)//开始进行伤害判定
        {
            if (rayhit2.transform.tag=="Player")
            {
                rayhit2.transform.GetComponent<Player>().player_health -= Damege;
                rayhit2.transform.GetComponent<Player>().Flash(Damege);
            }
            else if (rayhit2.transform.tag == "energy_block")
            {
                rayhit2.transform.GetComponent<energy_block>().energy += Damege;
                rayhit2.transform.GetComponent<energy_block>().energy_type = 1;
            }
        }
    }
    /*public void HpCheck()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        hit=Physics2D.
    }*/
}
