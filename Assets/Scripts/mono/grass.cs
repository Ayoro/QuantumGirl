using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grass : MonoBehaviour
{
    public bool is_fired = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "grass")
        {
            if (collision.GetComponent<grass>().is_fired)
                is_fired = true;
        }
        else if (collision.tag == "fire_ball")
            is_fired = true;
        else if (collision.tag == "atk")
        {
            if (collision.GetComponent<ATK>().energy_type == 2)
                is_fired = true;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(is_fired)
        {
            //动画
        }
    }
}
