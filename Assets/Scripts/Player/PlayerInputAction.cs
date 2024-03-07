using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAction : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>();

    private float maxDis = Mathf.Infinity;  //가장 가까운 오브젝트를 찾기 위한 변수
    private Transform selectedObj; //액션수행할 오브젝트

    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.started && selectedObj != null)
        {
            selectedObj.GetComponent<IInteractive>().DoAction();    //가장 가까운 객체의 액션을 수행한다.
            colliderList.Remove(selectedObj.GetComponent<Collider>());
            selectedObj = null;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IInteractive>() != null)
        {
            if (colliderList.Contains(other))
                return;
            colliderList.Add(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractive>() != null)
        {
            other.GetComponent<IInteractive>().SetOutLine(false);
            colliderList.Remove(other);
        }
    }

    private void LateUpdate()
    {
        if (colliderList.Count <= 0 || CameraControls.Instance.IsNpcInteracting == true)
            return;

        maxDis = Mathf.Infinity;

        for (int i = 0; i < colliderList.Count; i++)
        {
            colliderList[i].GetComponent<IInteractive>().SetOutLine(false);   //리스트의 모든 아웃라인 초기화
            float distance = Vector3.Distance(colliderList[i].transform.position, transform.position); //아이템과 플레이어 사이의 거리

            if (distance < maxDis)  //플레이어와 가장 가까운 객체 찾기
            {
                maxDis = distance;
                selectedObj = colliderList[i].GetComponent<Transform>();
            }
        }

        selectedObj.GetComponent<IInteractive>().SetOutLine(true);   //아이템 아웃라인 생성
    }
}
