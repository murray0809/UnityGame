using UnityEngine;

/// <summary>
/// 敵が歩いているように見せるため、上下に跳ねるアニメーションを付けるコンポーネント。
/// 親オブジェクトの移動とは独立して、ローカル座標で上下させる。
/// </summary>
public class EnemyBounceAnimation : MonoBehaviour
{
    /// <summary>跳ねる高さ（ローカル座標の単位）。</summary>
    [SerializeField]
    private float bounceHeight = 0.12f;

    /// <summary>跳ねる速さ。値が大きいほど速く跳ねる。</summary>
    [SerializeField]
    private float bounceSpeed = 8f;

    /// <summary>跳ねる前の基準となるローカル座標。</summary>
    private Vector3 basePosition;

    /// <summary>敵ごとに跳ねるタイミングをずらすための時間オフセット。</summary>
    private float phaseOffset;

    /// <summary>
    /// 基準位置とタイミングのずれを初期化する。
    /// </summary>
    private void Awake()
    {
        basePosition = transform.localPosition;

        // 敵ごとにタイミングをずらして、全員が同じ動きに揃わないようにする
        phaseOffset = Random.Range(0f, 10f);
    }

    /// <summary>
    /// 毎フレーム、サイン波をもとに上下位置を更新する。
    /// </summary>
    private void Update()
    {
        // Sinの絶対値を使うことで、地面から跳ねて着地する動きにする
        float offset =
            Mathf.Abs(Mathf.Sin((Time.time + phaseOffset) * bounceSpeed)) *
            bounceHeight;

        transform.localPosition =
            basePosition + new Vector3(0f, offset, 0f);
    }
}
