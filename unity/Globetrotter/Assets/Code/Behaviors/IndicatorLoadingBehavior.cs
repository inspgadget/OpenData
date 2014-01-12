using UnityEngine;
using System.Collections;
using System.ComponentModel;

using Globetrotter.GuiLayer.ViewModel;

public class IndicatorLoadingBehavior : MonoBehaviour
{
	private object m_lockObj = new object();

	private IndicatorSelectorViewModel m_indicatorSelectorViewModel;

	private float m_speed;
	private bool m_isFetching;

	private float m_z;

	void Start()
	{
		m_z = transform.position.z;
	}

	void FixedUpdate()
	{
		bool isFetching = false;

		lock(m_lockObj)
		{
			isFetching = m_isFetching;
		}

		if(isFetching == true)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, m_z);
			transform.Rotate(m_speed, 0.0f, 0.0f, Space.Self);
		}
		else
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, m_z - 10.0f);
		}
	}

	void OnDestroy()
	{
		m_indicatorSelectorViewModel.PropertyChanged -= PropertyChangedHandler;
	}

	public void Init(IndicatorSelectorViewModel indicatorSelectorViewModel, float speed)
	{
		m_indicatorSelectorViewModel = indicatorSelectorViewModel;
		m_indicatorSelectorViewModel.PropertyChanged -= PropertyChangedHandler;
		m_indicatorSelectorViewModel.PropertyChanged += PropertyChangedHandler;

		m_speed = speed;
		m_isFetching = false;
	}

	public void PropertyChangedHandler(object sender, PropertyChangedEventArgs args)
	{
		if(sender == m_indicatorSelectorViewModel)
		{
			if(args.PropertyName == "IsFetching")
			{
				lock(m_lockObj)
				{
					m_isFetching = m_indicatorSelectorViewModel.IsFetching;
				}
			}
		}
	}
}
