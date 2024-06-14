using System;
using UnityEngine;

namespace Owl.Character.Enemies
{
	public class DummyTarget : MonoBehaviour, IDamageable, IHealth
	{
		[SerializeField] private Single _Health;

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
		}

		private void Update()
		{
			if (_Health <= 0)
			{
				OnDeath();
			}
		}

		/// <inheritdoc />
		public void OnDeath()
		{
			Destroy(gameObject, 1f);
		}
	}
}