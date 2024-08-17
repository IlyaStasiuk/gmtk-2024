using System;


namespace Titan
{
    public interface IDestroyableTitanPart
    {
        event Action OnPartDestroy;

        void TakeDamage(float damage);
        float MaxHealth { get; }
        float Health { get; }
        float NormalizedHealth { get; }
    }
}