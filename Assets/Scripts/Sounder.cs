using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounder : MonoBehaviour
{
    //���� �迭
    public enum Sfx { Jump, Land, Hit, Reset }



    //�ʿ��� ������Ʈ
    public AudioClip[] clips; //����� Ŭ�� �迭
    AudioSource audios; //����� �ҽ�




    void Awake()
    {
        audios = GetComponent<AudioSource>(); //���� �ʱ�ȭ
    }



    //���� ���
    public void PlaySound(Sfx sfx) // �޾ƿ� ���� �迭
    {
        audios.clip = clips[(int)sfx]; //Sfx ������ => int������
        audios.Play(); // �޾ƿ� Ŭ�� ���
    }
}
