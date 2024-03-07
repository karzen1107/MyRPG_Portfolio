using UnityEngine;

//Sound의 속성을 관리하는 클래스
[System.Serializable]
public class Sound
{
    public string soundName;    //사운드 이름 명명
    public AudioClip clip;  //사운드 클립
    [Range(0f, 1f)]
    public float volume;    //소리 크기
    [Range(0.1f, 3f)]
    public float pitch; //소리 높낮이(높을수록 가벼운 소리)
    public bool loop;   //무한 반복 여부

    [HideInInspector]
    public AudioSource source;  //사운드 소스
}
