//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class QuickSkillUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
//{
//    [SerializeField] private Transform canvas;   //부모 캔버스 객체 위치
//    public GameObject loadPrefeb;   //경로를 통해 불러올 프리팹
//    [SerializeField] private GameObject swithSkill;   //복사한 스킬

//    public void OnPointerDown(PointerEventData eventData)
//    {
//        if (Input.GetMouseButtonDown(0)) //왼쪽 버튼으로만 가능하도록
//        {
//            //현재 드래그중인 UI화면의 최상단에 출력되도록 하기
//            this.transform.SetParent(canvas);  //부모 오브젝트를 canvas로 설정
//            this.transform.SetAsLastSibling(); //가장 앞에 보이도록 마지막 자식으로 설정

//            this.GetComponent<Image>().raycastTarget = false;  //광선충돌처리가 되지 않도록 하기
//            #region 광선충돌처리(raycastTarget)가 되지 않도록 해야하는 이유
//            //드래그하고 있는 오브젝트를 Canvas 바로 밑으로 올려 해당 오브젝트보다 하위에 존재하는 UI들이 반응하지 않는다.
//            //UI관련된 그래픽 컴포넌트들은 GraphicRaycast라고 따로 받는 레이캐스트가 존재한다.
//            //UI관련 오브젝트(예를 들면 Image)에 raycastTarget이라고 적힌 옵션이 있을 것이다.
//            //이걸 에디터에서 바로 꺼주면 드래그가 안되니 코드 상에서 잠시 꺼주면 된다.
//            #endregion
//        }
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        if (Input.GetMouseButton(0)) //왼쪽 버튼으로만 가능하도록
//        {
//            this.transform.position = eventData.position;  //UI위치 == 마우스 위치
//            this.GetComponent<RectTransform>().localScale = Vector3.one;
//        }
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        Destroy(this.gameObject);
//    }

//    void Awake()
//    {
//        canvas = FindFirstObjectByType<Canvas>().transform;
//    }
//}
