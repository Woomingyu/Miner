using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RePosition : MonoBehaviour
{
    public UnityEvent onMove;

    //��ó��
    private void LateUpdate()
    {
        if (transform.position.x > -17) //�� ���� ����ġ�� ����ġ�� ���
            return; // �ƹ��͵� ��ȯ ���ϰ� �Ѿ (�Լ� �� �Ʒ� �ڵ���� ���� x)

        //�ǵ��ư���
        transform.Translate(20, 0, 0, Space.Self);
        onMove.Invoke(); // �̺�Ʈ �Լ� ȣ��(�� ������ ��� ��������Ʈ ü����)
    }

}
