/// <summary>
/// Waveの進行状況（現在のWave番号と総Wave数）を管理するModel。
/// </summary>
public class WaveModel
{
    /// <summary>現在のWave番号（開始前は0、1から始まる）。</summary>
    public int CurrentWave { get; private set; }

    /// <summary>ゲームクリアまでに必要な総Wave数。</summary>
    public int TotalWaves { get; private set; }

    /// <summary>
    /// 総Wave数を指定してModelを生成する。
    /// </summary>
    /// <param name="totalWaves">総Wave数</param>
    public WaveModel(int totalWaves)
    {
        CurrentWave = 0;
        TotalWaves = totalWaves;
    }

    /// <summary>
    /// Wave番号を1つ進める。
    /// </summary>
    public void StartNextWave()
    {
        CurrentWave++;
    }

    /// <summary>
    /// すべてのWaveを消化したかどうかを判定する。
    /// </summary>
    /// <returns>最終Waveまで到達していればtrue</returns>
    public bool IsFinished()
    {
        return CurrentWave >= TotalWaves;
    }
}
