using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UI_Controller : MonoBehaviour
{
    public Image logo;
    public GameObject Setting_Menu,Start_Menu,Level_Menu;
    public Slider BGM, SE_Sound;
    public Toggle easy, normal, hard;
    private float logo_log = 0;
    private float alpha = 0;
    private int level = 1;
    public void start()
    {
        SceneManager.LoadScene("Scene1");
    }
    public void Level_menu()
    {
        Level_Menu.SetActive(true);
        Start_Menu.SetActive(false);
    }
    public void level_menu_exit()
    {
        Start_Menu.SetActive(true);
        Level_Menu.SetActive(false);
    }
    public void exit()
    {
        Application.Quit();
    }
    public void setting_menu()
    {
        Setting_Menu.SetActive(true);
        Start_Menu.SetActive(false);
    }
    public void setting_menu_exit()
    {
        Start_Menu.SetActive(true);
        Setting_Menu.SetActive(false);
    }
    public void BGM_set()
    {
        PlayerPrefs.SetFloat("BGM", BGM.value);
    }
    public void SE_set()
    {
        PlayerPrefs.SetFloat("SE", SE_Sound.value);
    }
    public void Level_set(int level)
    {
        switch(level)//调难易度
        {
            case 1:normal.isOn = false;hard.isOn = false;break;
            case 2:easy.isOn = false;hard.isOn = false;break;
            case 3:easy.isOn = false;normal.isOn = false;break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(PlayerPrefs.GetInt("is_start",1)==1)
        {
            logo_log += Time.deltaTime;
            if (logo_log <= 2)
            {
                logo.color = new Color(logo.color.r, logo.color.g, logo.color.b,alpha);
                alpha += Time.deltaTime/2;
            }
            if (logo_log >= 4)
            {
                logo.color = new Color(logo.color.r, logo.color.g, logo.color.b, alpha);
                alpha -= Time.deltaTime/2;
            }
            if(logo_log>=6.5)
            {
                PlayerPrefs.SetInt("is_start", 0);
                logo.enabled = false;
                Start_Menu.SetActive(true);
            }
        }

    }
}
