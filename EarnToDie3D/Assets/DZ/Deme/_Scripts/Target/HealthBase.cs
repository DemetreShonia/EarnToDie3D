using UnityEngine;

namespace DumbRide
{
    public abstract class HealthBase : MonoBehaviour, ITarget
    {
        [SerializeField] protected int _maxHealth;
        protected int _currentHealth;

        protected bool _isDead = false;

        protected virtual void Awake()
        {
            _currentHealth  = _maxHealth;
        }

        public int CurrentHealth => _currentHealth;

        public int MaxHealth => _maxHealth;

        public virtual void Die()
        {
            _isDead = true;
        }

        public virtual void TakeDamage(int amount)
        {
            if (_isDead) return;
            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }
}
