using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    [SerializeField] private ArrowView arrowView;
    [SerializeField] private LayerMask TargetLayerMask;

    public void StartTargeting(Vector3 startPosition)
    {
        arrowView.gameObject.SetActive(true);
        arrowView.SetupArrow(startPosition);
    }
    public EnemyView EndTargeting(Vector3 endPosition)
    {
        arrowView.gameObject.SetActive(false);
        if(Physics.Raycast(endPosition, Vector3.forward, out RaycastHit hit, 10f, TargetLayerMask)
        && hit.collider != null
        && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            return enemyView;
        }
        else
        {
            return null;
        }
    }
}
