using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Memo_IPointHandler_Interface : MonoBehaviour
{
    /* [ 메모장 ]
     *      인터페이스               /                           메서드                                    /                                  설명
     * IPointerEnterHandler / void OnPointerEnter(PointerEventData aaa) / 마우스 포인터가 현재 오브젝트 영역 내부로 들어갈때 1회 호출
     * IPointerExitHandler   /  void OnPointerExit(PointerEventData aaa)   /  마우스 포인터가 현재 오브젝트 영역 외부로 나갈때 1회 호출
     * IPointerDownHandler/ void OnPointerDown(PointerEventData aaa)/ 해당 오브젝트 내부에서 클릭하는 순간 1회 호출
     * IPointerUpHandler    / void OnPointerUp(PointerEventData aaa)      / 해당 오브젝트 내부에서 클릭을 떼는 순간 1회 호출
     * IPointerClickHandler / void OnPointerClick(PointerEventData aaa)   / 해당 오브젝트 내부에서 클릭다운&업 하는 순간 1회 호출(다운&업 할 때 마우스가 해당 오브젝트 내에 있어야 함)
     * IBeginDragHandler   / void OnBeginDrag(PointerEventData aaa)     / 해당 오브젝트를 드래그 시작할 때 1회 호출
     * IDragHandler            / void OnDrag(PointerEventData aaa)             / 해당 오브젝트를 드래그 중일 때 프레임마다 1회 호출
     * IEndDragHandler      / void OnEndDrag(PointerEventData aaa)        / 해당 오브젝트를 드래그 끝날 때 1회 호출
     * IDropHandler            / void OnDrop(PointerEventData aaa)              / 해당 오브젝트 영역 내부에서 드롭했을 때 1회 호출
     */

    // 예시 //
    public void OnBeginDrag(PointerEventData eventData) //드래그 시작할 때 호출
    {
        this.transform.BroadcastMessage("BeginDrag", transform, SendMessageOptions.DontRequireReceiver);
        //SendMessage : 이 오브젝트에 붙어 있는 모든 컴포넌트/스크립트에서 Function이라는 함수명을 찾아 실행.
        //BroadcastMessage : 게임 오브젝트의 모든 컴포넌트마다 지명한 함수를 부르고, 더해서 씬 계층상의 모든 자식 오브젝트와 그 자식 오브젝트의 모든 컴포넌트도 찾아서 실행.
        //"BeginDrag" : 호출할 함수 이름
        // transform : 실핼할 함수가 매개변수가 필요하다면 전달해줄 값
        //SendMessageOptions : 메세지를 보낼 방법에 대한 옵션.
        //              RequireReceiver : SendMessage에 대한 수신자가 필요한 경우 사용.
        //                                          3번째 파라미터를 지정하지 않아도 기본 값은 RequireReceiver이며 이는 한번 메시지 호출이 발생하면 누군가는 그것을 받아서 처리를 해주어야 한다는것을 의미.
        //                                          만약에 대응되는 메소드가 존재하지 않는다면 오류가 발생함.
        //              DontRequireReceiver : SendMessage에 대한 수신자가 필요없을 경우 사용
    }

    // 예시 //
    void BeginDrag(Transform card)  //수행할 메서드
    {
        Debug.Log("BeginDrag" + card.name);
    }
}
