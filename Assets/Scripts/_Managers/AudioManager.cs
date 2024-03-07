using System.Collections;
using UnityEngine;

public class AudioManager : Singletone_Destroy<AudioManager>
{
    public Sound[] sounds;
    [HideInInspector]
    public string bgmSound = string.Empty;    //브금이름

    public void SoundPlay(string name)  //소리 재생
    {
        Sound sound = null;
        foreach (var s in sounds)
        {
            if (s.soundName == name)
            {
                sound = s;
                break;
            }
        }

        if (sound == null)
            return;

        sound.source.Play();
    }

    public void SoundStop(string name)  //소리 멈춤
    {
        Sound sound = null;
        foreach (var s in sounds)
        {
            if (s.soundName == name)
            {
                sound = s;
                break;
            }
        }

        if (sound == null)
            return;

        sound.source.Stop();
    }

    public void SoundFadeOut(string name)   //브금 페이드 아웃 외부 공개
    {
        StartCoroutine(FadeOut(name));
    }

    private IEnumerator FadeOut(string name)    //브금 페이드 아웃
    {
        Sound sound = null;
        foreach (var s in sounds)
        {
            if (s.soundName == name)
            {
                sound = s;
                break;
            }
        }

        if (sound == null)
            yield return null;

        float a = sound.volume; // [1-2] //얘는 다시 실행 안함. 1번만 실행한다.

        while (a >= 0f) // [2]
        {
            a -= Time.deltaTime * 0.2f; // [3]
            sound.source.volume = a; // [4]
            yield return 0; // [5] : 일단 멈춤
        }
    }

    public void BgmPlay(string name)    //배경음악 재생
    {
        if (bgmSound == name)
            return;

        SoundStop(bgmSound);    //기존 재생중인 브금 멈춤

        Sound sound = null;
        foreach (var s in sounds)
        {
            if (s.soundName == name)
            {
                bgmSound = name;
                sound = s;
                break;
            }
        }

        if (sound == null)
            return;

        sound.source.Play();
    }

    protected override void Awake()
    {
        base.Awake();

        //사운드 목록 세팅
        foreach (var s in sounds)
        {
            s.source = this.gameObject.AddComponent<AudioSource>();    //각 클립의 오디오 소스 생성
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }
}
