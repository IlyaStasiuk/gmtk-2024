using System;


namespace Titan
{
    public interface IDestroyableTitanPart
    {
        event Action<float> OnPartDestroy;

        void TakeDamage(float damage, float force);
        float MaxHealth { get; }
        float Health { get; }
        float NormalizedHealth { get; }
    }
}