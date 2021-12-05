// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquipmentPanelData
// SMARTTEMPLATE SmartMultiTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.SmartEquipmentPanelData.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartEquipmentPanelData.Data {
	/// <summary>
	/// Dynamic collection of EquipmentPanelDataVar assets.
	/// </summary>
	[CreateAssetMenu(menuName="SmartData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData Multi", order=1)]
	public class EquipmentPanelDataMulti: SmartMulti<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataVar>, ISmartMulti<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataVar> {
		#if UNITY_EDITOR
		const string VALUETYPE = "WalkingSimFramework.Scriptable_Objects.EquipmentPanelData";
		const string DISPLAYTYPE = "WalkingSimFramework.Scriptable_Objects.EquipmentPanelData Multi";
		#endif
	}
}

namespace SmartData.SmartEquipmentPanelData {
	/// <summary>
	/// Indexed reference into a EquipmentPanelDataMulti (read-only access).
	/// For write access make a reference to EquipmentPanelDataMultiRefWriter.
	/// </summary>
	[System.Serializable]
	public class EquipmentPanelDataMultiReader : SmartDataMultiRef<EquipmentPanelDataMulti, WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataVar>  {
		public static implicit operator WalkingSimFramework.Scriptable_Objects.EquipmentPanelData(EquipmentPanelDataMultiReader r){
            return r.value;
		}
		
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onUpdate = null;
		
		protected override System.Action<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Indexed reference into a EquipmentPanelDataMulti, with a built-in UnityEvent.
	/// For read-only access make a reference to EquipmentPanelDataMultiRef.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquipmentPanelDataMultiWriter : SmartDataMultiRefWriter<EquipmentPanelDataMulti, WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataVar> {
		public static implicit operator WalkingSimFramework.Scriptable_Objects.EquipmentPanelData(EquipmentPanelDataMultiWriter r){
            return r.value;
		}
		
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onUpdate = null;
		
		protected override System.Action<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(WalkingSimFramework.Scriptable_Objects.EquipmentPanelData value){
			_onUpdate.Invoke(value);
		}
	}
}