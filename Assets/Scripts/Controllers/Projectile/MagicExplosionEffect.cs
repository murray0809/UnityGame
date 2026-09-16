using UnityEngine;
using System.Collections;

public class MagicExplosionEffect : MonoBehaviour
{
    private float duration = 0.35f;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // 敵より手前に表示されるようにする
        spriteRenderer.sortingOrder = 10;
    }

    // radius: 魔法攻撃の効果範囲（ワールド単位の半径）
    public void Initialize(float radius)
    {
        if (spriteRenderer == null)
        {
            Destroy(gameObject);
            return;
        }

        // スプライトの見た目上の直径がちょうどradius*2になるよう調整
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float targetScale = (radius * 2f) / spriteWidth;

        transform.localScale = Vector3.zero;

        StartCoroutine(AnimateAndDestroy(targetScale));
    }

    private IEnumerator AnimateAndDestroy(float targetScale)
    {
        float elapsed = 0f;

        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;

        // 前半40%: 少し大きめ(1.15倍)まで勢いよく広がる（不透明のまま）
        float popDuration = duration * 0.4f;
        float overshootScale = targetScale * 1.15f;

        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popDuration;

            float scale = Mathf.Lerp(0f, overshootScale, t);
            transform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        // 中盤: targetScaleまで少し戻す（不透明のまま維持）
        float settleDuration = duration * 0.2f;
        float settleElapsed = 0f;

        while (settleElapsed < settleDuration)
        {
            settleElapsed += Time.deltaTime;
            float t = settleElapsed / settleDuration;

            float scale = Mathf.Lerp(overshootScale, targetScale, t);
            transform.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        // 後半40%: ここで初めてフェードアウト
        float fadeDuration = duration - popDuration - settleDuration;
        float fadeElapsed = 0f;

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            float t = fadeElapsed / fadeDuration;

            color.a = Mathf.Lerp(1f, 0f, t);
            spriteRenderer.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }
}