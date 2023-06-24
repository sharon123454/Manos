using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AOEManager : MonoBehaviour
{
    public static AOEManager Instance { get; private set; }
    public List<Unit> enemyUnits;
    public List<Unit> freindyUnits;
    public Collider collider;
    private void Awake()
    {
        collider = GetComponent<Collider>();
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }
    public List<Unit> DetectAttack()
    {
        return enemyUnits;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>() != null)
        {
            if (!enemyUnits.Contains(other.gameObject.GetComponent<Unit>()))
                enemyUnits.Add(other.gameObject.GetComponent<Unit>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>() != null)
        {
            if (enemyUnits.Contains(other.gameObject.GetComponent<Unit>()))
                enemyUnits.Remove(other.gameObject.GetComponent<Unit>());
        }
    }
}
