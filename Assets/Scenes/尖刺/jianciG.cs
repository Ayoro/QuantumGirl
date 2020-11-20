using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jianciG : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("111" + collider);
        if (collider)
        {
            if (collider.tag == "Player")
            {
                Debug.Log("角色掉血");
            }
        }
    }

}
