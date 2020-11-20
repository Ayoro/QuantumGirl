using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pre_energy_block : MonoBehaviour {

    // Use this for initialization
    public GameObject Energy_Block,player,game_controller;
    public int block_num = 0;
    public bool is_generating = false;
    float size = 1;
    public Vector3 oringin_size;
    RaycastHit2D hit;
    GameObject obj;
    public int exit_block = 0;
    bool build_access1 = true;//判断当前位置是否允许建造
    bool build_access2 = true;//判断当前能量是否足够建造
    bool build_access3 = false;//判断当前距离是否允许建造
    public Camera camera;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "energy_block"&&Input.GetMouseButtonDown(1))//摧毁已有能量体，并返还能量值
            Destroy(other.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "energy_block")
        {
            build_access1 = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "energy_block")
        {
            build_access1 = true;
        }
    }
    void Block_generate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            is_generating = !is_generating;
            if (is_generating)
                game_controller.GetComponent<Game_Controler>().atk_mode = false;
            size = 1;
        }
        if (is_generating)
        {
            if (size * player.GetComponent<Player>().energy_cost >= player.GetComponent<Player>().energy)//判断是否还有能量建造
            {
                build_access2 = false;
            }
            else
            {
                build_access2 = true;
            }
            Vector3 mouse_position = camera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            mouse_position.z = 0;
            transform.position = mouse_position;
            if ((transform.position - player.transform.position).sqrMagnitude < 120)//只能在离主角为120的范围内建造能量块
                build_access3 = true;
            else
                build_access3 = false;
            if (Input.GetMouseButtonDown(0)&&exit_block<2&&build_access1&&build_access2&&build_access3)//建造条件：1.开启了建造模式 2.场上能量块数<2   3.当前地形允许建造   4.当前能量足够建造  5.当前位置在建造范围内
            {
                obj = Instantiate(Energy_Block);
                block_num++;
                exit_block++;
                obj.transform.Find("energy_block").GetComponent<energy_block>().num = block_num;
                obj.transform.position = transform.position;
                obj.transform.Find("energy_block").GetComponent<energy_block>().linjie_energy = player.GetComponent<Player>().energy_cost * size;
                obj.transform.localScale *= size;
                player.GetComponent<Player>().energy -= player.GetComponent<Player>().energy_cost * size;
                size = 1;
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && size <= 3)
            {
                size += 0.1f;
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && size >= 0.5)
            {
                size -= 0.1f;
            }
            transform.localScale = oringin_size * size;//改变大小
        }
        else
        {
            transform.position = new Vector3(0, -1000);
        }
    }
    void state_change()
    {
        if (build_access1 && build_access2&&build_access3&&exit_block<2)
            GetComponent<Animator>().SetInteger("mode", 1);
        else
            GetComponent<Animator>().SetInteger("mode", 0);
    }
    void Start()
    {
        oringin_size = transform.localScale;
    }
	// Update is called once per frame
	void Update () {
        Block_generate();
        state_change();
    }
}
