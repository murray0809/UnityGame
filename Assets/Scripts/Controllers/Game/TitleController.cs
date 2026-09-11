using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    private GameObject titlePanel;

    [SerializeField]
    private WaveController waveController;

    [SerializeField]
    private TowerPlacer towerPlacer;

    // StartButton‚ÌOnClick‚©‚çŒÄ‚Ño‚·
    public void StartGame()
    {
        titlePanel.SetActive(false);

        waveController.enabled = true;
        towerPlacer.enabled = true;
    }
}