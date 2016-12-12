using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class UIButtonPresenter : ViewPresenter, IUIButtonCommandReceiver
{
	private UIButtonCommand _command;

	[SerializeField]
	bool _ignoreInteractive;
	[SerializeField]
	string _id;

	protected Button _button;

	public UIButtonPresenter()
	{

	}

	void Start()
	{
		OnStarting();
	}

	protected virtual void OnStarting()
	{ }

	public UIButtonCommand Command()
	{
		if (_command == null)
			_command = new UIButtonCommand();
		return _command;
	}

	void Awake()
	{
		_button = GetComponent<Button>();
		_button.onClick.AddListener(OnClicked);
	}

	protected virtual void OnClicked()
	{
		if (_command != null)
			_command.OnClicked(this);
	}

	protected virtual void OnSelected()
	{
		if (_command != null)
			_command.OnSelected(this);
	}

	public Button GetButton()
	{
		return _button;
	}

	protected virtual void SetInteractive(bool value)
	{
		if (_button != null)
			_button.interactable = value;
	}
}
