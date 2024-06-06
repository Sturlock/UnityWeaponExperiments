using System;

namespace Owl
{
    public interface IHealth
    {
        public void IncreaseHealth(Single value);

        public void DecreaseHealth(Single value);

        public void OnDeath();
    }
}
