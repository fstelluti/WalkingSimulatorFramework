// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquipmentPanelData
// SMARTTEMPLATE SmartVarTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SmartData.SmartEquipmentPanelData.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartEquipmentPanelData.Data {
	/// <summary>
	/// ScriptableObject data which fires a Relay on data change.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData Variable", order=0)]
	public partial class EquipmentPanelDataVar : SmartVar<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData>, ISmartVar<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> {	// partial to allow overrides that don't get overwritten on regeneration
		#if UNITY_EDITOR
		const string VALUETYPE = "WalkingSimFramework.Scriptable_Objects.EquipmentPanelData";
		const string DISPLAYTYPE = "WalkingSimFramework.Scriptable_Objects.EquipmentPanelData";
		#endif

		[System.Serializable]
		public class EquipmentPanelDataEvent : UnityEvent<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData>{}
	}
}

namespace SmartData.SmartEquipmentPanelData {
	/// <summary>
	/// Read-only access to SmartEquipmentPanelData or WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, with built-in UnityEvent.
	/// For write access make a EquipmentPanelDataRefWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquipmentPanelDataReader : SmartDataRefBase<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataVar, EquipmentPanelDataConst, EquipmentPanelDataMulti> {
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onUpdate = null;
		
		protected sealed override System.Action<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Write access to SmartEquipmentPanelDataWriter or WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, with built-in UnityEvent.
	/// For read-only access make a EquipmentPanelDataRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquipmentPanelDataWriter : SmartDataRefWriter<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataVar, EquipmentPanelDataConst, EquipmentPanelDataMulti> {
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onUpdate = null;
		
		protected sealed override System.Action<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(WalkingSimFramework.Scriptable_Objects.EquipmentPanelData value){
			_onUpdate.Invoke(value);
		}
	}
}