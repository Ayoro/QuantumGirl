using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game_Controler : MonoBehaviour
{
    public GameObject Player;
    public GameObject pre_energy_block;
    public GameObject mouse;
    public Camera camera;
    public Slider health_slide,energy_slide;
    public bool atk_mode = false;
    RaycastHit2D rayhit1, rayhit2;
    Vector3 mouse_position;
    Vector3 FlashPos;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = Player.transform.position + new Vector3(0, 0, -5);
        camera.transform.position = Player.transform.position + new Vector3(0, 0, -5);
        Vector3 mouse_position = camera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        mouse_position.z = 0;
        mouse.transform.position = mouse_position;
        if (pre_energy_block.GetComponent<Pre_energy_block>().is_generating)
            mouse.SetActive(false);
        else
        {
            mouse.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (pre_energy_block.GetComponent<Pre_energy_block>().is_generating)
            {
                mouse.SetActive(true);
                atk_mode = true;
            }
            else
                atk_mode = !atk_mode;
            if (atk_mode)
            {
                pre_energy_block.GetComponent<Pre_energy_block>().is_generating = false;
                mouse.GetComponent<Animator>().SetInteger("mode", 1);
            }
            else
            {
                mouse.GetComponent<Animator>().SetInteger("mode", 0);
            }
        }
        energy_slide.value = 0.75f + Player.GetComponent<Player>().energy/400;
        health_slide.value = 0.75f + Player.GetComponent<Player>().player_health / 400;
    }
}
