using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flash : MonoBehaviour
{
    public GameObject Player;
    public GameObject Flash;//闪电
    public GameObject preFlash;//闪电前摇动画
    public bool atk_mode = false;
    RaycastHit2D rayhit1, rayhit2;
    private float dis;
    private float CD = 5f;
    Vector3 FlashPos;
    public void FlashAttack(float Damege)//闪电的攻击方法,可将攻击力和攻击距离作为参数传递
    {
        FlashPos = Player.transform.position + new Vector3(0, 3.75f, 0);//记录玩家现在的位置为闪电开始点
        Instantiate(preFlash, FlashPos, Quaternion.identity);//创建闪电前摇提醒玩家闪电落点
        rayhit1 = Physics2D.Raycast(FlashPos, new Vector2(0, -10f));
        if (rayhit1)
        {
            dis = Mathf.Abs(rayhit1.transform.position.y - FlashPos.y);
            //Flash.transform.localScale = new Vector3(0.5f, dis, 0);//得到闪电长度
        }
        StartCoroutine(Example(Damege));//进行伤害判定
        CD = 0f;
    }
    IEnumerator Example(float Damege)//敌人的攻击力作参数
    {
        yield return new WaitForSeconds(1.5f);//从闪电开始点进行射线扫描
        rayhit2 = Physics2D.Raycast(FlashPos, new Vector2(0, -30f));
        FlashPos = FlashPos + new Vector3(0, -rayhit2.distance) + new Vector3(0, 3.75f, 0);
        Instantiate(Flash, FlashPos, Quaternion.identity);//在之前闪电前摇的落点处创建落雷动画
        if (rayhit2)//开始进行伤害判定
        {
            if (rayhit2.transform.tag == "Player")
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CD >= 5)
            FlashAttack(100);
        CD += Time.deltaTime;
    }
}
