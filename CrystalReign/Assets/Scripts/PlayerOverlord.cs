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

    public Animator head;
    private bool idle;
    private float idleTime;
    private float randomIdleInterval = 0;
    public float idleInterval;

    void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
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
            case OverlordMessage.RESTORE_LIFE:
                {
                    HP = Mathf.Min(HP + value, maxHP);
                    hpBar.setHP(HP / maxHP);
                    break;
                }
        }
    }

    private void Update()
    {
        if (HP < maxHP)
            HP += 0.004f;
        hpBar.setHP(HP / maxHP);

        //Idle animation
        if ((idleTime = (idle ? idleTime + Time.deltaTime : 0)) > idleInterval + randomIdleInterval)
        {
            head.Play(RandomIdle());
            idleTime = 0;
            randomIdleInterval = Random.Range(0.0f, idleInterval);
        }
        idle = true;
    }

    public void setNotIdle()
    {
        idle = false;
    }

    private string RandomIdle()
    {
        return "BBIdle" + (Random.Range(1, 4)).ToString();
    }

    public override void Apply(Effect effect, Vector3 origin)
    {
        switch (effect.effectType)
        {
            case EffectType.REDUCE_HP:
                HpReduceEffect hpReduceEffect = (HpReduceEffect)effect;
                processMessage(OverlordMessage.CHANGE_PLAYER_HIT_POINTS, hpReduceEffect.value);
                break;
            case EffectType.RESTORE_HP:
                HpRestoreEffect hpRestoreEffect = (HpRestoreEffect)effect;
                processMessage(OverlordMessage.RESTORE_LIFE, hpRestoreEffect.value);
                break;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("GameEdge"))
        {
            processMessage(OverlordMessage.CHANGE_PLAYER_HIT_POINTS, maxHP);
        }
        else if (hit.gameObject.CompareTag("Health"))
        {
            if (!hit.gameObject.transform.parent.GetComponent<HealthPick>().wasUsed())
            {
                hit.gameObject.transform.parent.GetComponent<HealthPick>().use();   //without "use" technique one capsule may restore full HP
                processMessage(OverlordMessage.RESTORE_LIFE, 30);
                ObjectsSpawner.instance.GetComponent<AudioSource>().Play();
                Destroy(hit.collider.transform.parent.gameObject);
            }
        }

    }

}

public enum OverlordMessage
{
    CHANGE_PLAYER_HIT_POINTS,
    CHANGE_AMMUNITION,
    CHANGE_WEAPON,
    RESTORE_LIFE
}
