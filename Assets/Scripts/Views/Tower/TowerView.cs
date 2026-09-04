using UnityEngine;

public class TowerView : MonoBehaviour
{
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ShowAttack()
    {
        Debug.Log("Tower Attack!");
    }
}