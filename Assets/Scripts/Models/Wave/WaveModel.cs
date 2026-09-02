public class WaveModel
{
    public int CurrentWave { get; private set; }
    public int TotalWaves { get; private set; }

    public WaveModel(int totalWaves)
    {
        CurrentWave = 0;
        TotalWaves = totalWaves;
    }

    public void StartNextWave()
    {
        CurrentWave++;
    }

    public bool IsFinished()
    {
        return CurrentWave >= TotalWaves;
    }
}