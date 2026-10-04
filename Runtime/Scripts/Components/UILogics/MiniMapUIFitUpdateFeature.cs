using System;
using ParkMinDev.UPM.Foundation.Components;
using ParkMinDev.UPM.Foundation.Constants;
using ParkMinDev.UPM.Foundation.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParkMinDev.UPM.Workflow.Minimap.Components.UILogics
{
	[UnityEngine.Scripting.APIUpdating.MovedFrom(true, sourceNamespace: "ParkMinPackages.Workflow.Minimap.Components.UILogics", sourceAssembly: "ParkMinPackages.Workflow.Minimap", sourceClassName: "MiniMapUIFitUpdateFeature")]
	public sealed class MiniMapUIFitUpdateFeature : Feature<MiniMapUIFitFeature>, IR3PreLateUpdatable
	{
		// - Public Methods -
		public override void ValidateDependencies() {
			base.ValidateDependencies();
			if (_pointA == null)
				throw new InvalidOperationException($"{nameof(PointA)} is not assigned.");
			if (_pointB == null)
				throw new InvalidOperationException($"{nameof(PointB)} is not assigned.");
		}

		public void SetPoints(Transform pointA, Transform pointB) {
			_pointA = pointA;
			_pointB = pointB;
		}

		public void RefreshFit() {
			Owner.Fit(_pointA.position, _pointB.position);
		}

		// - Public Properties -
		public MiniMapUIFitFeature FitFeature
		{
			get { return Owner; }
		}
		public Transform PointA
		{
			get { return _pointA; }
			set { _pointA = value; }
		}
		public Transform PointB
		{
			get { return _pointB; }
			set { _pointB = value; }
		}

		// - Handler -
		protected override void OnReady() {
			base.OnReady();
			RefreshFit();
		}

		void IR3PreLateUpdatable.R3PreLateUpdate() {
			RefreshFit();
		}

		// - Internals -
		[Title(Headers.Injectable)]
		[SerializeField] Transform _pointA;
		[SerializeField] Transform _pointB;
	}
}