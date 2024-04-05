using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//현재 아이템 Input 하면 버리는 코드로 되어있음 ==> input값 입력시 판매로 변경
//로직 : Call 호출 -> 드랍UI 활성화 & 안내 텍스트 아이템 최대값으로 설정 ->
//CheckNumber 에서입력값의 상태 판별(숫자만, 문자열 포함, 입력x) ->
//OK 에서 입력값 상태에 따른 num 값 반환 -> DropItemCoroutine에서 num값에 따라 아이템 생성 & 슬롯값 null변경
public class InputNumber : MonoBehaviour
{
    [SerializeField]
    private Text text_Preview; // 프리뷰 텍스트 미리보기(보유 개수 보여줌)
    [SerializeField]
    private Text text_Input; // 실제 입력값
    [SerializeField]
    private InputField if_text; // 유저 인풋값 (이전 인풋값 초기화용)

    [SerializeField]
    private GameObject go_Base; // 아이템 드랍 UI base

    [SerializeField]
    private PlayerController playerPos; //아이템 드랍 위치용


    //버리기 입력창 호출
    public void Call()
    {
        go_Base.SetActive(true); //입력창 활성화
        if_text.text = ""; // 텍스트 입력값 초기화 (이전에 입력했던 텍스트가 안남도록)

        //기본 표기 텍스트에  드래그한 아이템 갯수를 대입(문자열로 변환)
        //호출과 동시에 텍스트가 아이템의 최대개수로 설정됨 == 모두 버림
        text_Preview.text = DragSlot.instance.dragSlot.itemCount.ToString();

    }
    //버리기 취소
    public void Cancel()
    {
        go_Base.SetActive(false); //입력창 비활성화
        DragSlot.instance.SetColor(0); // 드래그 아이템 투명화
        DragSlot.instance.dragSlot = null; //드래그 아이템 초기화
    }

    public void OK()
    {
        DragSlot.instance.SetColor(0); //이미지 투명화/ok를 누르면 드래그한 이미지는 사라지는게 자연스러움

        int num;
        if (text_Input.text != "")
        {
            if (CheckNumber(text_Input.text)) //text_Input.text 안에 문자가 하나라도 있는지 확인
            {
                num = int.Parse(text_Input.text); // 숫자가 맞는 경우  입력값을 int형으로 바꿔줌

                if (num > DragSlot.instance.dragSlot.itemCount) // 입력값이 최대 아이템 개수보다 많으면
                    num = DragSlot.instance.dragSlot.itemCount; // 입력값을 최대값으로 변경
            }
            else // 문자, 숫자 외 인경우
            {
                num = 1; // 문자 입력시 하나만 버리게 설정
                         // 편의성에 따라 버리지 못하게 하거나 전부 버리거나 등등 조정
            }
        }
        else // 아무것도 적지 않은 경우
            num = int.Parse(text_Preview.text); // 최대개수 만큼 입력

        StartCoroutine(DropItemCoroutine(num)); // 입력 끝났으면 버리기 코루틴 실행


    }

    //실제 버리기 코루틴(입력값만큼 버리기 반복)
    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            playerPos.DecreaseWT(DragSlot.instance.dragSlot.item.itemWeight); //무게 감소                                                                                 
            PlayerController.canPickUp = true;//아이템 획득 가능
            Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, playerPos.transform.position + playerPos.transform.forward, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1); //인벤토리 아이템의 개수 감소
            yield return new WaitForSeconds(0.05f); //각 생성의 대기시간
        }

      
        DragSlot.instance.dragSlot = null;
        go_Base.SetActive(false);
    }



    //입력값에 문자가 하나라도 포함되었는지 판별
    private bool CheckNumber(string _argString)
    {
        double tempDouble;
        return Double.TryParse(_argString, out tempDouble);
    }
}
