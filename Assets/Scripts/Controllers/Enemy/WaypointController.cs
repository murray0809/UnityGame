using UnityEngine;

/// <summary>
/// 敵が進む経路（ウェイポイント）を管理するController。
/// 敵はインデックス順にウェイポイントを辿り、最後の地点に着くとゴールとなる。
/// </summary>
public class WaypointController : MonoBehaviour
{
    /// <summary>経路上の地点。Inspectorで通過順に並べて設定する。</summary>
    [SerializeField]
    private Transform[] waypoints;

    /// <summary>
    /// 指定したインデックスのウェイポイントを取得する。
    /// </summary>
    /// <param name="index">ウェイポイントの番号（0始まり）</param>
    /// <returns>ウェイポイント。範囲外の場合はnull</returns>
    public Transform GetWaypoint(int index)
    {
        // 範囲外アクセスで例外が出ないよう、nullを返して呼び出し側で判定させる
        if (index < 0 || index >= waypoints.Length)
        {
            return null;
        }

        return waypoints[index];
    }

    /// <summary>ウェイポイントの総数。</summary>
    public int WaypointCount
    {
        get { return waypoints.Length; }
    }
}
