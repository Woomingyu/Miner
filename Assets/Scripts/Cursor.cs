using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Cursor : MonoBehaviour
{
    //�ʿ��� ���
    [SerializeField]
    private GameObject cursor; // Ŀ�� ������Ʈ
    [SerializeField]
    private LayerMask layerMask; // base / point ���п�
    [SerializeField]
    private TextMeshProUGUI PointTxt; // ����Ʈ ���� �ؽ�Ʈ ǥ��� /�ظ���
    private Ore ore; //���� �Լ�ȣ���
    private Rigidbody2D rigid;
    [SerializeField]
    private PlayerController player;
    //����
    Vector2 currentPos; // ���� ��ġ ����
    public float speed; // Ŀ�� �ӵ�
    public float MaxPos; // Ŀ�� �̵� �ִ�ġ

    //����ĳ��Ʈ
    [SerializeField]
    private float range; //���̹��� (Ŀ�� ��������)
    [SerializeField]
    private Vector2 rayStart; //���� ������ġ (Ŀ�� ���� ������ġ)

    //����
    [SerializeField]
    private string hit_Sound; // �ǰݻ���

    //���º���

    public static bool coolTime; // ��ųüũ ��Ÿ��(�ߺ����� ����) *���� �ı� �� ���º��� �� �ٲܰ�
    void Start()
    {
        ore = FindObjectOfType<Ore>();
        rigid = GetComponent<Rigidbody2D>();
        currentPos = transform.localPosition; // ��ġ ����
        
    }
    private void FixedUpdate()
    {
        if (UIManager.isMinning && !Inventory.inventoryActivated)
        {
            cursorMove();
            TryCheck();
        }
    }



    //��ųüũ Ŀ�� �̵�&�ӵ�����
    private void cursorMove()
    {
        //##�ð��� ���� �����ϴ� ���� �ƴ� �����ӵ� �ο��� �����Ұ�## Ȥ�� ��޿� ���� ���̵� ����
        Vector2 pos = currentPos;
        speed += Time.deltaTime * 0.001f; //�ð��� ���� Ŀ�� �̵��ӵ� ��� ==> ���ھ ���� �ӵ� ������� ���� 
        pos.x += MaxPos * Mathf.Sin(Time.time * speed); // �̵�, �ִ�ġ ���޽� ����(�߰��� ������ ������ õõ��)
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

    /** Ŀ�� ����Ʈ üũ ����ĳ��Ʈ */
    IEnumerator CheckPointCoroutine()
    {
        coolTime = true;

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position * rayStart, Vector2.right, range, LayerMask.GetMask("Point"));
        //����ĳ��Ʈ�� ������ �����ϴ°� // 2D����ĳ��Ʈ(���� ��ġ/���� ���Ⱚ / �Ÿ���/ ������ ���̾��±�)
        Debug.DrawRay(rigid.position * rayStart, Vector2.right * range, new Color(0, 0, 1));
        //Debug.DrawRay(rigid.position, -dir, new Color(0, 0, 1));

        //����ĳ��Ʈ�� ���̰� ������ִ� �����(������ٵ� ��ġ�޾ƿ���/���� ���Ⱚ, ���̻�)

      
        //���� ���� �Ǻ� �ڷ�ƾ
            if (rayHit.collider != null) // ���� ���� ������ �ݶ��̴��� �ִ�
        {

            PlayerController.CurrentOre.OreMining();

        }                                   
            else
            {
            GameManager.I.PlaySE(hit_Sound);
            player.DecreaseHP(10);
            //�������ͽ� => ü�°��� �Լ� ȣ��
            }

        yield return new WaitForSeconds(2f);
        //RaycastHit2D raycastHit = Physics2D.Raycast(transform.localPosition, transform.forward, 1f, LayerMask.GetMask("Point"));
        //Physics2D.BoxCast(Box2D.bounds.center, Box2D.bounds.size, 0f, Vector2.down * 1.0f, 0f, LayerMask.GetMask("Point")); //����ĳ��Ʈ ���� ����Ʈ üũ

        coolTime = false;
    }



}



