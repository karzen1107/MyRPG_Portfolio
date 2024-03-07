using UnityEngine;
/// <summary>
/// [치트키 모음]
/// 로컬데이터 삭제 : LeftShift + P, 
/// 마우스 락/언락 : Esc
/// </summary>
public class ChitKeyManager : MonoBehaviour
{
    [SerializeField] private bool canChit;  //치트키 사용 여부 확인

    public void ClearLocalData()    //치트 - 모든 로컬 데이터 삭제 메서드
    {
        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.LeftShift))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!canChit)
            return;
        ClearLocalData();
    }
}
