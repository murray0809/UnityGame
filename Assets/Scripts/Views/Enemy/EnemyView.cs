using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;   // ← 追加

    private void Awake()
    {
        // Inspectorで未設定の場合は自動取得する（子オブジェクトも含めて検索）
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    // EnemyDataに設定されたスプライトを反映する   ← 追加
    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer == null || sprite == null)
        {
            return;
        }

        spriteRenderer.sprite = sprite;
    }
}