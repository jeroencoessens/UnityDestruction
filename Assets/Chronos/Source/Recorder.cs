using UnityEngine;

namespace Chronos
{
	/// <summary>
	/// An abstract base component that saves snapshots at regular intervals to enable rewinding.
	/// </summary>
	[HelpURL("http://ludiq.io/chronos/documentation#Recorder")]
	public abstract class Recorder<TSnapshot> : MonoBehaviour
	{
		private class DelegatedRecorder : RecorderTimeline<Component, TSnapshot>
		{
			private Recorder<TSnapshot> parent;

			public DelegatedRecorder(Recorder<TSnapshot> parent, Timeline timeline) : base(timeline, null)
			{
				this.parent = parent;
			}

			protected override void ApplySnapshot(TSnapshot snapshot)
			{
				parent.ApplySnapshot(snapshot);
			}

			protected override TSnapshot CopySnapshot()
			{
				return parent.CopySnapshot();
			}

			protected override TSnapshot LerpSnapshots(TSnapshot from, TSnapshot to, float t)
			{
				return parent.LerpSnapshots(from, to, t);
			}
		}

		protected virtual void Awake()
		{
			CacheComponents();
		}

		protected virtual void Start()
		{
			recorder.Start();
		}

		protected virtual void Update()
		{
			recorder.Update();
		}

		/// <summary>
		/// Modifies all snapshots via the specified modifier delegate.
		/// </summary>
		public virtual void ModifySnapshots(RecorderTimeline<Component, TSnapshot>.SnapshotModifier modifier)
		{
			recorder.ModifySnapshots(modifier);
		}

		private Timeline timeline;
		private RecorderTimeline<Component, TSnapshot> recorder;

		protected abstract void ApplySnapshot(TSnapshot snapshot);
		protected abstract TSnapshot CopySnapshot();
		protected abstract TSnapshot LerpSnapshots(TSnapshot from, TSnapshot to, float t);

		public virtual void CacheComponents()
		{
			timeline = GetComponent<Timeline>();

			if (timeline == null)
			{
				throw new ChronosException(string.Format("Missing timeline for recorder."));
			}

			recorder = new DelegatedRecorder(this, timeline);
		}
	}
}
