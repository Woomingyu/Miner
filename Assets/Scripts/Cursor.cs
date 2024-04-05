using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Cursor : MonoBehaviour
{
    //필요한 요소
    [SerializeField]
    private GameObject cursor; // 커서 오브젝트
    [SerializeField]
    private LayerMask layerMask; // base / point 구분용
    [SerializeField]
    private TextMeshProUGUI PointTxt; // 포인트 지점 텍스트 표기용 /텍메프
    private Ore ore; //광물 함수호출용
    private Rigidbody2D rigid;
    [SerializeField]
    private PlayerController player;
    //변수
    Vector2 currentPos; // 현재 위치 저장
    public float speed; // 커서 속도
    public float MaxPos; // 커서 이동 최대치

    //레이캐스트
    [SerializeField]
    private float range; //레이범위 (커서 판정범위)
    [SerializeField]
    private Vector2 rayStart; //레이 시작위치 (커서 판정 시작위치)

    //사운드
    [SerializeField]
    private string hit_Sound; // 피격사운드

    //상태변수

    public static bool coolTime; // 스킬체크 쿨타임(중복실행 방지) *바위 파괴 시 상태변수 꼭 바꿀것
    void Start()
    {
        ore = FindObjectOfType<Ore>();
        rigid = GetComponent<Rigidbody2D>();
        currentPos = transform.localPosition; // 위치 저장
        
    }
    private void FixedUpdate()
    {
        if (UIManager.isMinning && !Inventory.inventoryActivated)
        {
            cursorMove();
            TryCheck();
        }
    }



    //스킬체크 커서 이동&속도제어
    private void cursorMove()
    {
        //##시간에 따라 증가하는 것이 아닌 랜덤속도 부여로 수정할것## 혹은 등급에 따라 난이도 조정
        Vector2 pos = currentPos;
        speed += Time.deltaTime * 0.001f; //시간에 따른 커서 이동속도 상승 ==> 스코어에 따른 속도 상승으로 변경 
        pos.x += MaxPos * Mathf.Sin(Time.time * speed); // 이동, 최대치 도달시 반전(중간은 빠르게 끝에선 천천히)
        transform.localPosition = pos;
    }


    private void TryCheck()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) && UIManager.isMinning)
        {
            if (!coolTime)
            {
                StartCoroutine("CheckPointCoroutine");
            }
        }            
    }

    /** 커서 포인트 체크 레이캐스트 */
    IEnumerator CheckPointCoroutine()
    {
        coolTime = true;

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position * rayStart, Vector2.right, range, LayerMask.GetMask("Point"));
        //레이캐스트가 맞으면 반응하는것 // 2D레이캐스트(시작 위치/백터 방향값 / 거리값/ 감지할 레이어태그)
        Debug.DrawRay(rigid.position * rayStart, Vector2.right * range, new Color(0, 0, 1));
        //Debug.DrawRay(rigid.position, -dir, new Color(0, 0, 1));

        //레이캐스트가 보이게 만들어주는 디버그(리지드바디 위치받아오기/백터 방향값, 레이색)

      
        //성공 실패 판별 코루틴
            if (rayHit.collider != null) // 빔을 쏴서 맞으면 콜라이더가 있다
        {

            PlayerController.CurrentOre.OreMining();

        }                                   
            else
            {
            GameManager.I.PlaySE(hit_Sound);
            player.DecreaseHP(10);
            //스테이터스 => 체력감소 함수 호출
            }

        yield return new WaitForSeconds(2f);
        //RaycastHit2D raycastHit = Physics2D.Raycast(transform.localPosition, transform.forward, 1f, LayerMask.GetMask("Point"));
        //Physics2D.BoxCast(Box2D.bounds.center, Box2D.bounds.size, 0f, Vector2.down * 1.0f, 0f, LayerMask.GetMask("Point")); //레이캐스트 쏴서 포인트 체크

        coolTime = false;
    }



}



