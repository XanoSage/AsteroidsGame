using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIButtonCommandReceiver
{
	UIButtonCommand Command();
}

public class UIButtonCommand
{
	public delegate void OnClickedHandler(ViewPresenter button);
	public event OnClickedHandler Clicked;

	public virtual void OnClicked(ViewPresenter button)
	{
		if (Clicked != null)
			Clicked(button);
	}

	public delegate void UndoHandler();
	public UndoHandler Undo;

	public virtual void Cancel()
	{
		if (Undo != null)
			Undo();
	}

	public delegate void OnSelectHandler(ViewPresenter button);
	public OnSelectHandler Selected;

	public virtual void OnSelected(ViewPresenter button)
	{
		if (Selected != null)
			Selected(button);
	}
}