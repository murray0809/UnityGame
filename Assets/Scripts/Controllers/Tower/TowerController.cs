using UnityEngine;

public class TowerController : MonoBehaviour
{
    private TowerModel model;
    private TowerView view;

    private float attackTimer;

    [SerializeField]
    private TowerData towerData;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private bool isMagicTower;

    private SpriteRenderer spriteRenderer;

    private GameController gameController;

    private void Awake()
    {
        model = new TowerModel(
            towerData.cost,
            towerData.attackPower,
            towerData.attackInterval,
            towerData.attackRange,
            towerData.upgradeAttackPower,
            towerData.upgradeAttackRange
        );

        view = GetComponent<TowerView>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        gameController = FindFirstObjectByType<GameController>();
    }

    private void Update()
    {
        // ゲーム終了後は攻撃しない（Game Over後に無意味な演出が続くのを防ぐ）。
        if (gameController != null && gameController.IsGameEnded)
        {
            return;
        }

        attackTimer += Time.deltaTime;

        if (attackTimer < model.AttackInterval)
        {
            return;
        }

        attackTimer = 0f;

        Attack();
    }

    private void Attack()
    {
        EnemyController target = FindTarget();

        if (target == null)
        {
            return;
        }

        Vector3 direction =
            target.transform.position - transform.position;

        if (Mathf.Abs(direction.x) > 0.01f)
        {
            spriteRenderer.flipX = direction.x < 0f;
        }

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        ProjectileController projectile =
            projectileObject.GetComponent<ProjectileController>();

        if (projectile != null)
        {
            projectile.Initialize(
                target,
                model.AttackPower,
                isMagicTower
            );
        }
    }

    public void Upgrade()
    {
        if (gameController == null)
        {
            return;
        }

        if (!gameController.TryBuyTower(model.UpgradeCost))
        {
            Debug.Log("Moneyが足りません。");
            return;
        }

        model.Upgrade();

        Debug.Log(
            "Tower Upgraded! " +
            "AttackPower: " + model.AttackPower +
            " / AttackRange: " + model.AttackRange
        );
    }

    private EnemyController FindTarget()
    {
        EnemyController[] enemies =
            FindObjectsByType<EnemyController>(
                FindObjectsSortMode.None
            );

        EnemyController target = null;
        int furthestWaypoint = -1;
        float closestDistance = float.MaxValue;

        foreach (EnemyController enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            float distance =
                Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance > model.AttackRange)
            {
                continue;
            }

            if (enemy.CurrentWaypointIndex > furthestWaypoint)
            {
                furthestWaypoint = enemy.CurrentWaypointIndex;
                closestDistance = distance;
                target = enemy;
            }
            else if (
                enemy.CurrentWaypointIndex == furthestWaypoint &&
                distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy;
            }
        }

        return target;
    }

    private void OnDrawGizmosSelected()
    {
        if (model == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(
            transform.position,
            model.AttackRange
        );
    }

    public TowerModel GetModel()
    {
        return model;
    }
}