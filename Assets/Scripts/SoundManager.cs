using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sounds
{
    public string name;             // 곡명
    public AudioClip clip;          // 곡
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    #region Singleton
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBGM;

    public string[] playSoundNames;

    public Sounds[] effectSounds;
    public Sounds[] bgmSounds;

    void Start()
    {
        playSoundNames = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundNames[j] = effectSounds[i].name;

                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();

                        return;
                    }
                }

                Debug.Log("모든 가용 AudioSource가 사용 중입니다.");
                return;
            }
        }
        Debug.Log("사운가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundNames[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }

        Debug.Log("재생 중인 " + _name + " 사운드가 없습니다.");
    }
}
