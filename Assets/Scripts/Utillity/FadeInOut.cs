using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private Image img;    //페이더 이미지
    [SerializeField] private AnimationCurve curve;    //자연스러운 페이드 효과

    [SerializeField] private GameObject backGroundUI;   //검은 배경 UI
    [SerializeField] private GameObject youDeathUI; //당신은 죽었습니다!

    // 속성 //
    public Image Img { get { return img; } }

    public void InFade()    //페이드 인 외부공개 메서드
    {
        StartCoroutine(FadeIn());  // [1]
    }

    public void OutFade()   //페이드 아웃 외부공개 메서드
    {
        StartCoroutine(FadeOut());
    }

    public void OutFade(string sceneName)   //페이드 아웃 외부공개 메서드. 오버로딩
    {
        StartCoroutine(FadeOut(sceneName));
    }

    public void OutFade(int sceneNum)    //페이드 아웃 외부공개 메서드. 오버로딩
    {
        StartCoroutine(FadeOut(sceneNum));  // [1]
    }

    public void OutFade(bool isDeath)
    {
        StartCoroutine(FadeOut(isDeath));
    }

    private IEnumerator FadeIn()    //페이드 인
    {
        backGroundUI.SetActive(true);
        float a = 1;  // [1-2] //얘는 다시 실행 안함. 1번만 실행한다.

        while (a >= 0f) // [2]
        {
            a -= Time.deltaTime; // [3]
            float cValue = curve.Evaluate(a);
            img.color = new Color(0, 0, 0, cValue); // [4]
            yield return 0; // [5] : 일단 멈춤
        }

        backGroundUI.SetActive(false);
    }

    private IEnumerator FadeOut()    //페이드 아웃
    {
        backGroundUI.SetActive(true);
        float a = 0; // [1-2] //얘는 다시 실행 안함. 1번만 실행한다.

        while (a <= 1f) // [2]
        {
            a += Time.deltaTime; // [3]
            float cValue = curve.Evaluate(a);
            img.color = new Color(0, 0, 0, cValue); // [4]
            yield return 0; // [5] : 일단 멈춤
        }
    }

    private IEnumerator FadeOut(int scneNum)    //페이드 아웃. 오버로딩
    {
        backGroundUI.SetActive(true);
        float a = 0; // [1-2] //얘는 다시 실행 안함. 1번만 실행한다.

        while (a <= 1f) // [2]
        {
            a += Time.deltaTime; // [3]
            float cValue = curve.Evaluate(a);
            img.color = new Color(0, 0, 0, cValue); // [4]
            yield return 0; // [5] : 일단 멈춤
        }
        SceneManager.LoadSceneAsync(scneNum); //[8]
    }

    private IEnumerator FadeOut(string sceneName)   //페이드 아웃. 오버로딩
    {
        backGroundUI.SetActive(true);
        float a = 0; // [1-2] //얘는 다시 실행 안함. 1번만 실행한다.

        while (a <= 1f) // [2]
        {
            a += Time.deltaTime; // [3]
            float cValue = curve.Evaluate(a);
            img.color = new Color(0, 0, 0, cValue); // [4]
            yield return 0; // [5] : 일단 멈춤
        }
        SceneManager.LoadSceneAsync(sceneName); //[8]
    }

    private IEnumerator FadeOut(bool isDeath)   //페이드 아웃. 오버로딩
    {
        if (isDeath)
        {
            backGroundUI.SetActive(true);
            float a = 0; // [1-2] //얘는 다시 실행 안함. 1번만 실행한다.

            while (a <= 1f) // [2]
            {
                a += Time.deltaTime; // [3]
                float cValue = curve.Evaluate(a);
                img.color = new Color(0, 0, 0, cValue); // [4]
                yield return 0; // [5] : 일단 멈춤
            }

            youDeathUI.SetActive(true);
        }
    }

    public void ReLoadClick()
    {
        SystemManager.Instance.SetCheckPointPlayer();
        youDeathUI.SetActive(false);
        InFade();
    }

    public void ExitClick()
    {
        Application.Quit();
    }

    private void Start()
    {
        //img.color 초기화 //검은색 만들어 놓고 시작
        img.color = new Color(0, 0, 0, 0); // [4]
    }
}
