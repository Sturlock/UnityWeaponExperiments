using System;
using UnityEngine;

namespace Owl.Character.Enemies
{
	public class DummyTarget : MonoBehaviour, IDamageable, IHealth
	{
		[SerializeField] private Single _Health;

		// Start is called before the first frame update
		private void Start()
		{
			_Health = Mathf.Infinity;
		}

		/// <inheritdoc />
		public void DamageTarget(Single value)
		{
			Debug.Log($"Damaged {name} for {value} damage.");
			DecreaseHealth(value);
		}

		/// <inheritdoc />
		public void IncreaseHealth(Single value)
		{
			_Health += value;
		}

		/// <inheritdoc />
		public void DecreaseHealth(Single value)
		{
			_Health -= value;
			if (_Health <= 0)
			{
				OnDeath();
			}
		}

		/// <inheritdoc />
		public void OnDeath()
		{
			Destroy(this, 1f);
		}
	}
}