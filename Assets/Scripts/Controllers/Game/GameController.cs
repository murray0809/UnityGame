using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体の進行（ライフ・所持金・勝敗判定・リトライ）を管理するController。
/// GameModelの値を更新し、その結果をGameUIへ反映する。
/// 勝敗はイベントで通知し、各システムが個別に停止処理を行えるようにしている。
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>ライフ・所持金などのゲーム状態を保持するModel。</summary>
    private GameModel model;

    /// <summary>ゲーム全体の見た目を担当するView。</summary>
    [SerializeField]
    private GameView view;

    /// <summary>ライフ・所持金・結果を表示するHUD。</summary>
    [SerializeField]
    private GameUI gameUI;

    /// <summary>ゲームオーバーまたはゲームクリアで終了しているかどうか。</summary>
    public bool IsGameEnded { get; private set; }

    /// <summary>ゲーム終了（敗北）を通知するイベント。</summary>
    public event Action OnGameOver;

    /// <summary>ゲーム終了（クリア）を通知するイベント。</summary>
    public event Action OnGameClear;

    /// <summary>
    /// 初期ライフ15、初期所持金150でゲーム状態を生成する。
    /// </summary>
    private void Awake()
    {
        model = new GameModel(15, 150);
    }

    /// <summary>
    /// 初期状態をHUDに表示する。
    /// </summary>
    private void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// 敵がゴールに到達したときにライフを減らす。
    /// ライフが0以下になった場合はゲームオーバーにする。
    /// </summary>
    /// <param name="damage">減らすライフ量</param>
    public void DamageLife(int damage)
    {
        // 終了後にライフが減り続けないようにする
        if (IsGameEnded) return;

        model.DecreaseLife(damage);

        Debug.Log("Life: " + model.Life);

        UpdateUI();

        if (model.Life <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// タワーの設置・強化のために所持金を支払う。
    /// </summary>
    /// <param name="cost">必要な金額</param>
    /// <returns>支払いに成功した場合はtrue</returns>
    public bool TryBuyTower(int cost)
    {
        // 終了後は購入できないようにする
        if (IsGameEnded) return false;

        bool success = model.TrySpendMoney(cost);

        if (success)
        {
            Debug.Log("Tower purchased! Money: " + model.Money);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money!");
        }

        return success;
    }

    /// <summary>
    /// 敵撃破の報酬として所持金を加算する。
    /// </summary>
    /// <param name="amount">加算する金額</param>
    public void AddMoney(int amount)
    {
        // 終了後に所持金が増えないようにする
        if (IsGameEnded) return;

        model.AddMoney(amount);

        Debug.Log("Money: " + model.Money);

        UpdateUI();
    }

    /// <summary>
    /// Modelの現在値をHUDに反映する。
    /// </summary>
    private void UpdateUI()
    {
        if (gameUI == null)
        {
            return;
        }

        gameUI.UpdateLife(model.Life);
        gameUI.UpdateMoney(model.Money);
    }

    /// <summary>
    /// ゲームオーバー処理。結果を表示し、購読しているシステムへ敗北を通知する。
    /// </summary>
    private void GameOver()
    {
        IsGameEnded = true;
        Debug.Log("Game Over!");

        if (gameUI != null)
        {
            gameUI.ShowResult("GAME OVER");
        }

        // WaveControllerやEnemySpawnerに停止を促す
        OnGameOver?.Invoke();
    }

    /// <summary>
    /// ゲームクリア処理。全Waveを消化したときにWaveControllerから呼び出される。
    /// </summary>
    public void GameClear()
    {
        // ゲームオーバーと同時に呼ばれても二重に終了処理をしない
        if (IsGameEnded) return;

        IsGameEnded = true;
        Debug.Log("Game Clear!");

        if (gameUI != null)
        {
            gameUI.ShowResult("GAME CLEAR");
        }

        OnGameClear?.Invoke();
    }

    /// <summary>
    /// RetryButtonのOnClickから呼び出す。
    /// 現在のシーンを読み込み直して、ゲームを最初からやり直す。
    /// </summary>
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
