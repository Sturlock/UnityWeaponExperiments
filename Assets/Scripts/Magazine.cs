using System;
using UnityEngine;

namespace Owl
{
	public class Magazine : MonoBehaviour
	{
		[SerializeField] private Int32 _AmmunitionCount = 30;
		[SerializeField] private Int32 _MaxAmmunition = 30;

		/// <summary>
		///    The amount of ammunition currently available in the Magazine.
		/// </summary>
		public Int32 AmmunitionCount
		{
			get => _AmmunitionCount;
			set => _AmmunitionCount = value;
		}

		/// <summary>
		///    Reloads the Magazine.
		/// </summary>
		/// <returns>
		///    Returns true if reloaded and false if not.
		/// </returns>
		public Boolean Reload()
		{
			if (_AmmunitionCount >= _MaxAmmunition) return false;
			_AmmunitionCount = _MaxAmmunition;
			return true;
		}

		/// <summary>
		///    Is there enough Ammunition to successfully shoot.
		/// </summary>
		/// <returns>
		///    If enough ammo is present it will return true, else false.
		/// </returns>
		public Boolean CanShoot()
		{
			return _AmmunitionCount > 0;
		}
	}
}