using System;
using System.Collections.Generic;
using System.Linq;

namespace Globetrotter
{
	public class ObjectDepot
	{
		private static object m_staticLockObject = new Object();
		private object m_lockObject = new Object();
		
		private static ObjectDepot m_instance = null;
		
		private Dictionary<Type, IList<object>> m_objects;
		
		private ObjectDepot()
		{
			m_objects = new Dictionary<Type, IList<object>>();
		}
		
		public static ObjectDepot Instance
		{
			get
			{
				lock(m_staticLockObject)
				{
					if(m_instance == null)
					{
						m_instance = new ObjectDepot();
					}
					
					return m_instance;
				}
			}
		}
		
		public T Retrive<T>()
		{
			lock(m_lockObject)
			{
				IList<T> objects = RetrieveAll<T>();
				
				if(objects.Count > 0)
				{
					return objects[0];
				}
				else
				{
					return default(T);
				}
			}
		}
		
		public IList<T> RetrieveAll<T>()
		{
			lock(m_lockObject)
			{
				Type key = typeof(T);
				
				if(m_objects.ContainsKey(key) == true)
				{
					IList<object> objects = m_objects[key];
					IList<T> genericObjects = new List<T>();
					
					for(int i = 0; i < objects.Count; i++)
					{
						genericObjects.Add((T)objects[i]);
					}
					
					return genericObjects;
				}
				else
				{
					return new List<T>();
				}
			}
		}
		
		public void Store<T>(T obj)
		{
			lock(m_lockObject)
			{
				if(obj != null)
				{
					Type key = typeof(T);
					
					if(m_objects.ContainsKey(key) == false)
					{
						m_objects[key] = new List<object>();
					}
					
					m_objects[key].Add(obj);
				}
			}
		}
	}
}
