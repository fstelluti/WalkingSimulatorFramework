// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquippedPanelData
// SMARTTEMPLATE SmartVarTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SmartData.SmartEquippedPanelData.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartEquippedPanelData.Data {
	/// <summary>
	/// ScriptableObject data which fires a Relay on data change.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData Variable", order=0)]
	public partial class EquippedPanelDataVar : SmartVar<WalkingSimFramework.Scriptable_Objects.EquippedPanelData>, ISmartVar<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> {	// partial to allow overrides that don't get overwritten on regeneration
		#if UNITY_EDITOR
		const string VALUETYPE = "WalkingSimFramework.Scriptable_Objects.EquippedPanelData";
		const string DISPLAYTYPE = "WalkingSimFramework.Scriptable_Objects.EquippedPanelData";
		#endif

		[System.Serializable]
		public class EquippedPanelDataEvent : UnityEvent<WalkingSimFramework.Scriptable_Objects.EquippedPanelData>{}
	}
}

namespace SmartData.SmartEquippedPanelData {
	/// <summary>
	/// Read-only access to SmartEquippedPanelData or WalkingSimFramework.Scriptable_Objects.EquippedPanelData, with built-in UnityEvent.
	/// For write access make a EquippedPanelDataRefWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataReader : SmartDataRefBase<WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataVar, EquippedPanelDataConst, EquippedPanelDataMulti> {
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onUpdate = null;
		
		protected sealed override System.Action<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Write access to SmartEquippedPanelDataWriter or WalkingSimFramework.Scriptable_Objects.EquippedPanelData, with built-in UnityEvent.
	/// For read-only access make a EquippedPanelDataRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataWriter : SmartDataRefWriter<WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataVar, EquippedPanelDataConst, EquippedPanelDataMulti> {
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onUpdate = null;
		
		protected sealed override System.Action<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(WalkingSimFramework.Scriptable_Objects.EquippedPanelData value){
			_onUpdate.Invoke(value);
		}
	}
}