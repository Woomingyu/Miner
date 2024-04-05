using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject go_SkillCheckBase; // ��ųüũ UI ON/OFF



    //�ð��� ������ �ʱ�����(ä�� UI�� ����� �ִµ��� ��� ���̵��� �ö󰡵���)
    //���� ������ �̿��� �ð��� ������ �������� ������
    public static bool isMinning = false;




    private void Update()
    {
        if (isMinning)
            OnMinningUI();
        else
            OffMinningUI();
    }



    //ä�� ��ųüũ
    public void OnMinningUI()
    {
        go_SkillCheckBase.SetActive(true); // ��ųüũ UI Ȱ��ȭ
    }

    public void OffMinningUI()
    {
        isMinning = false;

        go_SkillCheckBase.SetActive(false); // ��ųüũ UI ��Ȱ��ȭ
    }

}
