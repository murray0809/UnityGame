using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private TowerData towerData;   // ← 追加

    [SerializeField]
    private GameObject magicTowerPrefab;

    [SerializeField]
    private TowerData magicTowerData;   // ← 追加

    [SerializeField]
    private Collider2D placementArea;

    [SerializeField]
    private Collider2D enemyPath;

    [SerializeField]
    private TowerUpgradeUI towerUpgradeUI;

    private GameController gameController;

    // 現在選択しているTower
    private GameObject selectedTowerPrefab;

    private TowerData selectedTowerData;   // ← 追加

    private void Awake()
    {
        gameController =
            FindFirstObjectByType<GameController>();

        // 最初は通常Towerを選択
        selectedTowerPrefab = towerPrefab;

        selectedTowerData = towerData;   // ← 追加
    }

    private void Update()
    {
        if (gameController != null && gameController.IsGameEnded)   // ← 追加
        {
            return;
        }

        if (Mouse.current == null)
        {
            Debug.Log("Mouse.current が null");
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("クリック検出");

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

            Debug.Log("クリック位置: " + worldPosition);

            Collider2D[] hits =
                Physics2D.OverlapPointAll(worldPosition);

            foreach (Collider2D hit in hits)
            {
                Debug.Log("Collider発見: " + hit.gameObject.name);

                TowerController tower =
                    hit.GetComponent<TowerController>();

                if (tower != null)
                {
                    Debug.Log("Tower発見");

                    towerUpgradeUI.Show(tower);

                    return;
                }
            }

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

        // After
        if (selectedTowerData == null)
        {
            Debug.LogError("選択中のTowerDataが設定されていません。");
            return;
        }

        int cost = selectedTowerData.cost;

        if (!gameController.TryBuyTower(cost))
        {
            return;
        }

        Instantiate(
            selectedTowerPrefab,
            worldPosition,
            Quaternion.identity
        );

        Debug.Log(
            selectedTowerPrefab.name +
            "を設置しました。"
        );
    }

    // 通常Towerを選択
    public void SelectNormalTower()
    {
        selectedTowerPrefab = towerPrefab;

        selectedTowerData = towerData;   // ← 追加

        Debug.Log("通常Towerを選択しました。");
    }

    // MagicTowerを選択
    public void SelectMagicTower()
    {
        selectedTowerPrefab = magicTowerPrefab;

        selectedTowerData = magicTowerData;   // ← 追加

        Debug.Log("MagicTowerを選択しました。");
    }
}