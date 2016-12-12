using UnityEngine;

public class ViewPresenter : MonoBehaviour
{
	[Header("View Presenter")]
	public RectTransform ViewBounds;

	private bool _isActive = false;
	public bool IsActive { get { return _isActive; } }

	public virtual void Show()
	{
		gameObject.SetActive(true);
		_isActive = true;
	}

	public virtual void Hide()
	{
		if (gameObject != null)
			gameObject.SetActive(false);

		_isActive = false;
	}

	public void SetPriority(int priority)
	{
		transform.SetSiblingIndex(priority);
	}

	public void DestroyView()
	{
		if (gameObject != null)
			Destroy(gameObject);
	}
}
