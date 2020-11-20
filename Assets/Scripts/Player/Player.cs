using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float energy,energy_cost,player_health;
    public Animator animator;
    bool is_fired = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="jianci")
        {
            player_health = 0;
        }
    }
    public void Flash(float damege)//被雷击
    {
        player_health -= damege;
        GetComponent<Movement>().enabled = false;
        //播放被雷击动画
        animator.SetInteger("mode", 4);
        StartCoroutine(Palsy());
    }
    IEnumerator Palsy()//麻痹效果
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<Movement>().enabled = true;
    }
    private void Fire(float damege)//被火焰攻击
    {
        player_health -= damege;
        is_fired = true;
    }
    private void Firing()
    {
        player_health -= 0.05f;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (is_fired)
            Firing();
        if (energy > 100)
        {
            energy = 100;
            //player_health -= energy - 100;
        }
        if (player_health == 0)
        {
            Debug.Log(0);
            //gameObject.GetComponent<Movement>().enabled = false;
            animator.SetBool("is_dead", true);
        }
    }
}
