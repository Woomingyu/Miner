using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; // ��ġ ��ȣ�ۿ� �ִ� ��ũ��Ʈ�� �߰�((UI ������ġ ������)) == ��������/��ųüũ
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //���� �� ������ [�ִϸ��̼� ������]
    public enum State { Stand, Run, Jump, Hit, Dead } // �迭���� == int�� 0,1,2,3..


    //#��ġ ����#
    public float startJumpPower; //������ ������ ������ ���¿�
    public float jumpPower;
    public float resetJump; //������ ���¿� = jumpPower�� ���ϼ�ġ �Է�

    //#���� ����#
    public bool isGround;
    public bool isJumpKey; // Ű ���� ������ (��,�� ���� �Ѵ� ���� ��ǲ�� ��� �� ���� �߻�����)





    //#�ʿ��� ������Ʈ#
    Rigidbody2D rigid;
    Animator anim;
    public UIManager uiManager; // ��ųüũ UI ON/OFF
    public UnityEvent onHit; // �ǰ� �̺�Ʈ
    Sounder sound; // ���� 
    public GameObject uiOver; //���ӿ��� UI
    public GameObject uiEscapeOver; //Ż�� ���ӿ���UI

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private TextMeshProUGUI PickUpTxt; //�Ӹ� �� ������ ȹ������ txt

    [SerializeField]
    private GameObject potion;
    public static Ore CurrentOre;

    public Ore[] ores;

    //##�������ͽ� ����##

    // ü��
    [SerializeField]
    private int hp;  // �ִ� ü��. ����Ƽ ������ ���Կ��� ������ ��.
    private int currentHp;

    // ü�� ���Ҽӵ�
    [SerializeField]
    private int hpDecreaseTime; 
    private int currentHpDecreaseTime;

    // ����
    [SerializeField] // �ʵ� ����
    private int mp; // �ִ� MP ����Ƽ ������ ���Կ��� ������ ��.
    private int currentMp; //���� MP

    //���� ���Ҽӵ�
    [SerializeField] // �ʵ� ����
    private int mpDecreaseTime; // MP �����ϴ� �ð�
    private int currentMpDecreaseTime; //���� MP ���� �ð�


    // ����
    [SerializeField]
    private int dp;  // �ִ� ����. ����Ƽ ������ ���Կ��� ������ ��.
    private int currentDp;

    //����
    [SerializeField]
    private int Weight; // �ִ� �����ѵ�
    [SerializeField]
    private int currentWeight; // ���� ���Ի���


    // �ʿ��� �̹���
    [SerializeField]
    private Image[] images_Gauge;
    // �� ���¸� ��ǥ�ϴ� �ε���
    private const int HP = 0, MP = 1, DP = 2, WT = 3;
    [SerializeField]
    private TextMeshProUGUI[] Txts_Gauge;
    private const int HPTXT = 0, MPTXT = 1, DPTXT = 2, WTTXT = 3;
    //���º���
    public static bool canPickUp = true; //���� ���� (����ǰ �Ѱ�)

    void Awake() //������Ʈ �ʱ�ȭ�� start�� ��ü Ȱ��ȭ ���� on off���� �۵������� Awake�� �� �ѹ� �۵�
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<Sounder>();
        uiManager = GetComponent<UIManager>();

    }

    private void Start()
    {
        sound.PlaySound(Sounder.Sfx.Reset); // ���� ��û
        
        //�������ͽ� �ʱ�ȭ
        currentHp = hp;
        currentMp = mp;
        currentDp = 0;

    }


    void Update()
    {
        if (!GameManager.isLive) //���ӿ��� ����
        {
            rigid.simulated = false;
            //sound.PlaySound(Sounder.Sfx.Hit); // ���� ��û
            ChangeAnim(State.Dead);
            onHit.Invoke(); // onhit �̺�Ʈ�� ����� �Լ� ȣ��
        }

        
        else
        {
            if ((Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) && isGround && !UIManager.isMinning && !EventSystem.current.IsPointerOverGameObject() && !Inventory.inventoryActivated && !EventContoroller.randomEvent) // ������(�������� �۵�) & UI���� ���� �߰�
            {
                rigid.AddForce(Vector2.up * startJumpPower, ForceMode2D.Impulse); // ForceMode ������
            }

            isJumpKey = Input.GetButton("Jump") || Input.GetMouseButton(0); //���� ���� �� true�� ������Ʈ 

            if (!Inventory.inventoryActivated && !EventContoroller.randomEvent)
            {
                IsMP(); // ���� ����
            }



            //���� ������Ʈ
            if (currentWeight <= Weight)
            {
                canPickUp = true;
            }
            else
            {
                canPickUp = false;
                Debug.Log("ĳ������ ���԰� ���� á���ϴ�!!");
            }

            GaugeUpdate(); // ���� UI ������ ����
                           //Debug.Log(score);

            if(currentHp <= 0)
            {
                uiOver.SetActive(true); // ���ӿ��� UIȰ��ȭ
                GameManager.I.GameOver();
            }

        }



    }
    private void FixedUpdate()
    {
        if (!GameManager.isLive)
            return;

        if (isJumpKey && !isGround && !UIManager.isMinning && !Inventory.inventoryActivated && !EventContoroller.randomEvent) // ������(������ �۵�)
        {
            jumpPower = Mathf.Lerp(jumpPower, 0, 0.05f); //���氪 , ��ǥ��, ��ǥ������ ����Ǵ� ��
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); // ForceMode ������
        }
    }



    //##���� ����##

    //����(�ٴ� ����)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!isGround) // �������� ����� ���� �ѹ� ����
        {
            ChangeAnim(State.Run);
            sound.PlaySound(Sounder.Sfx.Land); // ���� ��û
            jumpPower = resetJump; //���� �� ������ �� �ʱ�ȭ
        }


        
        
        isGround = true;
    }

    //ü��(��� ����)
    private void OnCollisionExit2D(Collision2D collision)
    {
        ChangeAnim(State.Jump);
        sound.PlaySound(Sounder.Sfx.Jump); // ���� ��û
        isGround = false;
    }






    //��ֹ� / �̺�Ʈ / �� (Ʈ���� �ݶ��̴�) // ������ ���������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //����
        if (collision.transform.tag == "Ore") 
        {
            DecreaseHP(5);
            UIManager.isMinning = true;

            CurrentOre = collision.gameObject.GetComponent<Ore>();
        }

        //������ = �Ҹ�ǰ ((�̰� �ʿ���°Ű���))
        if (collision.transform.tag == "Used") // ������ �ݶ��̴� �±� �Ҹ�ǰ(����)
        {
            //ȹ�� ������ �κ��丮��
            inventory.AcquireItem(potion.transform.GetComponent<ItemPickUp>().item);

            //������ ȹ�� �ȳ� txt
            PickUpTxt.gameObject.SetActive(true);
            PickUpTxt.text = potion.transform.GetComponent<ItemPickUp>().item.itemName + "<color=red> " + "</color>" + "ȹ��";

            Invoke("HidePickUpTxt", 1.5f);
        }


        //ü�� & ���� �칰
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

        //���� �̺�Ʈ
        if (collision.transform.tag == "RandomEvent")
        {
            collision.gameObject.GetComponent<EventContoroller>().CallEvent();
        }

        //����
        if (collision.transform.tag == "Store")
        {
            inventory.OpenShop();
        }
        //����(���ظ� �ִ� ������ü)

        //Ż�� ��ũ��
        if (collision.transform.tag == "EscapeScroll")
        {
            uiEscapeOver.SetActive(true); // Ż����ӿ��� UIȰ��ȭ
            GameManager.isLive = false;
            //���ӸŴ����� Ż�� ���ӿ��� �ҷ�����          
        }


        //����ڵ�
        /*
        rigid.simulated = false;
        sound.PlaySound(Sounder.Sfx.Hit); // ���� ��û 
        ChangeAnim(State.Dead);
        onHit.Invoke(); // onhit �̺�Ʈ�� ����� �Լ� ȣ��
        */
    }
    //�ִϸ��̼� ����
    void ChangeAnim(State state)
    {
        anim.SetInteger("State", (int)state); //������ ����ȯ State => int
        
    }

    //ȹ�� �ȳ� �ؽ�Ʈ �����
    private void HidePickUpTxt()
    {
        PickUpTxt.gameObject.SetActive(false);

    }



    //##�������ͽ�##
    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp; //ü��
        images_Gauge[MP].fillAmount = (float)currentMp / mp; //����
        images_Gauge[DP].fillAmount = (float)currentDp / dp; //����


        //�ؽ�Ʈ ������Ʈ
        Txts_Gauge[HPTXT].text = currentHp + " / " + hp; // ���� ü�� / �ִ� ü�� ǥ��
        Txts_Gauge[MPTXT].text = currentMp + " / " + mp; // ���� ���� / �ִ� ���� ǥ��
        Txts_Gauge[DPTXT].text = currentDp + " / " + dp; // ���� ���� / �ִ� ���� ǥ��
        Txts_Gauge[WTTXT].text = currentWeight + " / " + Weight; // ���� ���� / �ִ� ���� ǥ��
    }

    private void IsMP()  //������Ʈ �� �ȿ��� �� �����Ӹ��� ���� �ȴ�.
                         //�������Ӹ��� ���� �ð� �� ������ �ڿ������� MP ��ġ�� ���������� �� ���̴�.
    {

        if (currentMp > 0 && !Inventory.inventoryActivated && !EventContoroller.randomEvent) // ���� MP ��ġ�� 0 ���� ũ�ٸ�
        {
            if (currentMpDecreaseTime <= mpDecreaseTime) // [���� MP ���ҽð�]�� [MP ���ҽð�]�� ���ų� �۴ٸ�
                currentMpDecreaseTime++; // [���� MP ���ҽð�]�� ��� 1�� ������ ���̴�

            else //[���� MP ���ҽð�]�� [MP ���ҽð�]�� �����ߴٸ�
            {
                currentMp--; // MP ��ġ�� ���ҽ�Ű��
                currentMpDecreaseTime = 0; // [���� MP ���ҽð�] �� 0���� �ǵ�����.
            }
        }


        if (currentMp <= 0 && !Inventory.inventoryActivated && !EventContoroller.randomEvent)  //���� MP ��ġ�� 0 ���϶�� (�� if �� 0���� ū ����̹Ƿ�)
        {
            if (currentHpDecreaseTime <= hpDecreaseTime) // [���� HP ���ҽð�]�� [HP ���ҽð�]�� ���ų� �۴ٸ�
                currentHpDecreaseTime++; // [���� HP ���ҽð�]�� ��� 1�� ������ ���̴�

            else //[���� HP ���ҽð�]�� [HP ���ҽð�]�� �����ߴٸ�
            {
                currentHp--; // HP ��ġ�� ���ҽ�Ű��
                currentHpDecreaseTime = 0; // [���� HP ���ҽð�] �� 0���� �ǵ�����.
            }
        }                                                                                      
    }



    //##�������ͽ� ���� ó��##

    public void IncreaseHP(int _count) // HP ���� ó��
    {
        if (currentHp + _count < hp) //���� ü�¿��� �μ��� ���� _count ��ŭ ���� (�ִ�ü�� �̸��� ���) 
            currentHp += _count; // ���� ü�¿��� �μ��� ���� _count ��ŭ ������Ų��.
        else
            currentHp = hp; //�ִ�ġ ���� ������
    }

    public void DecreaseHP(int _count) //HP ���� ó�� * �� DP �� �����Ѵٸ� DP�� �켱�ؼ� �𵵷�

    {
        if (currentDp > 0) //������ ���� ���� ü���� ��´�.
        {
            DecreaseDP(_count); //��� �� ����
            return;
        }
        currentHp -= _count;

        if (currentHp <= 0) //ü�� ��
        {
            uiOver.SetActive(true); // ���ӿ��� UIȰ��ȭ
            GameManager.I.GameOver();
        }


        //Debug.Log("ĳ������ ü���� 0�� �Ǿ����ϴ�!!");
    }


    public void IncreaseMP(int _count) //MP ���� ó��
    {
        if (currentMp + _count < mp) // ���� MP ���� _count�� ������ �� �ִ� MP ���� ���� ���
            currentMp += _count; // ���� MP ���� �μ��� ���� _count ��ŭ �״�� �����ش�

        else // ���� �μ��� �ִ� MP�� �Ѵ°��
            currentMp = mp; // �ִ�ġ ���� ������

    }

    public void DecreaseMP(int _count) //MP ���� ó��
    {
        if (currentMp - _count < 0) //���� MP ���� _count�� �� ���� 0���� �������
            currentMp = 0; // ���� MP = 0
        else
            currentMp -= _count; // �ִ�ġ ���� ������
    }



    public void IncreaseDP(int _count)  // DP ���� ó��
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    public void DecreaseDP(int _count) // DP ���� ó��
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("ĳ������ ������ 0�� �Ǿ����ϴ�!!");
    }

    public void IncreaseWT(int _Weight)  // WT ���� ó�� (������ ����, ������ ����)
    {
        if (currentWeight + _Weight <= Weight) //���� ���Կ� ���� �μ��� ���Ѱ��� �ִ빫�Ժ��� �������
        {
            currentWeight += _Weight; //�μ� �״�� ���ϱ�
        }

        else
        {
            canPickUp = false;
            Debug.Log("ĳ������ ���԰� ���� á���ϴ�!!"); //���� �ִ빫�Ժ��� ū ���
        }
    }

    public void DecreaseWT(int _Weight) // WT ���� ó��
    {
        currentWeight -= _Weight; // ���� �μ� ���ֱ�

        if (currentWeight <= 0) // ���� ���� ���԰� 0���� �۴ٸ�
            currentWeight = 0; // ���� -����
    }

    //�����
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

   
    //������
    public void Return()
    {
        SceneManager.LoadScene(1);
    }
    // 1. ����(�����Ŀ�)
    // 2. ���� (���� �浹 �̺�Ʈ)
    // 3. ��ֹ� ��ġ(Ʈ���� �浹 �̺�Ʈ)
    // 4. �ִϸ��̼�
    // 5. ����
}
