using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Collider2D placementArea;

    [SerializeField]
    private Collider2D enemyPath;

    private GameController gameController;

    private void Awake()
    {
        gameController =
            FindFirstObjectByType<GameController>();
    }

    private void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceTower();
        }
    }

    private void PlaceTower()
    {
        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Vector3 screenPosition =
            new Vector3(
                mousePosition.x,
                mousePosition.y,
                -Camera.main.transform.position.z
            );

        Vector3 worldPosition =
            Camera.main.ScreenToWorldPoint(screenPosition);

        worldPosition.z = 0f;

        // 設置可能エリア外なら終了
        if (!placementArea.OverlapPoint(worldPosition))
        {
            Debug.Log("設置可能エリア外です。");
            return;
        }

        // 敵の通路上なら終了
        if (enemyPath.OverlapPoint(worldPosition))
        {
            Debug.Log("敵の通路にはTowerを置けません。");
            return;
        }

        // お金が足りなければ終了
        if (!gameController.TryBuyTower(50))
        {
            return;
        }

        Instantiate(
            towerPrefab,
            worldPosition,
            Quaternion.identity
        );

        Debug.Log("Towerを設置しました。");
    }
}