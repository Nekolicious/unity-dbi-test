using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kunai object pooling.
/// </summary>
public class KunaiPool : MonoBehaviour
{
    public static KunaiPool Instance { get; private set; }
    public GameObject kunaiPrefab;
    public int poolSize = 10;
    private Queue<GameObject> _kunaiQueue = new();
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject kunai = Instantiate(kunaiPrefab);
            kunai.transform.parent = gameObject.transform;
            kunai.SetActive(false);
            _kunaiQueue.Enqueue(kunai);
        }
    }

    /// <summary>
    /// Get a kunai from the pool or create new one.
    /// </summary>
    /// <returns>Kunai game object.</returns>
    public GameObject GetKunai()
    {
        if (_kunaiQueue.Count > 0)
        {
            GameObject kunai = _kunaiQueue.Dequeue();
            kunai.SetActive(true);
            return kunai;
        }
        else
        {
            GameObject newKunai = Instantiate(kunaiPrefab);
            return newKunai;
        }
    }

    /// <summary>
    /// Deactivate kunai from the scene and return it to the pool.
    /// </summary>
    /// <param name="kunai">Kunai game object.</param>
    public void ReturnKunai(GameObject kunai)
    {
        kunai.SetActive(false);
        _kunaiQueue.Enqueue(kunai);
    }
}
