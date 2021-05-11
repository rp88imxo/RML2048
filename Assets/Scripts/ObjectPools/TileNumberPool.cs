using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RML/Pools/Tile Number")]
public class TileNumberPool : ObjectPool
{
    [SerializeField]
    TileNumber[] tileNumberPrefab;

    [SerializeField]
    bool recycle;

    [System.NonSerialized]
    List<TileNumber> pools;

    int TotalPrefabs => tileNumberPrefab.Length;

    void CreatePools()
    {
        pools = new List<TileNumber>();
    }

    public TileNumber Get(int index)
    {
        if (index >= TotalPrefabs || index < 0)
        {
            return null;
        }

        TileNumber tileNumberObject;

        if (recycle)
        {
            if (pools == null || !SceneManager.GetSceneByName(name).isLoaded)
            {
                CreatePools();
            }

            int lastIndex = pools.Count - 1;
            if (lastIndex < 0)
            {
                tileNumberObject = CreateGameObjectInstance(tileNumberPrefab[index], true);
                tileNumberObject.OriginPool = this;
            }
            else
            {
                tileNumberObject = pools[lastIndex];
                tileNumberObject.gameObject.SetActive(true);
                tileNumberObject.ResetStateNoDestroy();
                pools.RemoveAt(lastIndex);
            }
        }
        else
        {
            tileNumberObject = CreateGameObjectInstance(tileNumberPrefab[index], false);
            tileNumberObject.OriginPool = this;
        }

        return tileNumberObject;
    }

    public TileNumber GetRandom()
    {
        return Get(Random.Range(0, TotalPrefabs));
    }

    public void Reclaim(TileNumber platformObject)
    {
        if (platformObject.OriginPool != this)
        {
            Debug.LogError("Operating an object from different Origin Pool!");
        }

        if (recycle)
        {
            if (pools == null || !SceneManager.GetSceneByName(name).isLoaded)
            {
                CreatePools();
            }

            pools.Add(platformObject);
            platformObject.gameObject.SetActive(false);

        }
        else
        {
            Destroy(platformObject.gameObject);
        }

    }

}
