using UnityEngine;

/// <summary>
/// タイトル画面の制御を行うController。
/// スタートボタンが押されるまでWaveの進行とタワー設置を止めておく。
/// </summary>
public class TitleController : MonoBehaviour
{
    /// <summary>タイトル画面のパネル。</summary>
    [SerializeField]
    private GameObject titlePanel;

    /// <summary>ゲーム開始時に有効化するWaveController。</summary>
    [SerializeField]
    private WaveController waveController;

    /// <summary>ゲーム開始時に有効化するTowerPlacer。</summary>
    [SerializeField]
    private TowerPlacer towerPlacer;

    /// <summary>
    /// StartButtonのOnClickから呼び出す。
    /// タイトル画面を閉じ、Waveの進行とタワー設置を開始する。
    /// </summary>
    public void StartGame()
    {
        titlePanel.SetActive(false);

        // 無効化しておいたコンポーネントを有効にすることでゲームを開始する
        waveController.enabled = true;
        towerPlacer.enabled = true;
    }
}
