using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scjianci : MonoBehaviour
{
    public GameObject jianciG;
    public float dropTime=2;
    public float Cd=0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cd += Time.deltaTime;
        if (Cd>=dropTime) {
            Instantiate(jianciG, transform.position, Quaternion.identity);
            Cd = 0;
        }
    }
}
