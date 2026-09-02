using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}