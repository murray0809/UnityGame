using UnityEngine;

/// <summary>
/// 敵の見た目（位置・表示状態・スプライト）を担当するView。
/// </summary>
public class EnemyView : MonoBehaviour
{
    /// <summary>敵の画像を描画するSpriteRenderer。</summary>
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// 参照の初期化を行う。
    /// </summary>
    private void Awake()
    {
        // Inspectorで未設定の場合は自動取得する（子オブジェクトも含めて検索）
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    /// <summary>
    /// 敵の表示位置を設定する。
    /// </summary>
    /// <param name="position">ワールド座標</param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// 敵の表示・非表示を切り替える。
    /// </summary>
    /// <param name="active">表示する場合はtrue</param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    /// <summary>
    /// EnemyDataに設定されたスプライトを反映する。
    /// 1つのPrefabで複数の種類の敵を表現するために使用する。
    /// </summary>
    /// <param name="sprite">表示するスプライト</param>
    public void SetSprite(Sprite sprite)
    {
        // 参照やスプライトが無い場合はPrefabの初期スプライトのままにする
        if (spriteRenderer == null || sprite == null)
        {
            return;
        }

        spriteRenderer.sprite = sprite;
    }
}
