using System.Collections;
using UnityEngine;

public class RapidFireVisuals : TowerVisuals
{
    [Header("Recoil Details")] [SerializeField]
    protected float recoilOffset = -0.2f;

    [SerializeField] protected float recoverSpeed = 1f;
    protected string onAttackVFX = "OnAttackFX";

    public void PlayOnAttackVFX(Vector3 position) => SpawnVFX(onAttackVFX, position, Quaternion.identity);

    public void RecoilVFX(Transform gunPoint)
    {
        PlayOnAttackVFX(gunPoint.position);
        StartCoroutine(RecoilCoroutine(gunPoint));
    }

    protected IEnumerator RecoilCoroutine(Transform gunPoint)
    {
        Transform objToMove = gunPoint.parent;
        Vector3 originalPosition = objToMove.localPosition;
        Vector3 recoilPosition = originalPosition + new Vector3(0, 0, recoilOffset);

        objToMove.localPosition = recoilPosition;

        while (objToMove.localPosition != originalPosition)
        {
            objToMove.localPosition =
                Vector3.MoveTowards(objToMove.localPosition, originalPosition, recoverSpeed * Time.deltaTime);
            yield return null;
        }
    }
}