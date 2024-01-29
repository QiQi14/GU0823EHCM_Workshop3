
using System;
using UnityEngine.EventSystems;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnDead;

    private int health;
    private int healthMax;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealthAmount(int health)
    {
        this.health = health;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public float GetHealthPercent()
    {
        return (float)health / healthMax;
    }

    public int GetHealthAmount()
    {
        return health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0) health = 0;
        if (OnHealthChanged != null) OnHealthChanged(this,EventArgs.Empty);

        if (health < 0)
        {
            Die();
        }

    }

    public void Die()
    {
        if (OnDead != null) OnDead(this,EventArgs.Empty);
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > healthMax) health = healthMax;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);

    }
}


