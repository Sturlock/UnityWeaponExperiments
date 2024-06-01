using System;
using UnityEngine;

namespace Owl
{
	public class Magazine : MonoBehaviour
	{
		[SerializeField] private Int32 _AmmunitionCount = 30;
		[SerializeField] private Int32 _MaxAmmunition = 30;

		public Int32 AmmunitionCount
		{
			get => _AmmunitionCount;
			set => _AmmunitionCount = value;
		}

		public Boolean Reload()
		{
			if (_AmmunitionCount >= _MaxAmmunition) return false;
			_AmmunitionCount = _MaxAmmunition;
			return true;
		}

		public Boolean CanShoot()
		{
			return _AmmunitionCount > 0;
		}
	}
}