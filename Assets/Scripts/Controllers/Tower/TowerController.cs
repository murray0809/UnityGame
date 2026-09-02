using UnityEngine;

public class TowerController : MonoBehaviour
{
    private TowerModel model;
    private TowerView view;

    private float attackTimer;

    private void Awake()
    {
        model = new TowerModel(
            50,
            25,
            0.6f,
            3f
        );

        view = GetComponent<TowerView>();
    }

    private void Update()
    {
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

        target.TakeDamage(model.AttackPower);

        Debug.Log("Tower Attack!");
    }

    private EnemyController FindTarget()
    {
        EnemyController[] enemies =
            FindObjectsByType<EnemyController>(
                FindObjectsSortMode.None
            );

        EnemyController target = null;

        float closestDistance = float.MaxValue;

        foreach (EnemyController enemy in enemies)
        {
            float distance =
                Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance > model.AttackRange)
            {
                continue;
            }

            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy;
            }
        }

        return target;
    }
}