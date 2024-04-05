using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RePosition : MonoBehaviour
{
    public UnityEvent onMove;

    //후처리
    private void LateUpdate()
    {
        if (transform.position.x > -17) //맵 생성 기준치에 못미치는 경우
            return; // 아무것도 반환 안하고 넘어감 (함수 내 아래 코드들이 실행 x)

        //되돌아가기
        transform.Translate(20, 0, 0, Space.Self);
        onMove.Invoke(); // 이벤트 함수 호출(맵 생성시 배경 스프라이트 체인지)
    }

}
