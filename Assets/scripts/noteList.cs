using NUnit.Framework.Constraints;
using UnityEngine;

public class noteList : MonoBehaviour
{
	public float GetHeightFromPitch(string pitch)
	{
		switch (pitch)
		{
			case "D4":
				return -8.5f;
			case "E4":
				return -7.5f;
			case "G4":
				return -5.5f;
			case "A4":
				return -4.5f;
			case "C5":
				return -2.5f;
			case "D5":
				return -1.5f;
			case "E5":
				return 0.5f;
			case "G5":
				return 2.5f;
			default:
				return 0f;


		}
	}
}
