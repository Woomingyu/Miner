using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{

    //NPC
    [SerializeField]
    private TextMeshProUGUI NPCTxt; // NPC ��ǰ����
    [SerializeField]
    private GameObject[] YesBtn;
    [SerializeField]
    private GameObject[] NoBtn;

    //���� ����
    private ShopSlot[] shopSlots;
    private Item SelectItem;
    private ShopSlot SelectSlot;

    //�κ��丮
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private PlayerController player;
    void Start()
    {
        shopSlots = GetComponentsInChildren<ShopSlot>();
        NPCTxt.text = "���� NPC �⺻���";
    }

    //���� NPC ��ȣ�ۿ�
    public void CallNPC(Item _item)
    {


        for (int i = 0; i < shopSlots.Length; i++)
        {
            if(shopSlots[i].item == _item)
            {               
                NPCTxt.text = shopSlots[i].item.itemName + "��(��)" + "<color=yellow> " + shopSlots[i].item.itemBuyCost + "G" + "</color>" + " �� �����Ͻðڽ��ϱ�?";
                SelectItem = shopSlots[i].item;
                SelectSlot = shopSlots[i];

                YesBtn[i].SetActive(true);
                NoBtn[i].SetActive(true);
            }
            
        }

    }

    //���� ��ư Ŭ��
    public void YBtn()
    {
        if (PlayerController.canPickUp && SelectItem.itemBuyCost <= GameManager.score)
        {
            GameManager.score -= SelectItem.itemBuyCost; //������ ���ݸ�ŭ �����ݿ��� ����
            inventory.AcquireItem(SelectItem); // �κ��丮�� ������ �߰�
            SelectSlot.SetSlotCount(-1); // ������ �ش� ���Կ��� ������ ����
            NBtn();
        }
        else
            NPCTxt.text = "�������̳� ���԰� �����մϴ�.";

    }
    //��� �� ����
    public void NBtn()
    {
        NPCTxt.text = "���� NPC �⺻���";
        for (int i = 0; i < shopSlots.Length; i++)
        {
            YesBtn[i].SetActive(false);
            NoBtn[i].SetActive(false);
        }

    }


}
