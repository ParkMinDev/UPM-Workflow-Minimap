using System;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.Foundation.Constants;
using ParkMinDev.UPM.Foundation.Interfaces;
using ParkMinDev.UPM.Workflow.Minimap.Components.UIs;
using ParkMinDev.UPM.Workflow.Minimap.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParkMinDev.UPM.Workflow.Minimap.Components.UILogics
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Minimap.Components.UILogics", sourceAssembly: "ParkMinPackages.Workflow.Minimap", sourceClassName: "MiniMapUISmoothFeature")]
	[RequireComponent(typeof(MiniMapUI))]
	public sealed class MiniMapUISmoothFeature : Feature<MiniMapUI>, IMiniMapUI, IR3PostLateUpdatable
	{
		// - Public Methods -
		public override void ValidateDependencies() {
			base.ValidateDependencies();
			if (_smoothness <= 0f)
				throw new ArgumentOutOfRangeException(nameof(Smoothness));
		}

		public void SetView(
			Vector3 center,
			float rotation,
			float viewWorldHeight
		) {
			_targetCenter = center;
			_targetRotation = rotation;
			_targetViewWorldHeight = Mathf.Max(0.01f, viewWorldHeight);
		}

		public void SetCenter(Vector3 center) {
			SetView(center, Rotation, ViewWorldHeight);
		}

		public void SetRotation(float rotation) {
			SetView(Center, rotation, ViewWorldHeight);
		}

		public void SetViewWorldHeight(float viewWorldHeight) {
			SetView(Center, Rotation, viewWorldHeight);
		}

		public void SnapToTargetView() {
			Owner.SetView(_targetCenter, _targetRotation, _targetViewWorldHeight);
		}

		// - Public Properties -
		public MiniMapUI MiniMapUI
		{
			get { return Owner; }
		}
		public Vector3 Center
		{
			get { return _targetCenter; }
		}
		public float Rotation
		{
			get { return _targetRotation; }
		}
		public float ViewWorldHeight
		{
			get { return _targetViewWorldHeight; }
		}
		public float ViewAspectRatio
		{
			get { return Owner.ViewAspectRatio; }
		}
		public float Smoothness
		{
			get { return _smoothness; }
			set
			{
				if (value <= 0f)
					throw new ArgumentOutOfRangeException(nameof(value));

				_smoothness = value;
			}
		}

		// - Handler -
		void IR3PostLateUpdatable.R3PostLateUpdate() {
			UpdateSmoothedView();
		}

		// - Internals -
		[Title(Headers.Settings)]
		[SerializeField, Min(0.01f)] float _smoothness = 10f;

		Vector3 _targetCenter;
		float _targetRotation;
		float _targetViewWorldHeight;

		void UpdateSmoothedView() {
			if (Owner.Center == _targetCenter &&
			    Mathf.Approximately(Owner.Rotation, _targetRotation) &&
			    Mathf.Approximately(Owner.ViewWorldHeight, _targetViewWorldHeight))
				return;

			float interpolation = 1f - Mathf.Exp(-_smoothness * Time.deltaTime);
			Vector3 center = Vector3.Lerp(Owner.Center, _targetCenter, interpolation);
			float rotation = Mathf.LerpAngle(Owner.Rotation, _targetRotation, interpolation);
			float viewWorldHeight = Mathf.Lerp(Owner.ViewWorldHeight, _targetViewWorldHeight, interpolation);
			if ((center - _targetCenter).sqrMagnitude <= 0.000001f)
				center = _targetCenter;
			if (Mathf.Abs(Mathf.DeltaAngle(rotation, _targetRotation)) <= 0.001f)
				rotation = _targetRotation;
			if (Mathf.Abs(viewWorldHeight - _targetViewWorldHeight) <= 0.001f)
				viewWorldHeight = _targetViewWorldHeight;
			Owner.SetView(center, rotation, viewWorldHeight);
		}
	}
}
