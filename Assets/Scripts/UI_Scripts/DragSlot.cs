using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ���� ��� ������ �巡�� ���Կ� ��ũ��Ʈ
//������ �տ� �����ְ�, ���� ���� ��ġ�� �ű��� �ʰ��ϱ�����
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance; //�巡�� ���� �ν��Ͻ�

    public Slot dragSlot; // �巡�װ� ���۵Ǹ� instance�� �� ����


    // ������ �̹���
    [SerializeField]
    private Image imageItem;

    private void Start()
    {
        instance = this; //�ν��Ͻ��� �ڱ��ڽ� �־��ֱ�
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
