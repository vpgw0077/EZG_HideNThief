using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject[] Items;
    public int SpawnCount;// 아이템이 스폰될 갯수
    private int ItemType;// Items 배열
    Vector3 SpawnZone;

    // Start is called before the first frame update
    void Start()
    {
        SpawnZone = transform.position;
        SpawnCount = Random.Range(2, 5);
        for (int i = 0; i < SpawnCount; i++)
        {
            ItemType = Random.Range(0, Items.Length);
            Instantiate(Items[ItemType], SpawnZone, Quaternion.identity);
            SpawnZone.x += 1;

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
