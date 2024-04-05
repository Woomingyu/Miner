using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    //���� ��������Ʈ ���� ���� (����)
    public Sprite[] sprites;
    SpriteRenderer spriter;
    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        Change();
    }

   
    public void Change()
    {
        //�迭 ���̿� ���� ������ ���� ���� 
        int ran = Random.Range(0, sprites.Length);
        //��������Ʈ�� �迭���� �������� �̾� ����
        spriter.sprite = sprites[ran];
    }
}
