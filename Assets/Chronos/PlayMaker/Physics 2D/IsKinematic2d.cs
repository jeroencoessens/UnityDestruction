#if CHRONOS_PLAYMAKER

using HutongGames.PlayMaker;

namespace Chronos.PlayMaker
{
	[ActionCategory("Physics 2d (Chronos)")]
	[Tooltip("Tests if a Game Object's Rigid Body 2D is simulated.")]
	public class IsSimulated : ChronosComponentAction<Timeline>
	{
		[RequiredField]
		[CheckForComponent(typeof(Timeline))]
		public FsmOwnerDefault gameObject;

		public FsmEvent trueEvent;

		public FsmEvent falseEvent;

		[UIHint(UIHint.Variable)]
		public FsmBool store;

		public bool everyFrame;

		public override void Reset()
		{
			gameObject = null;
			trueEvent = null;
			falseEvent = null;
			store = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoIsSimulated();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoIsSimulated();
		}

		private void DoIsSimulated()
		{
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject))) return;

			var isSimulated = timeline.rigidbody2D.simulated;
			store.Value = isSimulated;

			Fsm.Event(isSimulated ? trueEvent : falseEvent);
		}
	}
}

#endif
