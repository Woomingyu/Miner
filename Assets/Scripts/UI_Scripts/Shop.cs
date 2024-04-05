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
    private TextMeshProUGUI NPCTxt; // NPC 상품설명
    [SerializeField]
    private GameObject[] YesBtn;
    [SerializeField]
    private GameObject[] NoBtn;

    //상점 슬롯
    private ShopSlot[] shopSlots;
    private Item SelectItem;
    private ShopSlot SelectSlot;

    //인벤토리
    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private PlayerController player;
    void Start()
    {
        shopSlots = GetComponentsInChildren<ShopSlot>();
        NPCTxt.text = "상점 NPC 기본대사";
    }

    //상점 NPC 상호작용
    public void CallNPC(Item _item)
    {


        for (int i = 0; i < shopSlots.Length; i++)
        {
            if(shopSlots[i].item == _item)
            {               
                NPCTxt.text = shopSlots[i].item.itemName + "을(를)" + "<color=yellow> " + shopSlots[i].item.itemBuyCost + "G" + "</color>" + " 에 구매하시겠습니까?";
                SelectItem = shopSlots[i].item;
                SelectSlot = shopSlots[i];

                YesBtn[i].SetActive(true);
                NoBtn[i].SetActive(true);
            }
            
        }

    }

    //구매 버튼 클릭
    public void YBtn()
    {
        if (PlayerController.canPickUp && SelectItem.itemBuyCost <= GameManager.score)
        {
            GameManager.score -= SelectItem.itemBuyCost; //아이템 가격만큼 소지금에서 차감
            inventory.AcquireItem(SelectItem); // 인벤토리에 아이템 추가
            SelectSlot.SetSlotCount(-1); // 상점의 해당 슬롯에서 아이템 제거
            NBtn();
        }
        else
            NPCTxt.text = "소지금이나 무게가 부족합니다.";

    }
    //취소 겸 리셋
    public void NBtn()
    {
        NPCTxt.text = "상점 NPC 기본대사";
        for (int i = 0; i < shopSlots.Length; i++)
        {
            YesBtn[i].SetActive(false);
            NoBtn[i].SetActive(false);
        }

    }


}
