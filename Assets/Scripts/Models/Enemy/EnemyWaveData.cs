/// <summary>
/// 1つのWaveで「どの種類の敵を何体出すか」を表すデータ。
/// WaveControllerからEnemySpawnerへ出現内容を渡す際に使用する。
/// </summary>
[System.Serializable]
public class EnemyWaveData
{
    /// <summary>出現させる敵の種類。</summary>
    public EnemyData enemyData;

    /// <summary>出現させる体数。</summary>
    public int count;
}
