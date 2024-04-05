using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounder : MonoBehaviour
{
    //사운드 배열
    public enum Sfx { Jump, Land, Hit, Reset }



    //필요한 컴포넌트
    public AudioClip[] clips; //오디오 클립 배열
    AudioSource audios; //오디오 소스




    void Awake()
    {
        audios = GetComponent<AudioSource>(); //변수 초기화
    }



    //사운드 재생
    public void PlaySound(Sfx sfx) // 받아올 사운드 배열
    {
        audios.clip = clips[(int)sfx]; //Sfx 열거형 => int형으로
        audios.Play(); // 받아온 클립 재생
    }
}
