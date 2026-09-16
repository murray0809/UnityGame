using TMPro;
using UnityEngine;

/// <summary>
/// ゲーム中のHUD（ライフ・所持金・Wave）と、終了時の結果表示を担当するUI。
/// 値そのものは持たず、Controllerから渡された値を表示するだけにしている。
/// </summary>
public class GameUI : MonoBehaviour
{
    /// <summary>ライフを表示するテキスト。</summary>
    [SerializeField]
    private TextMeshProUGUI lifeText;

    /// <summary>所持金を表示するテキスト。</summary>
    [SerializeField]
    private TextMeshProUGUI moneyText;

    /// <summary>現在のWave / 総Wave数を表示するテキスト。</summary>
    [SerializeField]
    private TextMeshProUGUI waveText;

    /// <summary>"GAME OVER" / "GAME CLEAR" を表示するテキスト。</summary>
    [SerializeField]
    private TextMeshProUGUI resultText;

    /// <summary>ゲーム終了時に表示するリトライボタン。</summary>
    [SerializeField]
    private GameObject retryButton;

    /// <summary>
    /// ゲーム開始時は結果表示とリトライボタンを非表示にしておく。
    /// </summary>
    private void Awake()
    {
        if (resultText != null)
        {
            resultText.gameObject.SetActive(false);
        }

        if (retryButton != null)
        {
            retryButton.SetActive(false);
        }
    }

    /// <summary>
    /// ライフの表示を更新する。
    /// </summary>
    /// <param name="life">現在のライフ</param>
    public void UpdateLife(int life)
    {
        lifeText.text = "Life: " + life;
    }

    /// <summary>
    /// 所持金の表示を更新する。
    /// </summary>
    /// <param name="money">現在の所持金</param>
    public void UpdateMoney(int money)
    {
        moneyText.text = "Money: " + money;
    }

    /// <summary>
    /// Waveの表示を「Wave: 現在 / 総数」の形式で更新する。
    /// </summary>
    /// <param name="currentWave">現在のWave番号</param>
    /// <param name="totalWaves">総Wave数</param>
    public void UpdateWave(int currentWave, int totalWaves)
    {
        waveText.text =
            "Wave: " + currentWave + " / " + totalWaves;
    }

    /// <summary>
    /// "GAME OVER" / "GAME CLEAR" を画面に表示し、リトライボタンを出す。
    /// </summary>
    /// <param name="message">表示する結果メッセージ</param>
    public void ShowResult(string message)
    {
        if (resultText == null)
        {
            return;
        }

        resultText.text = message;
        resultText.gameObject.SetActive(true);

        if (retryButton != null)
        {
            retryButton.SetActive(true);
        }
    }
}
