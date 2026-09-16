using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// マウスクリックによるタワーの設置と選択を管理するController。
/// 既存タワーをクリックした場合は強化UIを開き、
/// 空いている場所をクリックした場合は選択中のタワーを設置する。
/// </summary>
public class TowerPlacer : MonoBehaviour
{
    /// <summary>通常タワーのPrefab。</summary>
    [SerializeField]
    private GameObject towerPrefab;

    /// <summary>通常タワーのデータ（設置コストの参照に使用）。</summary>
    [SerializeField]
    private TowerData towerData;

    /// <summary>魔法タワーのPrefab。</summary>
    [SerializeField]
    private GameObject magicTowerPrefab;

    /// <summary>魔法タワーのデータ（設置コストの参照に使用）。</summary>
    [SerializeField]
    private TowerData magicTowerData;

    /// <summary>タワーを設置できる範囲を表すCollider。</summary>
    [SerializeField]
    private Collider2D placementArea;

    /// <summary>敵の通路を表すCollider群。この上にはタワーを設置できない。</summary>
    [SerializeField]
    private Collider2D[] enemyPathColliders;

    /// <summary>既存タワーをクリックしたときに開く強化UI。</summary>
    [SerializeField]
    private TowerUpgradeUI towerUpgradeUI;

    /// <summary>ゲーム終了判定と設置コストの支払いに使用する。</summary>
    private GameController gameController;

    /// <summary>現在選択しているTowerのPrefab。</summary>
    private GameObject selectedTowerPrefab;

    /// <summary>現在選択しているTowerのデータ。</summary>
    private TowerData selectedTowerData;

    /// <summary>
    /// 参照を取得し、初期状態では通常タワーを選択しておく。
    /// </summary>
    private void Awake()
    {
        gameController =
            FindFirstObjectByType<GameController>();

        // 最初は通常Towerを選択
        selectedTowerPrefab = towerPrefab;

        selectedTowerData = towerData;
    }

    /// <summary>
    /// クリックを検出し、タワーの選択または設置を行う。
    /// </summary>
    private void Update()
    {
        // ゲーム終了後は設置・選択を受け付けない
        if (gameController != null && gameController.IsGameEnded)
        {
            return;
        }

        // マウスが接続されていない環境では処理しない
        if (Mouse.current == null)
        {
            Debug.Log("Mouse.current が null");
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // UI（ボタンなど）の上をクリックした場合は無視する
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Debug.Log("クリック検出");

            // マウスのスクリーン座標をワールド座標に変換する
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            // zにカメラからの距離を指定しないと、変換結果がカメラ位置になってしまう
            Vector3 screenPosition =
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -Camera.main.transform.position.z
                );

            Vector3 worldPosition =
                Camera.main.ScreenToWorldPoint(screenPosition);

            // 2Dゲームなのでz座標は0に揃える
            worldPosition.z = 0f;

            Debug.Log("クリック位置: " + worldPosition);

            // クリック位置にあるColliderをすべて取得する
            Collider2D[] hits =
                Physics2D.OverlapPointAll(worldPosition);

            foreach (Collider2D hit in hits)
            {
                Debug.Log("Collider発見: " + hit.gameObject.name);

                TowerController tower =
                    hit.GetComponent<TowerController>();

                // 既存タワーをクリックした場合は、設置せずに強化UIを開く
                if (tower != null)
                {
                    Debug.Log("Tower発見");

                    towerUpgradeUI.Show(tower);

                    return;
                }
            }

            // タワー以外の場所をクリックした場合は設置を試みる
            PlaceTower();
        }
    }

    /// <summary>
    /// マウス位置に選択中のタワーを設置する。
    /// 設置可能エリア外・敵の通路上・所持金不足の場合は設置しない。
    /// </summary>
    private void PlaceTower()
    {
        // マウスのスクリーン座標をワールド座標に変換する
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

        // 敵の通路の上なら終了（敵の進路をふさがないようにする）
        foreach (Collider2D pathCollider in enemyPathColliders)
        {
            if (pathCollider != null && pathCollider.OverlapPoint(worldPosition))
            {
                Debug.Log("敵の通路にはTowerを置けません。");
                return;
            }
        }

        // 設置コストを参照するためのデータが無ければ設置できない
        if (selectedTowerData == null)
        {
            Debug.LogError("選択中のTowerDataが設定されていません。");
            return;
        }

        int cost = selectedTowerData.cost;

        // 所持金が足りなければ設置しない（足りていればここで支払われる）
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

    /// <summary>
    /// 通常Towerを選択する。通常タワーボタンのOnClickから呼び出す。
    /// </summary>
    public void SelectNormalTower()
    {
        selectedTowerPrefab = towerPrefab;

        selectedTowerData = towerData;

        Debug.Log("通常Towerを選択しました。");
    }

    /// <summary>
    /// MagicTowerを選択する。魔法タワーボタンのOnClickから呼び出す。
    /// </summary>
    public void SelectMagicTower()
    {
        selectedTowerPrefab = magicTowerPrefab;

        selectedTowerData = magicTowerData;

        Debug.Log("MagicTowerを選択しました。");
    }
}
