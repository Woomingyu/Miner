using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : Slot
{
    [SerializeField]
    private Slot[] quickSlots; //퀵슬롯의 슬롯
    [SerializeField]
    private Image[] img_CoolTime; //쿨타임 이미지
    [SerializeField]
    private Transform tf_parent; //퀵슬롯의 부모객체 (퀵슬롯 그리드세팅)



    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>(); //그리드세팅이 퀵슬롯 자식객체의 slot.cs만 획득
    }

    // Update is called once per frame
    void Update()
    {
        if(isCooltTime)
            CoolTimeCalc(); // 쿨타임 계산
    }

    private void CoolTimeCalc()
    {
            Debug.Log("작동됨");
            currentCoolTime -= Time.deltaTime;
            for (int i = 0; i < img_CoolTime.Length; i++)
            {
                img_CoolTime[i].fillAmount = currentCoolTime / coolTime; // 쿨타임 수치 게이지 반영
            }
            if (currentCoolTime <= 0)
                isCooltTime = false;      
    }


   
}
