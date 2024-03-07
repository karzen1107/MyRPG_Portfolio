using UnityEngine;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject newUserButton;
    [SerializeField] private GameObject loginButton;
    [SerializeField] private GameObject createButton;
    [SerializeField] private GameObject cancleButton;

    public void NewUserClick()   //계정 생성 버튼 클릭
    {
        newUserButton.SetActive(false);
        loginButton.SetActive(false);
        createButton.SetActive(true);
        cancleButton.SetActive(true);
        Debug.Log("실행");
    }

    public void LoginClick()    //로그인 버튼 클릭
    {

    }

    public void CreateClick()   //생성 할 계정 입력 후 만들기 버튼 클릭
    {

    }

    public void CancleClick()   //계정 생성 취소 버튼 클릭
    {
        newUserButton.SetActive(true);
        loginButton.SetActive(true);
        createButton.SetActive(false);
        cancleButton.SetActive(false);
    }
}
