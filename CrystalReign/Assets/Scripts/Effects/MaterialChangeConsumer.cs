using UnityEngine;

public class MaterialChangeConsumer : EffectConsumer
{

    public Material mat1;
    public Material mat2;

    private bool mat1IsCurrentMat;
    
    public override void Apply(Effect effect)
    {
        if (effect.effectType == EffectType.CHANGE_MATERIAL) 
        {   
            if (mat1IsCurrentMat)
            {
                transform.GetComponent<MeshRenderer>().material = mat2;
            }
            else
            {
                transform.GetComponent<MeshRenderer>().material = mat1;
            }
            mat1IsCurrentMat = !mat1IsCurrentMat;
        }
    }
}
