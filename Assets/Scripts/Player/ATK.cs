using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATK : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject energy_block;
    public GameObject pre_energy_block;
    public float speed=10;
    public int energy_type=0;
    float damage;
    bool is_atk = false;
    bool is_fired = false;
    float theta = 0;
    Vector3 origin_scale;
    Vector3 atk_dir;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (is_fired&&collision.tag!="Player")
        {
            Destroy(gameObject);
        }
        if(collision.tag=="Enemy_Flash")
        {
            collision.GetComponent<EnemyFlash>().health -= damage;
        }
        else if(collision.tag=="Enemy_Fire")
        {
            collision.GetComponent<EnemyFire>().health -= damage;
        }
        else if(collision.tag=="BOSS")
        {
            collision.GetComponent<Boss>().health -= damage;
        }
    }
    public void atk(int energy_type,Vector3 mouse_position)
    {
        energy_block.GetComponent<energy_block>().enabled = false;
        GetComponent<Animator>().SetInteger("mode", energy_type);
        is_atk = true;
        StartCoroutine(Fire(mouse_position));
    }
    IEnumerator Fire(Vector3 mouse_position)
    {
        yield return new WaitForSeconds(1);//1s后发射
        is_atk = false;
        is_fired = true;
        atk_dir = mouse_position - transform.position;
        GetComponent<Rigidbody2D>().velocity = atk_dir.normalized*speed;
        pre_energy_block.GetComponent<Pre_energy_block>().exit_block--;
    }
    void Start()
    {
        origin_scale = energy_block.transform.localScale;
        GetComponent<Animator>().SetInteger("mode", energy_block.GetComponent<energy_block>().energy_type);
        damage= energy_block.GetComponent<energy_block>().energy + energy_block.GetComponent<energy_block>().linjie_energy;
    }

    // Update is called once per frame
    void Update()
    {
        if(is_atk)
        {
            energy_block.transform.localRotation = Quaternion.Euler(0, 0,theta );
            theta += 6;
            if (energy_block.transform.localScale.x - origin_scale.x / 60 > 0)
                energy_block.transform.localScale -= origin_scale*1/60;
            else
            {
                damage = energy_block.GetComponent<energy_block>().energy + energy_block.GetComponent<energy_block>().linjie_energy;
                energy_block.transform.localScale = new Vector3(0, 0, 0);
            }
                
        }
        if(is_fired)
        {
            speed += 0.5f;
            GetComponent<Rigidbody2D>().velocity = atk_dir.normalized * speed;
        }
        
    }
}
