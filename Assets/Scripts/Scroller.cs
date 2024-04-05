using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{

    public int count; // 생성 그룹 내 자식 수
    public float speedRate; // 맵 스크롤링 속도%



    void Start()
    {
        count = transform.childCount; // 트랜스폼에서 자식객체 수 받아오기 (get함수획득은 레거시임)
    }

    void Update()
    {
        if (!GameManager.isLive)
            return;


        if(!UIManager.isMinning && !Inventory.inventoryActivated && !EventContoroller.randomEvent)
        {
            //맵 스크롤링 속도(=이동속도)
            float totalSpeed = GameManager.globalSpeed * speedRate * Time.deltaTime * -1f;
            //트랜스폼 이동값은 델타타임 필수
            transform.Translate(totalSpeed, 0f, 0f); // 델타타임 / 지정 방향으로 이동 (벡터 x값만)
        }

        

    }
}
