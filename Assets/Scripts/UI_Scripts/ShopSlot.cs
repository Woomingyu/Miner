using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// == ���� ���� ���� -> ���� ���Ե�� ���� ��ǰ �ҷ����� -> ���� ���Կ� �����ϰ� ���� ��ǰ�� ��ġ -> ���� ���� ���� -> ��� ��� ���� ���Ը� �ʱ�ȭ(update) 
// ������ ���´� �κ��丮���� �����ϴ� �װ����� shopslot�� ���¸� �����ų��
public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage; // �������� �̹���
    public Item[] shopItems; //������ �Ǹ��� ���� �����۵�   


    //������ ����
    public bool isSell = false; //true�� ���Ը� �Ǹ� Ŭ���� �ȸ�����
    [SerializeField]
    private Inventory inventory;
    public bool isShopSlot; //�������� �Ǻ�


    //���� ������ ������ ������ ���δ� �̹���
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private ShopSlot[] slots; //������ �ش��ϴ� ���Ը� �����Ұ�

    //����(���ſ�)
    private Shop go_ShopGrid;

    //���� NPC UI

    //�ذ��� ��ǳ�� image
    //�ذ��� ��� txt
    //��ư 2�� Y/N


    void Start()
    {
        go_ShopGrid = FindObjectOfType<Shop>();
        slots = GetComponents<ShopSlot>();
    }

    //������ �� ������ �� ������ �̹��� ��ü ����ȭ
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha; // Color�� ���İ�
        itemImage.color = color; //�Ķ���� 1 ���� 0 �Ⱥ���
    }

    //���� ���� ������ �߰�
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item; // ����
        itemCount = _count; // ����
        itemImage.sprite = item.itemImage; //������ �̹���

        if (item.itemType != Item.ItemType.Equipment) //������ Ÿ���� "���" �� �ƴѰ�츸 ����ǥ��
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();

        }
        else //��� ������
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);

        }


        SetColor(1);

    }

    public void SetShop()
    {
        int RandomInt = Random.Range(0, shopItems.Length);
        AddItem(shopItems[RandomInt]);
    }

    //�κ��丮 �� ������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count; //������ ���ϰų� ���� �� ����
        text_Count.text = itemCount.ToString();
        if (itemCount <= 0) // �������� ���������
            ClearSlot();
    }

    //���� ����(�ʱ�ȭ)
    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void ResetShop()
    {
        go_ShopGrid.NBtn();
        ClearSlot();
    }

    //**����** ���� Ŭ��
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {

                go_ShopGrid.CallNPC(item); //�ش� �κк��� Shop.cs�� �ѱ�
                //�ذ񿡼� �������� ���� + ������ ������ �����ϰ� Y/N ������ ��ư�� �����ϴ� UI�� Ȱ��ȭ ��
                //SetSlotCount(-1);
            }

        }
    }

}


    /*
    private Inventory inventoryBase;
    [SerializeField]
    private GameObject shopSlot_parent; //���� ������ �θ�ü (�׸��弼��)
    [SerializeField]
    private Slot[] shopSlots; //������ ����
    public Item[] shopItems; //������ �Ǹ��� ���� �����۵�   

    private void Start()
    {
        inventoryBase = GameObject.Find("Inventory").GetComponent<Inventory>();
        shopSlots = shopSlot_parent.GetComponentsInChildren<Slot>(); //�׸��尡 ������ �ڽİ�ü�� slot.cs�� ȹ��

    }


    //������ ������ ����
    public void ShopSet()
    {
        if (shopSlot_parent == null || shopSlots == null)
        {
            Debug.LogError("ShopSlot parent or slots are not initialized!");
            return;
        }

        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].isShopSlot = true;
        }

        for (int i = 0; i < shopSlots.Length; i++)
        {
            if (shopSlots[i].item == null && shopSlots[i].isShopSlot)
            {
                int RandomInt = Random.Range(0, shopItems.Length);
                shopSlots[i].AddItem(shopItems[RandomInt]);
            }
        }
    }

    public void OnClick()
    {
        //TryBuy();
    }
    /*
    private void TryBuy()
    {
        if (item != null && _cost <= GameManager.score)
        {
            // Deduct price from player's money
            GameManager.score -= _cost;

            // Remove item from exchange slot
            item = null;
            GetComponentInChildren<Image>().sprite = null;

            // Add a copy of the item to inventory
            inventoryBase.AcquireItem(item);
        }
    }

    private void CanBuy(float _cost, Item item)
    {

    }
    */

 