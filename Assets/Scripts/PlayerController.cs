using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; // 터치 상호작용 있는 스크립트에 추가((UI 이중터치 방지용)) == 점프조작/스킬체크
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //지정 값 열거형 [애니메이션 열거임]
    public enum State { Stand, Run, Jump, Hit, Dead } // 배열순서 == int값 0,1,2,3..


    //#수치 변수#
    public float startJumpPower; //숏점프 롱점프 롱점프 리셋용
    public float jumpPower;
    public float resetJump; //롱점프 리셋용 = jumpPower와 동일수치 입력

    //#상태 변수#
    public bool isGround;
    public bool isJumpKey; // 키 씹힘 방지용 (숏,롱 점프 둘다 같은 인풋값 사용 시 씹힘 발생가능)





    //#필요한 컴포넌트#
    Rigidbody2D rigid;
    Animator anim;
    public UIManager uiManager; // 스킬체크 UI ON/OFF
    public UnityEvent onHit; // 피격 이벤트
    Sounder sound; // 사운드 
    public GameObject uiOver; //게임오버 UI
    public GameObject uiEscapeOver; //탈출 게임오버UI

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private TextMeshProUGUI PickUpTxt; //머리 위 아이템 획득정보 txt

    [SerializeField]
    private GameObject potion;
    public static Ore CurrentOre;

    public Ore[] ores;

    //##스테이터스 관리##

    // 체력
    [SerializeField]
    private int hp;  // 최대 체력. 유니티 에디터 슬롯에서 지정할 것.
    private int currentHp;

    // 체력 감소속도
    [SerializeField]
    private int hpDecreaseTime; 
    private int currentHpDecreaseTime;

    // 마나
    [SerializeField] // 필드 정렬
    private int mp; // 최대 MP 유니티 에디터 슬롯에서 지정할 것.
    private int currentMp; //현재 MP

    //마나 감소속도
    [SerializeField] // 필드 정렬
    private int mpDecreaseTime; // MP 감소하는 시간
    private int currentMpDecreaseTime; //현재 MP 감소 시간


    // 방어력
    [SerializeField]
    private int dp;  // 최대 방어력. 유니티 에디터 슬롯에서 지정할 것.
    private int currentDp;

    //무게
    [SerializeField]
    private int Weight; // 최대 무게한도
    [SerializeField]
    private int currentWeight; // 현재 무게상태


    // 필요한 이미지
    [SerializeField]
    private Image[] images_Gauge;
    // 각 상태를 대표하는 인덱스
    private const int HP = 0, MP = 1, DP = 2, WT = 3;
    [SerializeField]
    private TextMeshProUGUI[] Txts_Gauge;
    private const int HPTXT = 0, MPTXT = 1, DPTXT = 2, WTTXT = 3;
    //상태변수
    public static bool canPickUp = true; //무게 상태 (소지품 한계)

    void Awake() //컴포넌트 초기화용 start는 객체 활성화 상태 on off에도 작동하지만 Awake는 단 한번 작동
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<Sounder>();
        uiManager = GetComponent<UIManager>();

    }

    private void Start()
    {
        sound.PlaySound(Sounder.Sfx.Reset); // 사운드 요청
        
        //스테이터스 초기화
        currentHp = hp;
        currentMp = mp;
        currentDp = 0;

    }


    void Update()
    {
        if (!GameManager.isLive) //게임오버 상태
        {
            rigid.simulated = false;
            //sound.PlaySound(Sounder.Sfx.Hit); // 사운드 요청
            ChangeAnim(State.Dead);
            onHit.Invoke(); // onhit 이벤트에 연결된 함수 호출
        }

        
        else
        {
            if ((Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) && isGround && !UIManager.isMinning && !EventSystem.current.IsPointerOverGameObject() && !Inventory.inventoryActivated && !EventContoroller.randomEvent) // 숏점프(땅에서만 작동) & UI범위 제외 추가
            {
                rigid.AddForce(Vector2.up * startJumpPower, ForceMode2D.Impulse); // ForceMode 폭발형
            }

            isJumpKey = Input.GetButton("Jump") || Input.GetMouseButton(0); //조건 만족 시 true로 업데이트 

            if (!Inventory.inventoryActivated && !EventContoroller.randomEvent)
            {
                IsMP(); // 마나 갱신
            }



            //무게 업데이트
            if (currentWeight <= Weight)
            {
                canPickUp = true;
            }
            else
            {
                canPickUp = false;
                Debug.Log("캐릭터의 무게가 가득 찼습니다!!");
            }

            GaugeUpdate(); // 실제 UI 게이지 갱신
                           //Debug.Log(score);

            if(currentHp <= 0)
            {
                uiOver.SetActive(true); // 게임오버 UI활성화
                GameManager.I.GameOver();
            }

        }



    }
    private void FixedUpdate()
    {
        if (!GameManager.isLive)
            return;

        if (isJumpKey && !isGround && !UIManager.isMinning && !Inventory.inventoryActivated && !EventContoroller.randomEvent) // 롱점프(점프중 작동)
        {
            jumpPower = Mathf.Lerp(jumpPower, 0, 0.05f); //변경값 , 목표값, 목표값으로 변경되는 텀
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // ForceMode 폭발형
        }
    }



    //##점프 관련##

    //착지(바닥 판정)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!isGround) // 떨어졌다 닿았을 때만 한번 실행
        {
            ChangeAnim(State.Run);
            sound.PlaySound(Sounder.Sfx.Land); // 사운드 요청
            jumpPower = resetJump; //착지 후 롱점프 힘 초기화
        }


        
        
        isGround = true;
    }

    //체공(허공 판정)
    private void OnCollisionExit2D(Collision2D collision)
    {
        ChangeAnim(State.Jump);
        sound.PlaySound(Sounder.Sfx.Jump); // 사운드 요청
        isGround = false;
    }






    //장애물 / 이벤트 / 적 (트리거 콜라이더) // 지금은 사망판정임
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //광물
        if (collision.transform.tag == "Ore") 
        {
            DecreaseHP(5);
            UIManager.isMinning = true;

            CurrentOre = collision.gameObject.GetComponent<Ore>();
        }

        //아이템 = 소모품 ((이거 필요없는거같음))
        if (collision.transform.tag == "Used") // 감지된 콜라이더 태그 소모품(포션)
        {
            //획득 아이템 인벤토리로
            inventory.AcquireItem(potion.transform.GetComponent<ItemPickUp>().item);

            //아이템 획득 안내 txt
            PickUpTxt.gameObject.SetActive(true);
            PickUpTxt.text = potion.transform.GetComponent<ItemPickUp>().item.itemName + "<color=red> " + "</color>" + "획득";

            Invoke("HidePickUpTxt", 1.5f);
        }


        //체력 & 마나 우물
        if (collision.transform.tag == "HPWell")
        {
            IncreaseHP(100);
            collision.gameObject.GetComponent<DugeonObjectController>().UsedWell();
        }

        if (collision.transform.tag == "MPWell")
        {
            IncreaseMP(100);
            collision.gameObject.GetComponent<DugeonObjectController>().UsedWell();
        }

        //랜덤 이벤트
        if (collision.transform.tag == "RandomEvent")
        {
            collision.gameObject.GetComponent<EventContoroller>().CallEvent();
        }

        //상점
        if (collision.transform.tag == "Store")
        {
            inventory.OpenShop();
        }
        //깨시(피해를 주는 함정객체)

        //탈출 스크롤
        if (collision.transform.tag == "EscapeScroll")
        {
            uiEscapeOver.SetActive(true); // 탈출게임오버 UI활성화
            GameManager.isLive = false;
            //게임매니저의 탈출 게임오버 불러오기          
        }


        //사망코드
        /*
        rigid.simulated = false;
        sound.PlaySound(Sounder.Sfx.Hit); // 사운드 요청 
        ChangeAnim(State.Dead);
        onHit.Invoke(); // onhit 이벤트에 연결된 함수 호출
        */
    }
    //애니메이션 관리
    void ChangeAnim(State state)
    {
        anim.SetInteger("State", (int)state); //영시적 형변환 State => int
        
    }

    //획득 안내 텍스트 숨기기
    private void HidePickUpTxt()
    {
        PickUpTxt.gameObject.SetActive(false);

    }



    //##스테이터스##
    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp; //체력
        images_Gauge[MP].fillAmount = (float)currentMp / mp; //마나
        images_Gauge[DP].fillAmount = (float)currentDp / dp; //방어력


        //텍스트 업데이트
        Txts_Gauge[HPTXT].text = currentHp + " / " + hp; // 현재 체력 / 최대 체력 표시
        Txts_Gauge[MPTXT].text = currentMp + " / " + mp; // 현재 마나 / 최대 마나 표시
        Txts_Gauge[DPTXT].text = currentDp + " / " + dp; // 현재 방어력 / 최대 방어력 표시
        Txts_Gauge[WTTXT].text = currentWeight + " / " + Weight; // 현재 무게 / 최대 무게 표시
    }

    private void IsMP()  //업데이트 문 안에서 매 프레임마다 실행 된다.
                         //매프레임마다 일정 시간 씩 지나면 자연스럽게 MP 수치가 떨어지도록 할 것이다.
    {

        if (currentMp > 0 && !Inventory.inventoryActivated && !EventContoroller.randomEvent) // 현재 MP 수치가 0 보다 크다면
        {
            if (currentMpDecreaseTime <= mpDecreaseTime) // [현재 MP 감소시간]이 [MP 감소시간]과 같거나 작다면
                currentMpDecreaseTime++; // [현재 MP 감소시간]을 계속 1씩 더해줄 것이다

            else //[현재 MP 감소시간]이 [MP 감소시간]에 도달했다면
            {
                currentMp--; // MP 수치를 감소시키고
                currentMpDecreaseTime = 0; // [현재 MP 감소시간] 을 0으로 되돌린다.
            }
        }


        if (currentMp <= 0 && !Inventory.inventoryActivated && !EventContoroller.randomEvent)  //현재 MP 수치가 0 이하라면 (위 if 가 0보다 큰 경우이므로)
        {
            if (currentHpDecreaseTime <= hpDecreaseTime) // [현재 HP 감소시간]이 [HP 감소시간]과 같거나 작다면
                currentHpDecreaseTime++; // [현재 HP 감소시간]을 계속 1씩 더해줄 것이다

            else //[현재 HP 감소시간]이 [HP 감소시간]에 도달했다면
            {
                currentHp--; // HP 수치를 감소시키고
                currentHpDecreaseTime = 0; // [현재 HP 감소시간] 을 0으로 되돌린다.
            }
        }                                                                                      
    }



    //##스테이터스 증감 처리##

    public void IncreaseHP(int _count) // HP 증가 처리
    {
        if (currentHp + _count < hp) //현재 체력에서 인수로 들어온 _count 만큼 증가 (최대체력 미만인 경우) 
            currentHp += _count; // 현재 체력에서 인수로 들어온 _count 만큼 증가시킨다.
        else
            currentHp = hp; //최대치 오버 방지용
    }

    public void DecreaseHP(int _count) //HP 감소 처리 * 단 DP 가 존재한다면 DP를 우선해서 깎도록

    {
        if (currentDp > 0) //방어력이 없을 때만 체력을 깎는다.
        {
            DecreaseDP(_count); //방깎 후 리턴
            return;
        }
        currentHp -= _count;

        if (currentHp <= 0) //체력 고갈
        {
            uiOver.SetActive(true); // 게임오버 UI활성화
            GameManager.I.GameOver();
        }


        //Debug.Log("캐릭터의 체력이 0이 되었습니다!!");
    }


    public void IncreaseMP(int _count) //MP 증가 처리
    {
        if (currentMp + _count < mp) // 현재 MP 에서 _count를 더했을 때 최대 MP 보다 작은 경우
            currentMp += _count; // 현재 MP 에서 인수로 들어온 _count 만큼 그대로 더해준다

        else // 들어온 인수가 최대 MP를 넘는경우
            currentMp = mp; // 최대치 오버 방지용

    }

    public void DecreaseMP(int _count) //MP 감소 처리
    {
        if (currentMp - _count < 0) //현재 MP 에서 _count를 뺀 값이 0보다 작은경우
            currentMp = 0; // 현재 MP = 0
        else
            currentMp -= _count; // 최대치 오버 방지용
    }



    public void IncreaseDP(int _count)  // DP 증가 처리
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    public void DecreaseDP(int _count) // DP 감소 처리
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("캐릭터의 방어력이 0이 되었습니다!!");
    }

    public void IncreaseWT(int _Weight)  // WT 증가 처리 (아이템 개수, 아이템 무게)
    {
        if (currentWeight + _Weight <= Weight) //현재 무게에 들어온 인수를 더한값이 최대무게보다 작은경우
        {
            currentWeight += _Weight; //인수 그대로 더하기
        }

        else
        {
            canPickUp = false;
            Debug.Log("캐릭터의 무게가 가득 찼습니다!!"); //값이 최대무게보다 큰 경우
        }
    }

    public void DecreaseWT(int _Weight) // WT 감소 처리
    {
        currentWeight -= _Weight; // 들어온 인수 빼주기

        if (currentWeight <= 0) // 만약 현재 무게가 0보다 작다면
            currentWeight = 0; // 무게 -방지
    }

    //재시작
    public void ReStart()
    {
        SceneManager.LoadScene(0);

        for (int i = 0; i < ores.Length; i++)
        {
            ores[i].ResetOre();
        }
        GameManager.score = 0;
        GameManager.isLive = true;

    }

   
    //마을로
    public void Return()
    {
        SceneManager.LoadScene(1);
    }
    // 1. 점프(점프파워)
    // 2. 착지 (물리 충돌 이벤트)
    // 3. 장애물 터치(트리거 충돌 이벤트)
    // 4. 애니메이션
    // 5. 사운드
}
