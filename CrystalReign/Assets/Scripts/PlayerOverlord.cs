using System;
using Assets.Scripts.Effects;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Effects;

public class PlayerOverlord : EffectConsumer
{

    private const float maxHP = 100;
    public float HP = maxHP;
    public double AmmunitionLeft = 100;
    public Weapon SelectedPlayerWeapon;
    public WeaponChanger WeaponChanger;
    public HPBar hpBar;
    public Animator hpBckAnim;


    void Start()
    {
    }

    public void processMessage(OverlordMessage message, float value)
    {
        switch (message)
        {
            case OverlordMessage.CHANGE_PLAYER_HIT_POINTS:
                {
                    HP -= value;
                    hpBar.setHP(HP / maxHP);
                    hpBckAnim.Play("Fadeout");
                    if (HP <= 0)
                    {
                        SceneManager.LoadScene("GameOver");
                    }
                    break;
                }
            case OverlordMessage.CHANGE_AMMUNITION:
                {
                    AmmunitionLeft += value;
                    break;
                }
            case OverlordMessage.CHANGE_WEAPON:
                {
                    SelectedPlayerWeapon = value > 0 ?
                        WeaponChanger.GetNextWeapon() :
                        WeaponChanger.GetPreviousWeapon();
                    break;
                }
        }
    }

    private void Update()
    {
        if (HP < maxHP)
            HP += 0.004f;
        hpBar.setHP(HP / maxHP);
    }

    public override void Apply(Effect effect, Vector3 origin)
    {
        switch (effect.effectType)
        {
            case EffectType.REDUCE_HP:
                HpReduceEffect hpReduceEffect = (HpReduceEffect)effect;
                processMessage(OverlordMessage.CHANGE_PLAYER_HIT_POINTS, hpReduceEffect.value);
                break;
        }
    }
}

public enum OverlordMessage
{
    CHANGE_PLAYER_HIT_POINTS,
    CHANGE_AMMUNITION,
    CHANGE_WEAPON
}
