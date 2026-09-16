using UnityEngine;
using System.Collections;

/// <summary>
/// 魔法弾の着弾時に表示する爆発エフェクト。
/// 攻撃範囲と同じ大きさまで広がり、フェードアウトしてから自身を破棄する。
/// </summary>
public class MagicExplosionEffect : MonoBehaviour
{
    /// <summary>エフェクト全体の再生時間（秒）。</summary>
    private float duration = 0.35f;

    /// <summary>爆発の画像を描画するSpriteRenderer。</summary>
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// SpriteRendererの取得と描画順の設定を行う。
    /// </summary>
    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // 敵より手前に表示されるようにする
        spriteRenderer.sortingOrder = 10;
    }

    /// <summary>
    /// 攻撃範囲に合わせて大きさを計算し、アニメーションを開始する。
    /// </summary>
    /// <param name="radius">魔法攻撃の効果範囲（ワールド単位の半径）</param>
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

        // 大きさ0から広がる演出にするため、最初は見えない状態にする
        transform.localScale = Vector3.zero;

        StartCoroutine(AnimateAndDestroy(targetScale));
    }

    /// <summary>
    /// 「広がる → 少し縮んで落ち着く → フェードアウト」の3段階で再生し、最後に破棄するコルーチン。
    /// </summary>
    /// <param name="targetScale">最終的な大きさ（攻撃範囲と一致するスケール）</param>
    private IEnumerator AnimateAndDestroy(float targetScale)
    {
        float elapsed = 0f;

        // アルファ値を1にリセットして、完全に不透明な状態から始める
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
