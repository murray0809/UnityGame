using UnityEngine;

public class EnemyBounceAnimation : MonoBehaviour
{
    [SerializeField]
    private float bounceHeight = 0.12f;

    [SerializeField]
    private float bounceSpeed = 8f;

    private Vector3 basePosition;
    private float phaseOffset;

    private void Awake()
    {
        basePosition = transform.localPosition;

        // 敵ごとにタイミングをずらして、全員が同じ動きに揃わないようにする
        phaseOffset = Random.Range(0f, 10f);
    }

    private void Update()
    {
        float offset =
            Mathf.Abs(Mathf.Sin((Time.time + phaseOffset) * bounceSpeed)) *
            bounceHeight;

        transform.localPosition =
            basePosition + new Vector3(0f, offset, 0f);
    }
}