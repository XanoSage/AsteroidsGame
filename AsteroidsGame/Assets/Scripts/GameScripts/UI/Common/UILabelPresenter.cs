using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UILabelPresenter : ViewPresenter
{
	[SerializeField]
	private Text _label;

	public bool SetText(string text, bool isUpperCase = false)
	{
		if (_label == null)
			return false;

		_label.text = isUpperCase ? text.ToUpper() : text;
		return true;
	}

	public string GetText()
	{
		return _label.text;
	}

	public void SetTextColor(Color color)
	{
		_label.color = color;
	}
}