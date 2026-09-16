using UnityEngine;

/// <summary>
/// タワーの見た目（位置や攻撃演出）を担当するView。
/// </summary>
public class TowerView : MonoBehaviour
{
    /// <summary>
    /// タワーの表示位置を設定する。
    /// </summary>
    /// <param name="position">ワールド座標</param>
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// 攻撃時の演出を表示する（現在はデバッグログのみ）。
    /// </summary>
    public void ShowAttack()
    {
        Debug.Log("Tower Attack!");
    }
}
