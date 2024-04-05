using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//���� ������ Input �ϸ� ������ �ڵ�� �Ǿ����� ==> input�� �Է½� �Ǹŷ� ����
//���� : Call ȣ�� -> ���UI Ȱ��ȭ & �ȳ� �ؽ�Ʈ ������ �ִ밪���� ���� ->
//CheckNumber �����Է°��� ���� �Ǻ�(���ڸ�, ���ڿ� ����, �Է�x) ->
//OK ���� �Է°� ���¿� ���� num �� ��ȯ -> DropItemCoroutine���� num���� ���� ������ ���� & ���԰� null����
public class InputNumber : MonoBehaviour
{
    [SerializeField]
    private Text text_Preview; // ������ �ؽ�Ʈ �̸�����(���� ���� ������)
    [SerializeField]
    private Text text_Input; // ���� �Է°�
    [SerializeField]
    private InputField if_text; // ���� ��ǲ�� (���� ��ǲ�� �ʱ�ȭ��)

    [SerializeField]
    private GameObject go_Base; // ������ ��� UI base

    [SerializeField]
    private PlayerController playerPos; //������ ��� ��ġ��


    //������ �Է�â ȣ��
    public void Call()
    {
        go_Base.SetActive(true); //�Է�â Ȱ��ȭ
        if_text.text = ""; // �ؽ�Ʈ �Է°� �ʱ�ȭ (������ �Է��ߴ� �ؽ�Ʈ�� �ȳ�����)

        //�⺻ ǥ�� �ؽ�Ʈ��  �巡���� ������ ������ ����(���ڿ��� ��ȯ)
        //ȣ��� ���ÿ� �ؽ�Ʈ�� �������� �ִ밳���� ������ == ��� ����
        text_Preview.text = DragSlot.instance.dragSlot.itemCount.ToString();

    }
    //������ ���
    public void Cancel()
    {
        go_Base.SetActive(false); //�Է�â ��Ȱ��ȭ
        DragSlot.instance.SetColor(0); // �巡�� ������ ����ȭ
        DragSlot.instance.dragSlot = null; //�巡�� ������ �ʱ�ȭ
    }

    public void OK()
    {
        DragSlot.instance.SetColor(0); //�̹��� ����ȭ/ok�� ������ �巡���� �̹����� ������°� �ڿ�������

        int num;
        if (text_Input.text != "")
        {
            if (CheckNumber(text_Input.text)) //text_Input.text �ȿ� ���ڰ� �ϳ��� �ִ��� Ȯ��
            {
                num = int.Parse(text_Input.text); // ���ڰ� �´� ���  �Է°��� int������ �ٲ���

                if (num > DragSlot.instance.dragSlot.itemCount) // �Է°��� �ִ� ������ �������� ������
                    num = DragSlot.instance.dragSlot.itemCount; // �Է°��� �ִ밪���� ����
            }
            else // ����, ���� �� �ΰ��
            {
                num = 1; // ���� �Է½� �ϳ��� ������ ����
                         // ���Ǽ��� ���� ������ ���ϰ� �ϰų� ���� �����ų� ��� ����
            }
        }
        else // �ƹ��͵� ���� ���� ���
            num = int.Parse(text_Preview.text); // �ִ밳�� ��ŭ �Է�

        StartCoroutine(DropItemCoroutine(num)); // �Է� �������� ������ �ڷ�ƾ ����


    }

    //���� ������ �ڷ�ƾ(�Է°���ŭ ������ �ݺ�)
    IEnumerator DropItemCoroutine(int _num)
    {
        for (int i = 0; i < _num; i++)
        {
            playerPos.DecreaseWT(DragSlot.instance.dragSlot.item.itemWeight); //���� ����                                                                                 
            PlayerController.canPickUp = true;//������ ȹ�� ����
            Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, playerPos.transform.position + playerPos.transform.forward, Quaternion.identity);
            DragSlot.instance.dragSlot.SetSlotCount(-1); //�κ��丮 �������� ���� ����
            yield return new WaitForSeconds(0.05f); //�� ������ ���ð�
        }

      
        DragSlot.instance.dragSlot = null;
        go_Base.SetActive(false);
    }



    //�Է°��� ���ڰ� �ϳ��� ���ԵǾ����� �Ǻ�
    private bool CheckNumber(string _argString)
    {
        double tempDouble;
        return Double.TryParse(_argString, out tempDouble);
    }
}
