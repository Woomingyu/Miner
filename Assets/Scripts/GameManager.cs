using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable] //Ŭ���� ����ȭ
public class Sound // �� ���� ������ ������ ��밡�� ex == public Sound A;
{
    public string name; // ���� �̸�
    public AudioClip clip; // ���� ����
}




//���� �� ���� & ���ھ� & ���� ���� & �������ͽ� ����
public class GameManager : MonoBehaviour
{
    public static GameManager I; // �̱���ȭ


    //##���� ����##
    public AudioSource[] audioSourcesEffects; // ȿ���� ����� �迭 (���ÿ� ������ �������)
    public AudioSource audioSourceBgm; // ����� �����


    public string[] playSoundName; //Ư�� ���� ������

    //class Sound 
    public Sound[] effectSounds; // ȿ���� ���� �迭
    public Sound[] bgmSounds; // ����� ���� �迭
 
    


    //##���� ���ھ� ����##
    const float ORIGIN_SPEED = 3; // ��� (�Һ���ġ)
    public static float globalSpeed; // ���� �޸𸮿� ���
    public static float score; // ����



    /*
    //##�������ͽ� ����##

    // ü��
    [SerializeField]
    private int hp;  // �ִ� ü��. ����Ƽ ������ ���Կ��� ������ ��.
    private int currentHp;

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
    private Image[] images_Gauge;
    // �� ���¸� ��ǥ�ϴ� �ε���
    private const int HP = 0, MP = 1, DP = 2, WT = 3;

    private TextMeshProUGUI[] Txts_Gauge;
    private const int HPTXT = 0, MPTXT = 1, DPTXT = 2, WTTXT = 3;


    //���º���
    public static bool canPickUp = true; //���� ���� (����ǰ �Ѱ�)
    */

    public static bool isLive; // ���� ���� ����

    void Awake()
    {

        isLive = true; // ���� ����

        if (I == null)
        {
            I = this; //�̱���ȭ
            DontDestroyOnLoad(gameObject); //�ı�����
        }
        else
            Destroy(gameObject); // �� �̵� �� �ߺ����� ����





        if (!PlayerPrefs.HasKey("Score")) // ������ �������� ���ھ� ������ ���°��
            PlayerPrefs.SetFloat("Score", 0); // ���ھ� 0���� �ϴ� ����
    }


    private void Start()
    {
        /*
        //�������ͽ� �ʱ�ȭ
        currentHp = hp;
        currentMp = mp;
        currentDp = 0;
        */
    }



    void Update() //���ھ� & ���� ����
    {
        if (!isLive) //������쿡�� �Լ� �Ʒ� �۵� x
            return;

        if(!Inventory.inventoryActivated && !EventContoroller.randomEvent)
        {
            score += Time.deltaTime * 2; //�ð��� ���� �����ο� 
            globalSpeed = ORIGIN_SPEED + score * 0.01f; // ������ ���� ��ũ�Ѹ� �ӵ� ����

            
            //IsMP(); // ���� ����
        }



        /*
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
        */

    }

    // �÷��̾� ��� & ���ھ� ����
    public void GameOver() 
    {
        isLive = false; // ���ӻ��� off

        float highScore = PlayerPrefs.GetFloat("Score");
        PlayerPrefs.SetFloat("Score", Mathf.Max(highScore, score)); //Mathf.Max �� ���� ���Ͽ� ū �� ��ȯ
    }

    public void EscapeGameOver()
    {
        isLive = false; // ���ӻ��� off

        float highScore = PlayerPrefs.GetFloat("Score");
        //���ھ� ����+�����ϰ� �ִ� ������ ����
    }

    //##���ھ� ����##
    public void IncreaseScore(float _num)
    {
        score += _num;
    }

    public void DecreaseScore(float _num)
    {
        score -= _num;
        if (score <= 0)
            score = 0;
    }





    /*
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


        if (currentMp < 0)  //���� MP ��ġ�� 0 ���϶�� (�� if �� 0���� ū ����̹Ƿ�)
                            //�÷��̾ �׾ ������ ����Ǵ� ����� ó���� ���ָ� ���� �� ����.
            Debug.Log("MP ��ġ�� 0 �� �Ǿ����ϴ�.");

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
            GameOver();
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

    */

    //##����##

    //ȿ���� ���
    public void PlaySE(string _name) // �Ѿ�� ���ڿ��� ��ġ�ϴ� Sound.name�� ã�� Sound.clip�� ����� �ҽ��� �־� ���
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                //������� ����� �ҽ��� �ΰ� & ������� �ƴ� ������ҽ������� �÷���
                for (int y = 0; y < audioSourcesEffects.Length; y++)
                {
                    if(!audioSourcesEffects[y].isPlaying) //���� - ������� �ƴ� ������ҽ��� 
                    {
                        playSoundName[y] = effectSounds[i].name; //��û���� ���带 playSoundName�迭�� �Է�
                        audioSourcesEffects[y].clip = effectSounds[i].clip; // ������� �ƴ� ����� �ҽ��� = ������ ������ ������ �迭 ��°�� Ŭ���� ����
                        audioSourcesEffects[y].Play(); // Ŭ�� ���
                        return; //�Լ�����
                    }
                }
                Debug.Log("��� ���� AudioSource �����"); //�߸� ����� �ҽ� �÷�����
                return;
            }
        }
        Debug.Log(_name + "���尡 GameManager�� ����"); //ȿ������ ���ų� ���� �̸� Ʋ��
    }

    //��� ȿ���� ������
    public void StopAllSE()
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            audioSourcesEffects[i].Stop();
        }
    }

    //Ư�� ȿ���� ������
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            if(playSoundName[i] == _name) //���� ��Ʈ������ ��ġ�ϴ� ����� i��
            {
                audioSourcesEffects[i].Stop(); //��� ����
                return;
            }

        }
        Debug.Log("��� ����" + _name + "���尡 ����"); //����� ȿ���� ���ų� ���� �̸� Ʋ��
    }

}
