using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logo_Controller : MonoBehaviour
{
    public Image logo;
    private float logo_log = 0;
    private float alpha = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        logo_log += Time.deltaTime;
        if (logo_log <= 2)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, alpha);
            alpha += Time.deltaTime / 2;
        }
        if (logo_log >= 4)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, alpha);
            alpha -= Time.deltaTime / 2;
        }
        if (logo_log >= 6.5)
        {
            PlayerPrefs.SetInt("is_start", 0);
            logo.enabled = false;
            SceneManager.LoadScene("Game_menu");
        }
    }
}
