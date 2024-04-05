using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable] //클래스 직렬화
public class Sound // 이 안의 내용을 변수로 사용가능 ex == public Sound A;
{
    public string name; // 사운드 이름
    public AudioClip clip; // 사운드 파일
}




//게임 씬 관리 & 스코어 & 사운드 관리 & 스테이터스 관리
public class GameManager : MonoBehaviour
{
    public static GameManager I; // 싱글톤화


    //##사운드 관리##
    public AudioSource[] audioSourcesEffects; // 효과음 재생기 배열 (동시에 여러개 재생가능)
    public AudioSource audioSourceBgm; // 배경음 재생기


    public string[] playSoundName; //특정 사운드 정지용

    //class Sound 
    public Sound[] effectSounds; // 효과음 파일 배열
    public Sound[] bgmSounds; // 배경음 파일 배열
 
    


    //##던전 스코어 관리##
    const float ORIGIN_SPEED = 3; // 상수 (불변수치)
    public static float globalSpeed; // 변수 메모리에 얹기
    public static float score; // 점수



    /*
    //##스테이터스 관리##

    // 체력
    [SerializeField]
    private int hp;  // 최대 체력. 유니티 에디터 슬롯에서 지정할 것.
    private int currentHp;

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
    private Image[] images_Gauge;
    // 각 상태를 대표하는 인덱스
    private const int HP = 0, MP = 1, DP = 2, WT = 3;

    private TextMeshProUGUI[] Txts_Gauge;
    private const int HPTXT = 0, MPTXT = 1, DPTXT = 2, WTTXT = 3;


    //상태변수
    public static bool canPickUp = true; //무게 상태 (소지품 한계)
    */

    public static bool isLive; // 게임 진행 상태

    void Awake()
    {

        isLive = true; // 게임 시작

        if (I == null)
        {
            I = this; //싱글톤화
            DontDestroyOnLoad(gameObject); //파괴금지
        }
        else
            Destroy(gameObject); // 씬 이동 시 중복생성 방지





        if (!PlayerPrefs.HasKey("Score")) // 데이터 관리에서 스코어 정보가 없는경우
            PlayerPrefs.SetFloat("Score", 0); // 스코어 0으로 일단 저장
    }


    private void Start()
    {
        /*
        //스테이터스 초기화
        currentHp = hp;
        currentMp = mp;
        currentDp = 0;
        */
    }



    void Update() //스코어 & 마나 갱신
    {
        if (!isLive) //죽은경우에는 함수 아래 작동 x
            return;

        if(!Inventory.inventoryActivated && !EventContoroller.randomEvent)
        {
            score += Time.deltaTime * 2; //시간에 따라 점수부여 
            globalSpeed = ORIGIN_SPEED + score * 0.01f; // 점수에 따라 스크롤링 속도 증가

            
            //IsMP(); // 마나 갱신
        }



        /*
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
        */

    }

    // 플레이어 사망 & 스코어 갱신
    public void GameOver() 
    {
        isLive = false; // 게임상태 off

        float highScore = PlayerPrefs.GetFloat("Score");
        PlayerPrefs.SetFloat("Score", Mathf.Max(highScore, score)); //Mathf.Max 두 값을 비교하여 큰 값 반환
    }

    public void EscapeGameOver()
    {
        isLive = false; // 게임상태 off

        float highScore = PlayerPrefs.GetFloat("Score");
        //스코어 저장+소지하고 있던 아이템 저장
    }

    //##스코어 증감##
    public void IncreaseScore(float _num)
    {
        score += _num;
    }

    public void DecreaseScore(float _num)
    {
        score -= _num;
        if (score <= 0)
            score = 0;
    }





    /*
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


        if (currentMp < 0)  //현재 MP 수치가 0 이하라면 (위 if 가 0보다 큰 경우이므로)
                            //플레이어가 죽어서 게임이 종료되는 등등의 처리를 해주면 좋을 것 같다.
            Debug.Log("MP 수치가 0 이 되었습니다.");

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
            GameOver();
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

    */

    //##사운드##

    //효과음 재생
    public void PlaySE(string _name) // 넘어온 문자열에 일치하는 Sound.name을 찾고 Sound.clip을 오디오 소스에 넣어 재생
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                //재생중인 오디오 소스는 두고 & 재생중이 아닌 오디오소스에서만 플레이
                for (int y = 0; y < audioSourcesEffects.Length; y++)
                {
                    if(!audioSourcesEffects[y].isPlaying) //조건 - 재생중이 아닌 오디오소스만 
                    {
                        playSoundName[y] = effectSounds[i].name; //요청받은 사운드를 playSoundName배열에 입력
                        audioSourcesEffects[y].clip = effectSounds[i].clip; // 재생중이 아닌 오디오 소스에 = 위에서 조건을 만족한 배열 번째의 클립을 넣음
                        audioSourcesEffects[y].Play(); // 클립 재생
                        return; //함수종료
                    }
                }
                Debug.Log("모든 가용 AudioSource 사용중"); //뜨면 오디오 소스 늘려야함
                return;
            }
        }
        Debug.Log(_name + "사운드가 GameManager에 없음"); //효과음이 없거나 사운드 이름 틀림
    }

    //모든 효과음 재생취소
    public void StopAllSE()
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            audioSourcesEffects[i].Stop();
        }
    }

    //특정 효과음 재생취소
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            if(playSoundName[i] == _name) //들어온 스트링값이 일치하는 경우의 i만
            {
                audioSourcesEffects[i].Stop(); //재생 정지
                return;
            }

        }
        Debug.Log("재생 중인" + _name + "사운드가 없음"); //재생중 효과음 없거나 사운드 이름 틀림
    }

}
