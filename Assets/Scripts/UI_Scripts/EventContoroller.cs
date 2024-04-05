using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EventContoroller : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    //*���� �̺�Ʈ�� �ٽ� ������ �ʵ��� �Ұ�

    //�̹���
    public GameObject EventPopup; // �̺�Ʈ â on off��
    public GameObject[] EventImage; //�̺�Ʈ ���

    //UI �˾� Ʈ����
    private Vector2 targetPos = new Vector2(25, -5);

    //���ڿ� ����Ʈ = �̺�Ʈ ����Ʈ ((�̺�Ʈ �߰� �� �ش� ����Ʈ�� �߰��Ұ�))
    List<string> EventList = new List<string>() { "EventImage1", "EventImage2", "EventImage3", "EventImage4", "EventImage5" };

    //���°�
    public static bool randomEvent = false; // �̺�Ʈ ���°�


    private void Start()
    {
        CloseEvent();
    }
    //�̺�Ʈ ȣ��
    public void CallEvent()
    {
        randomEvent = true; //���� on

        int RandomInt = Random.Range(0, EventList.Count); // ������ ������Ʈ = ������(0~�̺�Ʈ ����Ʈ�� ����)�̴�

        if (EventList.Count <= 0)
        {
            Debug.Log("�̺�Ʈ ����");
            //���� ȿ���� �ְų� �ݺ� �̺�Ʈ�� �ִ°� ������
            //�ӽ÷� �̺�Ʈ �ݱ⸸ ������
            CloseEvent();
            return;
        }


        EventPopup.SetActive(true);// �̺�Ʈ ��ü â Ȱ��ȭ

        //Ʈ���� �̺�ƮUI ����&����Ƣ��

        for (int i = 0; i < EventImage.Length; i++) 
        {


            if (EventList[RandomInt] == "EventImage" + (i + 1)) // ����  (�̺�Ʈ ����Ʈ���� �������� �̺�Ʈ �̹���+ (i+1) �� ������)=�̺�Ʈ �̹�����1���� �����ϹǷ� 1�� �����ش�.
            {
                EventImage[i].SetActive(true); // �̺�Ʈ �̹��� ���ð� ��

                EventImage[i].transform.DOLocalMove(targetPos, 1).SetEase(Ease.InExpo).SetEase(Ease.OutBounce); //��Ʈ�� ���
            }
                

        }

        print(EventList[RandomInt]); // ���� �̺�Ʈ �����
        EventList.RemoveAt(RandomInt); //�̹� ���� �̺�Ʈ�� ����(����Ʈ���� ��)





        /*
        for (int i = 0; i < EventList.Count; i++) // ���� �̺�Ʈ �� ���ö����� �ݺ�
        {
            print("EventList : " + i + "��° " + EventList[i]);  //���� �̺�Ʈ �� ���
        }
        */
        //Time.timeScale = 0; // ����

        //##�̺�Ʈ##
    }

    //�̺�Ʈ ������

    //���ظ� �޾Ƽ� �̺�Ʈ ����
    public void DamageBtn() //�������� 0�� ��쿡�� ġ���� ������ ��
    {
        if (Random.Range(0, 5) == 0)
            CriticalDamage();
        else
            RandomDamage();

    }



    //##������ ȿ�� ��ϵ�##


    //�������ͽ� ����
    public void RandomDamage()
    {
        int RandomInt = Random.Range(0, 50);
        player.DecreaseHP(RandomInt);
        Debug.Log(RandomInt + "�� �Ϲ� ���� ����");
        //�̺�Ʈ ��� �ؽ�Ʈâ ����ְ� �ű⼭ ��ư ������ �̺�Ʈ �ݱ��ϸ� �ɵ�
        //�ӽ÷� �̺�Ʈ �ݱ� �Լ� ȣ��
        CloseEvent();
    }

    public void CriticalDamage()
    {
        int RandomInt = Random.Range(10, 100);
        player.DecreaseHP(RandomInt);
        Debug.Log(RandomInt + "�� ġ���� ���� ����");
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

    //������ ����

    public void GetItem()
    {
        //inventory.AcquireItem("���� ����������".GetComponent<ItemPickUp>().item);
        //GameManager.I.IncreaseWT("���� ������ ����".GetComponent<ItemPickUp>().item.itemWeight); //���� �߰� (������ �ƴϸ� �̰� ������ ��)
    }

    //�Ϲ� �̺�Ʈ ����
    public void CloseEvent()
    {
        randomEvent = false; //���� off

        EventPopup.SetActive(false); //�̺�Ʈ ��ü â ��Ȱ��ȭ

        for (int i = 0; i < EventImage.Length; i++) //�̺�Ʈ ��� ��ü ��Ȱ��ȭ
        {
            EventImage[i].SetActive(false);
        }
        //Time.timeScale = 1; // ����

    }
}
