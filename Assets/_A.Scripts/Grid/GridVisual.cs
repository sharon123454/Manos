using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DecalProjector))]
public class GridVisual : MonoBehaviour
{
    public bool IsActive { get { return _isActive; } private set { _isActive = value; } }

    private DecalProjector _decalProjector;
    private bool _isActive;
    public Outline Outline;
    private Color _transparant = new(0, 0, 0, 0);
    private void Awake()
    {
        _decalProjector = GetComponent<DecalProjector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mouse") && Outline.OutlineColor != _transparant)
            _decalProjector.enabled = true;
    }
    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Mouse"))
        //Debug.Log("on stay");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mouse"))
            _decalProjector.enabled = false;
    }

    public void SetIsActivate(bool active) { IsActive = active; }

}