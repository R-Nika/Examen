using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    public GameObject minionType1Prefab;
    public GameObject minionType2Prefab;
    public GameObject minionType3Prefab;

    private bool summoningMinions = false;
    public float summonInterval = 3f; // Time interval between summoning minions

    public override void Start()
    {
        base.Start();
        health = 500;
    }

    public override void Update()
    {
        base.Update();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= sightRange && !summoningMinions)
        {
            // Summon minions
            StartCoroutine(SummonMinionsRoutine());
        }
    }

    private IEnumerator SummonMinionsRoutine()
    {
        summoningMinions = true;

        SummonMinions();
        yield return new WaitForSeconds(summonInterval);

        summoningMinions = false;
    }

    public void SummonMinions()
    {
        int numberOfMinions = 3;
        List<GameObject> minionPrefabs = new List<GameObject> { minionType1Prefab, minionType2Prefab, minionType3Prefab };

        for (int i = 0; i < numberOfMinions; i++)
        {
            GameObject selectedMinionPrefab = minionPrefabs[Random.Range(0, minionPrefabs.Count)];

            Vector3 randomOffset = Random.onUnitSphere * 5f;
            Vector3 minionPosition = transform.position + randomOffset;

            RaycastHit hit;
            if (Physics.Raycast(minionPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Floor"))
                {
                    minionPosition = hit.point;

                    if (Vector3.Distance(minionPosition, transform.position) < 2f)
                    {
                        continue;
                    }
                    Instantiate(selectedMinionPrefab, minionPosition, Quaternion.identity);
                }
            }
        }
    }
}
