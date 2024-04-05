using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Inventory : MonoBehaviour
{

    public static bool inventoryActivated = false; //인벤토리 활성화 상태
    public static bool shopActivated = false; //상점 활성호 ㅏ상태





    //필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent; //그리드 세팅 (모든 슬롯관리)
    [SerializeField]
    private PlayerController player;

    //상점
    [SerializeField]
    private GameObject go_shopBase;
    [SerializeField]
    private GameObject go_shopSlotParent;
    private Animator anim;
    //슬롯들
    private Slot[] slots;
    private ShopSlot[] ShopSlots;
    void Start()
    {
        //슬롯 배열내에 모든 실제 슬롯 입력
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        ShopSlots = go_shopSlotParent.GetComponentsInChildren<ShopSlot>();
        anim = GetComponent<Animator>();
    }




    public void TryOpenIventory()
    {
        inventoryActivated = !inventoryActivated; //누르면 활성화 인벤 비활성화 스위칭
        anim.SetBool("Appear", inventoryActivated); //인벤 활성/비활성 애니 


        //인벤토리가 열린 경우는 상태변수로 여러 조작/흐름 을 막아놓음(메모)
        if (inventoryActivated)
        {
            OpenInventory();
        }


        else
        {
            CloseInventory();
        }
    }

    //인벤토리 활성화 && 비활성화
    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    //아이템 획득 ==> Ore의 Destruction.반복문 &&
    public void AcquireItem(Item _item, int _count = 1)
    {
        if (!PlayerController.canPickUp)
            return;
        
            if (Item.ItemType.Equipment != _item.itemType) // 장비가 아닌경우에만 (장비는 개수x)
            {
                if (_item.itemType != Item.ItemType.Ingredient) //아이템 타입이 "재료" 가 아닌경우만 무게 계산
                player.IncreaseWT(_item.itemWeight);

            //동일 아이템이 있는 경우부터 조사 반복문 
            for (int i = 0; i < slots.Length; i++) //슬롯 개수만큼 반복문
                {
                    if (slots[i].item != null) // 슬롯 기본값 null이므로 null참조 방지
                    {
                        if (slots[i].item.itemName == _item.itemName && PlayerController.canPickUp) //넘어온 아이템이 이미 인벤에 있다면
                        {
                            slots[i].SetSlotCount(_count);
                            return;
                        }
                    }

                }

            }

            //동일 아이템이 없는 경우 && 장비인 경우
            for (int i = 0; i < slots.Length; i++) //슬롯 개수만큼 반복문
            {
                if (slots[i].item == null && PlayerController.canPickUp) //인벤에 동일 아이템이 없다면
                {
                    slots[i].AddItem(_item, _count); // 슬롯의 additem 함수 호출(새 아이템 추가)
                    return;
                }
            }          

    }

    public void DeAcquireItem(Item _item, int _count = -1)
    {
        player.DecreaseWT(_item.itemWeight);
        //동일 아이템이 있는 경우부터 조사 반복문 
        for (int i = 0; i < slots.Length; i++) //슬롯 개수만큼 반복문
        {
            if (slots[i].item != null && slots[i].isSell) // 슬롯 기본값 null이므로 null참조 방지
            {
                if (slots[i].item.itemName == _item.itemName) //넘어온 아이템이 이미 인벤에 있다면
                {
                    slots[i].SetSlotCount(_count);                  
                }
            }           
        }


    }


    //##상점##

    public void OpenShop() //해당 메소드의 전달인자로 모든 슬롯에 shop상태를 변경함
    {
        go_shopBase.SetActive(true);
        shopActivated = true;
        TryOpenIventory();
        for (int i = 0; i < slots.Length; i++)//인벤토리,퀵슬롯
        {
            slots[i].isShop = true;
        }

        for (int i = 0; i < ShopSlots.Length; i++)//인벤토리,퀵슬롯
        {
            ShopSlots[i].SetShop();
        }
    }


    public void CloseShop()
    {
        shopActivated = false;
        go_shopBase.SetActive(false);
        TryOpenIventory();
        for (int i = 0; i < slots.Length; i++) //인벤토리,퀵슬롯
        {
            slots[i].isShop = false; //상점상태
            slots[i].isSell = false; //판매 체크상태
            slots[i].chkSell.SetActive(false); //판매 체크 이미지
        }
        for (int i = 0; i < ShopSlots.Length; i++)//인벤토리,퀵슬롯
        {
            ShopSlots[i].ResetShop();
        }
    }


    //아이템 판매
    public void SellItem()
    {
        for (int i = 0; i < slots.Length; i++) //슬롯 개수만큼 반복문
        {
            if (slots[i].item != null) // 슬롯 기본값 null이므로 null참조 방지
            {
                if (slots[i].isSell) //isSell체크가 된 항목만 포함
                {
                        GameManager.score += slots[i].item.itemCost; //스코어에 아이템 가격을 더해줌
                        DeAcquireItem(slots[i].item);

                    //체크로 모든 아이템을 판매하려는 경우 slots[i].itemCount를 통한 반복문을 사용하면 될듯?
                }
            }
            slots[i].isSell = false;
            slots[i].chkSell.SetActive(false);
        }
        /*
        Debug.Log("클릭만 함");
        if (slots)
        {
            Debug.Log("조건충족");
            for (int i = 0; i < itemCount; i++)
            {
                //해당 아이템 개수만큼 반복하도록 설정할것
                GameManager.score += item.itemCost; //스코어에 아이템 가격을 더해줌
                inventory.DeAcquireItem(item);
            }


        }
        */
    }

}


    

