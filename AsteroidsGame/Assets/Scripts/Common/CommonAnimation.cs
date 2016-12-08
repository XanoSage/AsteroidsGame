using UnityEngine;
using System.Collections;
using System;

public static class CommonAnimation
{
	public static IEnumerator FadeIn(SpriteRenderer sprite, float duration)
	{
		float timeCounter = 0f;
		float startValue = 0f;
		float endValue = 1f;
		var color = sprite.color;
		sprite.gameObject.SetActive(true);
		while(timeCounter < duration)
		{
			var time01 = timeCounter / duration;
			color.a = Mathf.Lerp(startValue, endValue, time01);
			sprite.color = color;
			timeCounter += Time.deltaTime;
			yield return null;
		}
		color.a = endValue;
		sprite.color = color;
	}

	public static IEnumerator FadeOut(SpriteRenderer sprite, float duration, bool inactiveOnEnd = false)
	{
		float timeCounter = 0f;
		float startValue = 1f;
		float endValue = 0f;
		var color = sprite.color;
		while(timeCounter < duration)
		{
			var time01 = timeCounter / duration;
			color.a = Mathf.Lerp(startValue, endValue, time01);
			sprite.color = color;
			timeCounter += Time.deltaTime;
			yield return null;
		}
		color.a = endValue;
		sprite.color = color;
		if (inactiveOnEnd)
			sprite.gameObject.SetActive(false);
	}

	public static IEnumerator DelayedAction(float delay, Action action)
	{
		yield return new WaitForSeconds(delay);
		if (action != null)
		{
			action();
		}
	}
}
