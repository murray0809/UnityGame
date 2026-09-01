using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameModel model;

    [SerializeField]
    private GameView view;

    private void Awake()
    {
        model = new GameModel(20, 200);
    }
}