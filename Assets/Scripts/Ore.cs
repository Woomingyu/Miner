using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ore : MonoBehaviour
{

    [SerializeField]
    private int hp; // 바위의 체력
    private int resetHp; // 바위체력 리셋용


    //필요한 콜라이더

    //광물 아이템 오브젝트 & 생성 개수 & 등장 위치 & 획득 안내 & 인벤토리 연동
    [SerializeField]
    private GameObject go_ore_item_prefab; // 광물 아이템
    [SerializeField]
    private int count;
    [SerializeField]
    private Transform Ore_ItemPos;
    [SerializeField]
    private TextMeshProUGUI PickUpTxt; //머리 위 아이템 획득정보 txt
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private PlayerController player;


    //콜라이더와 리지드바디를 이용해 자동으로 흩어지고 깨지는바위를 구현했음
    [SerializeField]
    private GameObject go_effect_prefabs; // 채굴 이펙트


    //필요한 사운드 이름((사운드 매니저에서 오디오소스 알아서 틀어줌)
    [SerializeField]
    private string strike_Sound; // 광물 피격 사운드
    [SerializeField]
    private string destroy_Sound; // 광물 파괴 사운드

    private void Start()
    {
        resetHp = hp;
    }




    public void OreMining() //광물 부수는중
    {
        
        GameManager.I.PlaySE(strike_Sound); //사운드매니저에서 요청재생
        /*
        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        go_effect_prefabs.GetComponent<Rigidbody2D>().AddForce((Vector2.one) * 3); // 파편 생성 시 가할 힘
       
        //생성(생성하는것, 콜라이더의 중심, 회전값.기본값으로)
        Destroy(clone, destroyTime); // 이펙트 파편파괴
         */


        hp--; // 무기에 따라 추가 hp 피해를 주는것을 추가하는게 좋을듯
        if (hp <= 0) //바위체력 1씩 감소시키고 0보다 작거나 같아지면
            Destruction(); //파괴함수 실행
      
    }

    private void Destruction() //광물 비활성 & 획득(자동)
    {       
        this.gameObject.SetActive(false); // 이 게임 오브젝트 비활성화
        GameManager.I.PlaySE(destroy_Sound); //사운드매니저에서 요청재생
        UIManager.isMinning = false;
        Cursor.coolTime = false;

        hp = resetHp; //광물 체력 초기화 (파괴 아니라서)
        //UIManager.OffMinningUI();

        //여기서 아이템 프리팹 설정해서 넣으면 == 광물 종류에 따라 아이템 들어가게 만들 수 있음
        
            
            for (int i = 0; i < count; i++) //count 만큼 돌맹이 아이템 생성
        {



            GameObject OreClone = Instantiate(go_ore_item_prefab, Ore_ItemPos.position, Quaternion.identity);
            OreClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 10);
            Destroy(OreClone, 0.5f); //광물 아이템 파괴


            player.IncreaseWT(OreClone.transform.GetComponent<ItemPickUp>().item.itemWeight); //무게 추가

            if (!PlayerController.canPickUp) //현재무게 + 광물아이템의 무게 >= 최대무게)
                return;

            //획득 아이템 인벤토리로
            inventory.AcquireItem(OreClone.transform.GetComponent<ItemPickUp>().item);
            /*
            //아이템 획득 안내 txt
            PickUpTxt.gameObject.SetActive(true);
            PickUpTxt.text = "<color=red> " + OreClone.transform.GetComponent<ItemPickUp>().item.itemName + "</color>" + "획득";
            */

        }

            Invoke("HidePickUpTxt", 1.5f);


        


    }

    private void HidePickUpTxt()
    {
        PickUpTxt.gameObject.SetActive(false);

    }

    //게임 리셋 시 불러오기
    public void ResetOre()
    {
        this.gameObject.SetActive(false); // 이 게임 오브젝트 비활성화
        UIManager.isMinning = false;
        Cursor.coolTime = false;

        hp = resetHp; //광물 체력 초기화 (파괴 아니라서)
    }
}

