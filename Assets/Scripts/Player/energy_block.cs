using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energy_block : MonoBehaviour {

	public float energy=0,linjie_energy;
    public GameObject player,pre_energy_block,atk,mouse,game_controller;
    public Animator animator;
    float Pi = 3.14f;
    RaycastHit2D hit;
    private bool is_explosion = false;
    public int num;
    float dis;
    public int energy_type=0;//0:没有能量  1:雷电    2：火焰
    private bool is_atk = false;
    private bool ready_attak = false;
    private float rotatespeed = 1.0f;
    float theta = 0;
    void OnTriggerStay2D(Collider2D mono)
    {
        if (mono.tag == "pre_energy_block")//摧毁已有能量体，并返还能量值
        {
            if(Input.GetMouseButtonDown(1)&&!is_explosion)
            {
                player.GetComponent<Player>().energy += linjie_energy/2+energy;
                mono.GetComponent<Pre_energy_block>().exit_block--;
                Destroy(gameObject);
            }
        }
        else if (mono.tag == "mouse")
        {
            if (Input.GetMouseButtonDown(0)&&game_controller.GetComponent<Game_Controler>().atk_mode&&!is_explosion)
            {
                ready_attak = true;
                atk.SetActive(true);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D mono)//能量块融合
    {
        if (mono.collider.tag == "energy_block")
            if (num > mono.collider.GetComponent<energy_block>().num)
            {
                pre_energy_block.GetComponent<Pre_energy_block>().exit_block--;
                energy += mono.collider.GetComponent<energy_block>().energy;
                linjie_energy += mono.collider.GetComponent<energy_block>().linjie_energy;
                Vector2 new_position = (mono.transform.position* mono.collider.GetComponent<energy_block>().linjie_energy + transform.position* linjie_energy)/(linjie_energy+ mono.collider.GetComponent<energy_block>().linjie_energy);//可以试着按权值
                Destroy(mono.gameObject);
                transform.position = new_position;
                transform.localScale =new Vector3(2,2,2)*(linjie_energy / player.GetComponent<Player>().energy_cost);
            }
    }
    IEnumerator explosion()
    {
        yield return new WaitForSeconds(1);//1s后爆炸
        GetComponent<Animator>().SetBool("explosion", true);
        dis = 5 * energy;
        for (float a = 0; a <= 2 * Pi; a += Pi / 30)
        {
            hit = Physics2D.Raycast(gameObject.transform.position, new Vector2(Mathf.Cos(a), Mathf.Sin(a)), dis);
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.position + new Vector3(Mathf.Cos(a), Mathf.Sin(a)) * dis);
            if (!hit) ;
            else
            {
                //能量体爆炸
                if (hit.collider.tag == "Player")
                {
                    hit.collider.GetComponent<Rigidbody2D>().velocity=(hit.collider.transform.position - transform.position)*energy/5 ;
                }
                else if (hit.collider.tag == "Enemy_Flash")
                {
                    hit.collider.GetComponent<Rigidbody2D>().AddForce((hit.collider.transform.position - transform.position) / hit.distance * energy /2);
                    hit.collider.GetComponent<EnemyFlash>().health -= energy / hit.distance;
                }
                else if (hit.collider.tag == "Enemy_Fire")
                {
                    hit.collider.GetComponent<Rigidbody2D>().AddForce((hit.collider.transform.position - transform.position) / hit.distance * energy / 2);
                    hit.collider.GetComponent<EnemyFire>().health -= energy / hit.distance;
                }
                else if (hit.collider.tag == "BOSS")
                {
                    hit.collider.GetComponent<Boss>().health -= energy / hit.distance;
                }
                else if (hit.collider.tag == "energy_block")
                {
                    hit.collider.GetComponent<energy_block>().energy += energy;
                }
            }
        }
        pre_energy_block.GetComponent<Pre_energy_block>().exit_block--;
        Destroy(gameObject,0.5f);//爆炸
    }
    void over_judge()
    {
        if (energy >= linjie_energy)
        {
            is_explosion = true;
        }
        if (is_explosion)
            StartCoroutine(explosion());
    }
    IEnumerator Attack(Vector3 mouse_position)
    {
        yield return new WaitForSeconds(1); 
        
    }
	void Start () {
        animator = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
        if(!is_explosion)
            over_judge();
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, theta);
            theta += 20;
        }
        animator.SetInteger("mode", energy_type);
        if(ready_attak&&!is_atk&&!is_explosion)
        {
            atk.SetActive(true);
            float z;
            if (mouse.transform.position.x > transform.position.x)
            {
                z = -Vector3.Angle(Vector3.up, mouse.transform.position - transform.position);
            }
            else
            {
                z = Vector3.Angle(Vector3.up, (mouse.transform.position - transform.position));
            }        //把旋转角度赋给旋转体，进行对应旋转
            atk.transform.localRotation = Quaternion.Euler(0, 0, z * rotatespeed);
            if (!Input.GetMouseButton(0))
            {
                is_atk = true;
                atk.GetComponent<ATK>().atk(energy_type, mouse.transform.position);
            }
        }
    }
}
