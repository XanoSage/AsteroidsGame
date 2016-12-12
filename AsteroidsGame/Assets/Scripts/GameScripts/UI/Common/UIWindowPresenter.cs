using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowPresenter : ViewPresenter
{
	[SerializeField] private WindowType _windowType;
	public WindowType Type { get { return _windowType; } }

	public virtual void Init()
	{ }
}
