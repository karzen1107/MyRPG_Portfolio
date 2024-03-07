using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject player;

    public void Click()
    {
        player.transform.position = new Vector3(3, 1.5f, 0);
    }

    private void Start()
    {
        player.transform.position = new Vector3(-2, 1.5f, 0);
    }
}
