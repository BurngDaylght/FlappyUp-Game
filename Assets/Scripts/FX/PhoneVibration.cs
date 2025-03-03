using UnityEngine;

public class PhoneVibration : MonoBehaviour
{
	private void OnEnable() 
	{
		GameState.instance.onGameOver += VibratePhone;
	}
	
	public void VibratePhone()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			Handheld.Vibrate();
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			#if UNITY_IOS
			iOS.Vibrate();
			#endif
		}
	}
}
