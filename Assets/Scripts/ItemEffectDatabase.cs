using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //아이템의 이름(키값)

    //아이템의 효과는 여러가지 일 수 있으므로 배열로 설정
    [Tooltip("HP,MP,DP 만 적용가능")]
    public string[] part; // 영향을 줄 부분 (hp,mp,dp ..)
    public int[] num; // 영향 수치( hp -10 ...)
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects; //소모품 아이템들
    [SerializeField]
    private SlotToolTip toolTip;
    [SerializeField]
    private PlayerController player;


    //상수 변수 == 아이템의 효과 string
    private const string HP = "HP", MP = "SP", DP = "DP";



    //추후 인벤토리 내 장비에 따른 교체 함수
    //[serializeField] private WeaponManager theWeaponManager;

    //함수 로직 slot.cs에서 _item 넘겨주고-> 해당 함수에서 SlotToolTip.cs의 ShowToolTip 실행
   //해당 방식은 스택 오버플로우 일어날 수 있으니 주의
    public void ShowToolTip(Item _item, Vector2 _pos)
    {
        toolTip.ShowToolTip(_item, _pos);

    }
    public void HideToolTip()
    {
        toolTip.HideToolTip();
    }

    //아이템 사용
    public void UseItem(Item _item)
    {
        /*
         if(item.itemType == Item.ItemType.Equipment)
        {
           StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.weaponType, item.itemName));
        }
        else if ..*/


        if (_item.itemType == Item.ItemType.Used)
        {
            for (int x = 0; x < itemEffects.Length; x++)
            {
                if (itemEffects[x].itemName == _item.itemName) //받아온 아이템의 이름과 같은 아이템을 배열에서 탐색
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++) // 탐색한 아이템의 효과 개수만큼 반복
                    {
                        switch (itemEffects[x].part[y]) // y번째 효과를 받음
                        {
                            case HP:
                                player.IncreaseHP(itemEffects[x].num[y]); //탐색한 아이템의 y번째 수치적용
                                break;
                            case MP:
                                player.IncreaseMP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                player.IncreaseDP(itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("잘못된 status 자원. HP,MP,DP 만 적용가능");
                                break;
                        }
                        Debug.Log(_item.itemName + "을 사용했습니다");

                    }
                    return;
                }

                }
            Debug.Log("ItemEffectDatabase에 일치하는 itemName이 없음");
            }
        }
    }


