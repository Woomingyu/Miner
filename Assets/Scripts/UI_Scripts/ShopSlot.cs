using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// == 상점 열림 감지 -> 상점 슬롯들과 상점 물품 불러오기 -> 상점 슬롯에 랜덤하게 상점 물품을 배치 -> 상점 닫힘 감지 -> 즉시 모든 상점 슬롯만 초기화(update) 
// 슬롯의 상태는 인벤토리에서 변경하니 그곳에서 shopslot의 상태를 변경시킬것
public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage; // 아이템의 이미지
    public Item[] shopItems; //상점에 판매할 랜덤 아이템들   


    //상점용 변수
    public bool isSell = false; //true인 슬롯만 판매 클릭시 팔리도록
    [SerializeField]
    private Inventory inventory;
    public bool isShopSlot; //상점슬롯 판별


    //슬롯 아이템 개수와 개수를 감싸는 이미지
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private ShopSlot[] slots; //상점에 해당하는 슬롯만 지정할것

    //상점(구매용)
    private Shop go_ShopGrid;

    //상점 NPC UI

    //해골의 말풍선 image
    //해골의 대사 txt
    //버튼 2종 Y/N


    void Start()
    {
        go_ShopGrid = FindObjectOfType<Shop>();
        slots = GetComponents<ShopSlot>();
    }

    //슬롯이 빈 상태일 때 아이템 이미지 객체 투명화
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha; // Color의 알파값
        itemImage.color = color; //파라미터 1 보임 0 안보임
    }

    //실제 슬롯 아이템 추가
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item; // 정보
        itemCount = _count; // 개수
        itemImage.sprite = item.itemImage; //아이템 이미지

        if (item.itemType != Item.ItemType.Equipment) //아이템 타입이 "장비" 가 아닌경우만 개수표기
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();

        }
        else //장비 아이템
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

    //인벤토리 내 아이템 개수 조절
    public void SetSlotCount(int _count)
    {
        itemCount += _count; //개수를 더하거나 깎을 수 있음
        text_Count.text = itemCount.ToString();
        if (itemCount <= 0) // 아이템이 없어진경우
            ClearSlot();
    }

    //슬롯 비우기(초기화)
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

    //**상점** 슬롯 클릭
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {

                go_ShopGrid.CallNPC(item); //해당 부분부터 Shop.cs로 넘김
                //해골에서 아이템의 정보 + 가격의 정보를 제공하고 Y/N 선택지 버튼을 포함하는 UI를 활성화 함
                //SetSlotCount(-1);
            }

        }
    }

}


    /*
    private Inventory inventoryBase;
    [SerializeField]
    private GameObject shopSlot_parent; //상점 슬롯의 부모객체 (그리드세팅)
    [SerializeField]
    private Slot[] shopSlots; //상점의 슬롯
    public Item[] shopItems; //상점에 판매할 랜덤 아이템들   

    private void Start()
    {
        inventoryBase = GameObject.Find("Inventory").GetComponent<Inventory>();
        shopSlots = shopSlot_parent.GetComponentsInChildren<Slot>(); //그리드가 퀵슬롯 자식객체의 slot.cs만 획득

    }


    //상점에 아이템 세팅
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

 