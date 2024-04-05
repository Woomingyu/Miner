using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EventContoroller : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    //*나온 이벤트는 다시 나오지 않도록 할것

    //이미지
    public GameObject EventPopup; // 이벤트 창 on off용
    public GameObject[] EventImage; //이벤트 목록

    //UI 팝업 트위닝
    private Vector2 targetPos = new Vector2(25, -5);

    //문자열 리스트 = 이벤트 리스트 ((이벤트 추가 시 해당 리스트에 추가할것))
    List<string> EventList = new List<string>() { "EventImage1", "EventImage2", "EventImage3", "EventImage4", "EventImage5" };

    //상태값
    public static bool randomEvent = false; // 이벤트 상태값


    private void Start()
    {
        CloseEvent();
    }
    //이벤트 호출
    public void CallEvent()
    {
        randomEvent = true; //상태 on

        int RandomInt = Random.Range(0, EventList.Count); // 정수값 랜덤인트 = 랜덤값(0~이벤트 리스트의 개수)이다

        if (EventList.Count <= 0)
        {
            Debug.Log("이벤트 없음");
            //뭔가 효과를 주거나 반복 이벤트를 주는게 좋을듯
            //임시로 이벤트 닫기만 실행함
            CloseEvent();
            return;
        }


        EventPopup.SetActive(true);// 이벤트 전체 창 활성화

        //트위닝 이벤트UI 등장&통통튀기

        for (int i = 0; i < EventImage.Length; i++) 
        {


            if (EventList[RandomInt] == "EventImage" + (i + 1)) // 만약  (이벤트 리스트에서 랜덤값이 이벤트 이미지+ (i+1) 과 같으면)=이벤트 이미지는1부터 시작하므로 1을 더해준다.
            {
                EventImage[i].SetActive(true); // 이벤트 이미지 세팅값 참

                EventImage[i].transform.DOLocalMove(targetPos, 1).SetEase(Ease.InExpo).SetEase(Ease.OutBounce); //닷트윈 모션
            }
                

        }

        print(EventList[RandomInt]); // 나온 이벤트 디버그
        EventList.RemoveAt(RandomInt); //이미 나온 이벤트를 제거(리스트에서 뺌)





        /*
        for (int i = 0; i < EventList.Count; i++) // 남은 이벤트 다 나올때까지 반복
        {
            print("EventList : " + i + "번째 " + EventList[i]);  //남은 이벤트 값 출력
        }
        */
        //Time.timeScale = 0; // 정지

        //##이벤트##
    }

    //이벤트 선택지

    //피해를 받아서 이벤트 종료
    public void DamageBtn() //랜덤값이 0일 경우에만 치명적 데미지 줌
    {
        if (Random.Range(0, 5) == 0)
            CriticalDamage();
        else
            RandomDamage();

    }



    //##선택지 효과 목록들##


    //스테이터스 관련
    public void RandomDamage()
    {
        int RandomInt = Random.Range(0, 50);
        player.DecreaseHP(RandomInt);
        Debug.Log(RandomInt + "의 일반 피해 입음");
        //이벤트 결과 텍스트창 띄워주고 거기서 버튼 누르면 이벤트 닫기하면 될듯
        //임시로 이벤트 닫기 함수 호출
        CloseEvent();
    }

    public void CriticalDamage()
    {
        int RandomInt = Random.Range(10, 100);
        player.DecreaseHP(RandomInt);
        Debug.Log(RandomInt + "의 치명적 피해 입음");
        CloseEvent();
    }

    public void RandomMPDamage()
    {
        int RandomInt = Random.Range(0, 50);
        player.DecreaseMP(RandomInt);
        CloseEvent();
    }

    public void CriticalMPDamage()
    {
        int RandomInt = Random.Range(50, 500);
        player.DecreaseMP(RandomInt);
        CloseEvent();
    }

    public void GetDP()
    {
        int RandomInt = Random.Range(1, 5);
        player.DecreaseDP(RandomInt);
        CloseEvent();
    }

    //아이템 관련

    public void GetItem()
    {
        //inventory.AcquireItem("얻을 아이템정보".GetComponent<ItemPickUp>().item);
        //GameManager.I.IncreaseWT("얻은 아이템 정보".GetComponent<ItemPickUp>().item.itemWeight); //무게 추가 (광물이 아니면 이건 지워도 됨)
    }

    //일반 이벤트 종료
    public void CloseEvent()
    {
        randomEvent = false; //상태 off

        EventPopup.SetActive(false); //이벤트 전체 창 비활성화

        for (int i = 0; i < EventImage.Length; i++) //이벤트 목록 전체 비활성화
        {
            EventImage[i].SetActive(false);
        }
        //Time.timeScale = 1; // 시작

    }
}
