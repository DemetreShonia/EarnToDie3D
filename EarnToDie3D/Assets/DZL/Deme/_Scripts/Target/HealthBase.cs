using UnityEngine;

namespace DumbRide
{
    public abstract class HealthBase : MonoBehaviour, ITarget
    {
        [SerializeField] protected int _maxHealth;
        protected int _currentHealth;

        protected virtual void Awake()
        {
            _currentHealth  = _maxHealth;
        }

        public int CurrentHealth => _currentHealth;

        public int MaxHealth => _maxHealth;

        public virtual void Die()
        {
            print("DEAD");
            Destroy(gameObject);
        }

        public virtual void TakeDamage(int amount)
        {
            print(_currentHealth);
            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }
}
