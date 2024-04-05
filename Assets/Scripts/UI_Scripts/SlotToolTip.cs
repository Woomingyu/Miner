using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    // �ʿ��� ������Ʈ
    [SerializeField]
    private GameObject go_Base; // ���� UI�̹���

    [SerializeField]
    private TextMeshProUGUI itemName_Txt; // ������ �̸�
    [SerializeField]
    private TextMeshProUGUI itemDesc_Txt; // ������ ����
    [SerializeField]
    private TextMeshProUGUI itemType_Txt; // ������ Ÿ��
    [SerializeField]
    private TextMeshProUGUI itemCost_Txt; // ������ ����

    private void Update()
    {
        if (!Inventory.inventoryActivated)
            HideToolTip();
    }


    //���� UI Ȱ��ȭ
    public void ShowToolTip(Item _item, Vector2 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector2(-go_Base.GetComponent<RectTransform>().rect.width * 5f, go_Base.GetComponent<RectTransform>().rect.height * 4f);
        go_Base.transform.position = _pos;
        string CostTxt = _item.itemCost.ToString(); //������ ���� ���ڿ� ����ȯ

        //Item ��ũ��Ʈ�� ���� ����/����/�ǸŰ� TXT ����
        itemName_Txt.text = _item.itemName;
        itemDesc_Txt.text = _item.itemDesc;
        itemCost_Txt.text = "�ǸŰ�" + CostTxt +"G";
        //itemType_Txt.text = _item.itemName;


        //������ Ÿ�� TXT
        if (_item.itemType == Item.ItemType.Equipment)
            itemType_Txt.text = "���";
        else if (_item.itemType == Item.ItemType.Used)
            itemType_Txt.text = "�Ҹ�ǰ";
        else if (_item.itemType == Item.ItemType.Ingredient)
            itemType_Txt.text = "���";
    }
    
    //���� UI ��Ȱ��ȭ

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
