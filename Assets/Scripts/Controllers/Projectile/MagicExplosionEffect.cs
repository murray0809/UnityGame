using UnityEngine;

public class MagicExplosionEffect : MonoBehaviour
{
    private float radius;
    private float duration = 0.2f;
    private int segments = 32;

    public void Initialize(float radius)
    {
        this.radius = radius;

        LineRenderer lineRenderer =
            gameObject.AddComponent<LineRenderer>();

        // 描画設定
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments;

        lineRenderer.startWidth = 0.08f;
        lineRenderer.endWidth = 0.08f;

        // Materialを設定
        lineRenderer.material =
            new Material(Shader.Find("Sprites/Default"));

        // 画面上に表示されるようにする
        lineRenderer.sortingOrder = 10;

        for (int i = 0; i < segments; i++)
        {
            float angle =
                2f * Mathf.PI * i / segments;

            float x =
                Mathf.Cos(angle) * radius;

            float y =
                Mathf.Sin(angle) * radius;

            lineRenderer.SetPosition(
                i,
                new Vector3(x, y, 0f)
            );
        }

        Destroy(gameObject, duration);
    }
}