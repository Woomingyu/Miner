using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : Slot
{
    [SerializeField]
    private Slot[] quickSlots; //�������� ����
    [SerializeField]
    private Image[] img_CoolTime; //��Ÿ�� �̹���
    [SerializeField]
    private Transform tf_parent; //�������� �θ�ü (������ �׸��弼��)



    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>(); //�׸��弼���� ������ �ڽİ�ü�� slot.cs�� ȹ��
    }

    // Update is called once per frame
    void Update()
    {
        if(isCooltTime)
            CoolTimeCalc(); // ��Ÿ�� ���
    }

    private void CoolTimeCalc()
    {
            Debug.Log("�۵���");
            currentCoolTime -= Time.deltaTime;
            for (int i = 0; i < img_CoolTime.Length; i++)
            {
                img_CoolTime[i].fillAmount = currentCoolTime / coolTime; // ��Ÿ�� ��ġ ������ �ݿ�
            }
            if (currentCoolTime <= 0)
                isCooltTime = false;      
    }


   
}
