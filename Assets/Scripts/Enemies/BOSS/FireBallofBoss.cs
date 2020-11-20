using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallofBoss : MonoBehaviour
{
    public float speed = 10f;//火球速度
    private Vector2 offset1, direction;//火球方向单位向量
    private float dis, x;//dis距离,x火球朝向
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        Player = GameObject.FindGameObjectWithTag("Player");
        offset1 = Player.transform.position - transform.position;
        x = offset1.x / Mathf.Abs(offset1.x);
        dis = Mathf.Sqrt((offset1).sqrMagnitude);
        direction = (Player.transform.position - transform.position) / dis;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        Destroy(gameObject, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider);
        if (collider)
        {
            if (collider.tag == "Player")
            {
                Destroy(gameObject);
                Debug.Log("角色掉血");
            }
            if (collider.tag == "GrassFloor")
            {
                Destroy(gameObject);
                Debug.Log("草方块燃烧");
            }
        }
    }
}
