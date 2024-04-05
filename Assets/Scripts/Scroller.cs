using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{

    public int count; // ���� �׷� �� �ڽ� ��
    public float speedRate; // �� ��ũ�Ѹ� �ӵ�%



    void Start()
    {
        count = transform.childCount; // Ʈ���������� �ڽİ�ü �� �޾ƿ��� (get�Լ�ȹ���� ���Ž���)
    }

    void Update()
    {
        if (!GameManager.isLive)
            return;


        if(!UIManager.isMinning && !Inventory.inventoryActivated && !EventContoroller.randomEvent)
        {
            //�� ��ũ�Ѹ� �ӵ�(=�̵��ӵ�)
            float totalSpeed = GameManager.globalSpeed * speedRate * Time.deltaTime * -1f;
            //Ʈ������ �̵����� ��ŸŸ�� �ʼ�
            transform.Translate(totalSpeed, 0f, 0f); // ��ŸŸ�� / ���� �������� �̵� (���� x����)
        }

        

    }
}
