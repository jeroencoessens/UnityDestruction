using UnityEngine;

namespace Chronos
{
	public interface IComponentTimeline
	{
		void Start();
		void Update();
		void FixedUpdate();
		void AdjustProperties();
	}

	public interface IComponentTimeline<T> : IComponentTimeline where T : Component
	{
		T component { get; }
	}

	public abstract class ComponentTimeline<T> : IComponentTimeline<T> where T : Component
	{
		protected Timeline timeline { get; private set; }
		public T component { get; internal set; }

		public ComponentTimeline(Timeline timeline, T component)
		{
			this.timeline = timeline;
			this.component = component;
			CopyProperties(component);
		}

		public virtual void Start() { }
		public virtual void Update() { }
		public virtual void FixedUpdate() { }
		public virtual void CopyProperties(T source) { }
		public virtual void AdjustProperties(float timeScale) { }

		public void AdjustProperties()
		{
			AdjustProperties(timeline.timeScale);
		}
	}
}
