using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_Base; // 툴팁 UI이미지

    [SerializeField]
    private TextMeshProUGUI itemName_Txt; // 아이템 이름
    [SerializeField]
    private TextMeshProUGUI itemDesc_Txt; // 아이템 설명
    [SerializeField]
    private TextMeshProUGUI itemType_Txt; // 아이템 타입
    [SerializeField]
    private TextMeshProUGUI itemCost_Txt; // 아이템 가격

    private void Update()
    {
        if (!Inventory.inventoryActivated)
            HideToolTip();
    }


    //툴팁 UI 활성화
    public void ShowToolTip(Item _item, Vector2 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector2(-go_Base.GetComponent<RectTransform>().rect.width * 5f, go_Base.GetComponent<RectTransform>().rect.height * 4f);
        go_Base.transform.position = _pos;
        string CostTxt = _item.itemCost.ToString(); //아이템 가격 문자열 형변환

        //Item 스크립트에 따른 제목/설명/판매가 TXT 변경
        itemName_Txt.text = _item.itemName;
        itemDesc_Txt.text = _item.itemDesc;
        itemCost_Txt.text = "판매가" + CostTxt +"G";
        //itemType_Txt.text = _item.itemName;


        //아이템 타입 TXT
        if (_item.itemType == Item.ItemType.Equipment)
            itemType_Txt.text = "장비";
        else if (_item.itemType == Item.ItemType.Used)
            itemType_Txt.text = "소모품";
        else if (_item.itemType == Item.ItemType.Ingredient)
            itemType_Txt.text = "재료";
    }
    
    //툴팁 UI 비활성화

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
