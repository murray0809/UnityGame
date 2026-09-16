using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 設置済みタワーをクリックしたときに表示される強化パネルのUI。
/// 選択中タワーのステータス表示と、強化ボタンの処理を担当する。
/// </summary>
public class TowerUpgradeUI : MonoBehaviour
{
    /// <summary>タワーのレベルを表示するテキスト。</summary>
    [SerializeField]
    private TMP_Text levelText;

    /// <summary>タワーの攻撃力を表示するテキスト。</summary>
    [SerializeField]
    private TMP_Text attackPowerText;

    /// <summary>タワーの攻撃範囲を表示するテキスト。</summary>
    [SerializeField]
    private TMP_Text attackRangeText;

    /// <summary>次の強化に必要な所持金を表示するテキスト。</summary>
    [SerializeField]
    private TMP_Text upgradeCostText;

    /// <summary>現在パネルに表示しているタワー。</summary>
    private TowerController selectedTower;

    /// <summary>強化ボタン。最大レベル時は押せないようにする。</summary>
    [SerializeField]
    private Button upgradeButton;

    /// <summary>
    /// 指定したタワーの情報で強化パネルを表示する。
    /// </summary>
    /// <param name="tower">選択されたタワー</param>
    public void Show(TowerController tower)
    {
        selectedTower = tower;

        UpdateUI();

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 強化パネルを閉じ、タワーの選択を解除する。
    /// </summary>
    public void Hide()
    {
        selectedTower = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 強化ボタンのOnClickから呼び出す。
    /// 選択中のタワーを強化し、表示を最新の値に更新する。
    /// </summary>
    public void OnUpgradeButton()
    {
        if (selectedTower == null)
        {
            return;
        }

        // 所持金のチェックと支払いはTowerController側で行う
        selectedTower.Upgrade();

        UpdateUI();
    }

    /// <summary>
    /// 選択中タワーのModelから値を読み取り、パネルの表示を更新する。
    /// </summary>
    private void UpdateUI()
    {
        if (selectedTower == null)
        {
            return;
        }

        TowerModel model = selectedTower.GetModel();

        levelText.text =
            "Level: " + model.Level;

        attackPowerText.text =
            "Attack: " + model.AttackPower;

        attackRangeText.text =
            "Range: " + model.AttackRange;

        upgradeCostText.text =
            "Cost: " + model.UpgradeCost;

        // 最大レベル（5）に達したら強化ボタンを押せなくする
        upgradeButton.interactable =
            model.Level < 5;
    }
}
