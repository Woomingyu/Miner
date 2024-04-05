using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; //�������� �̸�(Ű��)

    //�������� ȿ���� �������� �� �� �����Ƿ� �迭�� ����
    [Tooltip("HP,MP,DP �� ���밡��")]
    public string[] part; // ������ �� �κ� (hp,mp,dp ..)
    public int[] num; // ���� ��ġ( hp -10 ...)
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects; //�Ҹ�ǰ �����۵�
    [SerializeField]
    private SlotToolTip toolTip;
    [SerializeField]
    private PlayerController player;


    //��� ���� == �������� ȿ�� string
    private const string HP = "HP", MP = "SP", DP = "DP";



    //���� �κ��丮 �� ��� ���� ��ü �Լ�
    //[serializeField] private WeaponManager theWeaponManager;

    //�Լ� ���� slot.cs���� _item �Ѱ��ְ�-> �ش� �Լ����� SlotToolTip.cs�� ShowToolTip ����
   //�ش� ����� ���� �����÷ο� �Ͼ �� ������ ����
    public void ShowToolTip(Item _item, Vector2 _pos)
    {
        toolTip.ShowToolTip(_item, _pos);

    }
    public void HideToolTip()
    {
        toolTip.HideToolTip();
    }

    //������ ���
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
                if (itemEffects[x].itemName == _item.itemName) //�޾ƿ� �������� �̸��� ���� �������� �迭���� Ž��
                {
                    for (int y = 0; y < itemEffects[x].part.Length; y++) // Ž���� �������� ȿ�� ������ŭ �ݺ�
                    {
                        switch (itemEffects[x].part[y]) // y��° ȿ���� ����
                        {
                            case HP:
                                player.IncreaseHP(itemEffects[x].num[y]); //Ž���� �������� y��° ��ġ����
                                break;
                            case MP:
                                player.IncreaseMP(itemEffects[x].num[y]);
                                break;
                            case DP:
                                player.IncreaseDP(itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("�߸��� status �ڿ�. HP,MP,DP �� ���밡��");
                                break;
                        }
                        Debug.Log(_item.itemName + "�� ����߽��ϴ�");

                    }
                    return;
                }

                }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ� itemName�� ����");
            }
        }
    }


