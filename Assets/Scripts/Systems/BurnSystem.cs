using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSystem : MonoBehaviour
{
    [SerializeField] private GameObject burnVFX;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBurnGA>(ApplyBurnPerformer);
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<ApplyBurnGA>();
    }
    private IEnumerator ApplyBurnPerformer(ApplyBurnGA applyBurnGA)
    {
        CombatantView combatantView = applyBurnGA.Target;
        //Instantiate(burnVFX, combatantView.transform.position, Quaternion.identity);
        combatantView.Damage(applyBurnGA.BurnDamage);
        combatantView.RemoveStatusEffect(StatusEffectType.BURN,1);
        yield return new WaitForSeconds(1f);
    }
}
