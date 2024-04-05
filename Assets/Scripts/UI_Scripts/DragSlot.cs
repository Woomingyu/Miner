using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//실제 슬롯 대신 움직일 드래그 슬롯용 스크립트
//슬롯을 앞에 보여주고, 실제 슬롯 위치를 옮기지 않게하기위해
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance; //드래그 슬롯 인스턴스

    public Slot dragSlot; // 드래그가 시작되면 instance에 들어갈 변수


    // 아이템 이미지
    [SerializeField]
    private Image imageItem;

    private void Start()
    {
        instance = this; //인스턴스에 자기자신 넣어주기
    }

    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
