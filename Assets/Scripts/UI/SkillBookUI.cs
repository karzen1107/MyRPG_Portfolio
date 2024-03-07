using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 스킬북에 관한 클래스
/// </summary>

public class SkillBookUI : MonoBehaviour
{
    // 컴포넌트 //
    UIManager uiManager;
    PlayerSkillManager playerSkillManager;

    private void OnPointerDown()    //마우스 다운
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (RaycastResult result in uiManager.Results)
            {
                if (result.gameObject.GetComponent<UIDragAndDrop>() != null)    //스킬 오브젝트가 있다면
                {
                    for (int i = 0; i < playerSkillManager.BaseSkills.Count; i++)
                    {
                        if (result.gameObject.GetComponent<BaseSkill>().Skill == playerSkillManager.BaseSkills[i].Skill)   //타겟과 등록한 스킬이 같다면
                        {
                            uiManager.IconSelect(result.gameObject, i); //타겟 오브젝트와, 위치 정보 전송
                            break;
                        }
                    }
                }
            }
        }
    }

    public void CloseClick()    //닫기 버튼 클릭 메서드
    {
        this.gameObject.SetActive(false);
    }

    public void InitializeClick()   // 스킬 포인트 초기화 버튼 클릭 메서드
    {
        Debug.Log("스킬 초기화");
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        playerSkillManager = PlayerSkillManager.Instance;
    }
    private void Update()
    {
        if (uiManager.ValidNum == uiNumbers.SkillBookUI)
            OnPointerDown();
    }
}
