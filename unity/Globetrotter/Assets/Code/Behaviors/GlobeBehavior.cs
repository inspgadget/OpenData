using UnityEngine;
using System.Collections;

using Globetrotter.GuiLayer.ViewModel;

public class GlobeBehavior : MonoBehaviour
{
	private GlobeViewModel m_globeViewModel;

	void Update()
	{
		lock(m_globeViewModel.LockObject)
		{
			float horizontalAngle = m_globeViewModel.HorizontalAngle;
			m_globeViewModel.HorizontalAngle = 0.0f;

			float verticalAngle = m_globeViewModel.VerticalAngle;
			m_globeViewModel.VerticalAngle = 0.0f;

			transform.Rotate(verticalAngle, horizontalAngle, 0.0f, Space.World);
		}
	}

	public void Init(GlobeViewModel globeViewModel)
	{
		m_globeViewModel = globeViewModel;
	}
}
