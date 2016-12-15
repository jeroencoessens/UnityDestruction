#if CHRONOS_PLAYMAKER

using HutongGames.PlayMaker;

namespace Chronos.PlayMaker
{
	[ActionCategory("Physics 2d (Chronos)")]
	[Tooltip("Controls whether 2D physics affects the Game Object.")]
	public class SetSimulated : ChronosComponentAction<Timeline>
	{
		[RequiredField]
		[CheckForComponent(typeof(Timeline))]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		public FsmBool simulated;

		public override void Reset()
		{
			gameObject = null;
			simulated = false;
		}

		public override void OnEnter()
		{
			DoSetIsKinematic();
			Finish();
		}

		private void DoSetIsKinematic()
		{
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject))) return;

			timeline.rigidbody2D.simulated = simulated.Value;
		}
	}
}

#endif
