using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ore : MonoBehaviour
{

    [SerializeField]
    private int hp; // ������ ü��
    private int resetHp; // ����ü�� ���¿�


    //�ʿ��� �ݶ��̴�

    //���� ������ ������Ʈ & ���� ���� & ���� ��ġ & ȹ�� �ȳ� & �κ��丮 ����
    [SerializeField]
    private GameObject go_ore_item_prefab; // ���� ������
    [SerializeField]
    private int count;
    [SerializeField]
    private Transform Ore_ItemPos;
    [SerializeField]
    private TextMeshProUGUI PickUpTxt; //�Ӹ� �� ������ ȹ������ txt
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private PlayerController player;


    //�ݶ��̴��� ������ٵ� �̿��� �ڵ����� ������� �����¹����� ��������
    [SerializeField]
    private GameObject go_effect_prefabs; // ä�� ����Ʈ


    //�ʿ��� ���� �̸�((���� �Ŵ������� ������ҽ� �˾Ƽ� Ʋ����)
    [SerializeField]
    private string strike_Sound; // ���� �ǰ� ����
    [SerializeField]
    private string destroy_Sound; // ���� �ı� ����

    private void Start()
    {
        resetHp = hp;
    }




    public void OreMining() //���� �μ�����
    {
        
        GameManager.I.PlaySE(strike_Sound); //����Ŵ������� ��û���
        /*
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        go_effect_prefabs.GetComponent<Rigidbody2D>().AddForce((Vector2.one) * 3); // ���� ���� �� ���� ��
       
        //����(�����ϴ°�, �ݶ��̴��� �߽�, ȸ����.�⺻������)
        Destroy(clone, destroyTime); // ����Ʈ �����ı�
         */


        hp--; // ���⿡ ���� �߰� hp ���ظ� �ִ°��� �߰��ϴ°� ������
        if (hp <= 0) //����ü�� 1�� ���ҽ�Ű�� 0���� �۰ų� ��������
            Destruction(); //�ı��Լ� ����
      
    }

    private void Destruction() //���� ��Ȱ�� & ȹ��(�ڵ�)
    {       
        this.gameObject.SetActive(false); // �� ���� ������Ʈ ��Ȱ��ȭ
        GameManager.I.PlaySE(destroy_Sound); //����Ŵ������� ��û���
        UIManager.isMinning = false;
        Cursor.coolTime = false;

        hp = resetHp; //���� ü�� �ʱ�ȭ (�ı� �ƴ϶�)
        //UIManager.OffMinningUI();

        //���⼭ ������ ������ �����ؼ� ������ == ���� ������ ���� ������ ���� ���� �� ����
        
            
            for (int i = 0; i < count; i++) //count ��ŭ ������ ������ ����
        {



            GameObject OreClone = Instantiate(go_ore_item_prefab, Ore_ItemPos.position, Quaternion.identity);
            OreClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 10);
            Destroy(OreClone, 0.5f); //���� ������ �ı�


            player.IncreaseWT(OreClone.transform.GetComponent<ItemPickUp>().item.itemWeight); //���� �߰�

            if (!PlayerController.canPickUp) //���繫�� + ������������ ���� >= �ִ빫��)
                return;

            //ȹ�� ������ �κ��丮��
            inventory.AcquireItem(OreClone.transform.GetComponent<ItemPickUp>().item);
            /*
            //������ ȹ�� �ȳ� txt
            PickUpTxt.gameObject.SetActive(true);
            PickUpTxt.text = "<color=red> " + OreClone.transform.GetComponent<ItemPickUp>().item.itemName + "</color>" + "ȹ��";
            */

        }

            Invoke("HidePickUpTxt", 1.5f);


        


    }

    private void HidePickUpTxt()
    {
        PickUpTxt.gameObject.SetActive(false);

    }

    //���� ���� �� �ҷ�����
    public void ResetOre()
    {
        this.gameObject.SetActive(false); // �� ���� ������Ʈ ��Ȱ��ȭ
        UIManager.isMinning = false;
        Cursor.coolTime = false;

        hp = resetHp; //���� ü�� �ʱ�ȭ (�ı� �ƴ϶�)
    }
}

