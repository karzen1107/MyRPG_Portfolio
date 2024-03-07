using UnityEngine;

public class TargetRange : MonoBehaviour
{
    private bool isInitalized = true;
    public Transform Target { get; set; }
    public bool IsInitalized { get { return isInitalized; } set { isInitalized = value; } }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && isInitalized == true && other.GetComponent<PlayerState>().IsDeath == false)
        {
            Target = other.transform;
            isInitalized = false;
        }
    }
}
