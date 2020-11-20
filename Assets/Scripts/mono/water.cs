using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    public bool is_flashed = false;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "water")
        {
            if (collision.GetComponent<water>().is_flashed)
                is_flashed = true;
        }
        else if (collision.tag == "fire_ball")
            is_flashed = true;
        else if (collision.tag == "atk")
        {
            if (collision.GetComponent<ATK>().energy_type == 1)
                is_flashed = true;
        }
    }// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(is_flashed)
        {
            //播放动画
        }
    }
}
