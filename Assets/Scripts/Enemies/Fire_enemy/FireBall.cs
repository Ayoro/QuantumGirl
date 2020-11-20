using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public  float speed=5f;//火球速度

    private Vector2 offset1, direction;//火球方向单位向量
    private float dis,x;//dis距离,x火球朝向
    public GameObject Player;
    public float Damge=10f;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        offset1 = Player.transform.position - transform.position;
        x = offset1.x / Mathf.Abs(offset1.x);
        dis = Mathf.Sqrt((offset1).sqrMagnitude);
        direction = (Player.transform.position - transform.position) / dis;
        transform.rotation = Quaternion.Euler(0, (x + 1) * 90, 0);
        transform.localScale *= 0.5f;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Player>().player_health -= Damge;
            }
            if (collider.tag == "grass")
            {
                collider.GetComponent<grass>().is_fired = true;
                Debug.Log("草方块燃烧");
            }
            if(collider.tag=="energy_block")
            {
                collider.GetComponent<energy_block>().energy += Damge;
                collider.GetComponent<energy_block>().energy_type = 2;
            }
            if(collider.tag!="Enemy_Fire")
                Destroy(gameObject);
        }
    }

}
