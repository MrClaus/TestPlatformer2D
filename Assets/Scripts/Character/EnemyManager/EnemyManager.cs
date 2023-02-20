using System.Collections.Generic;
using UnityEngine;

// Creates enemies on the level
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform[] _respawnPoints;
    [SerializeField] private GameObject[] _enemyes;

    private List<int> _emptySlot = new List<int>();

    void Start()
    {
        if (_respawnPoints.Length > 0 && _enemyes.Length > 0)
        {
            int maxCount = (UIService.instance != null) ? UIService.instance.CountEnemy() : 1;
            maxCount = (maxCount > _respawnPoints.Length) ? _respawnPoints.Length : maxCount;

            for (int i = 0; i < maxCount; i++)
            {
                int id = Random.Range(0, maxCount);
                while (!IsEmpty(id))
                    id = Random.Range(0, maxCount);

                _emptySlot.Add(id);
                Instantiate(_enemyes[Random.Range(0, _enemyes.Length - 1)], _respawnPoints[id]);
            }
        }
    }

    private bool IsEmpty(int id)
    {
        if (_emptySlot.Count > 0)
        {
            foreach (int count in _emptySlot)
                if (count == id)
                    return false;
            return true;
        }
        else
            return true;
    }
}
