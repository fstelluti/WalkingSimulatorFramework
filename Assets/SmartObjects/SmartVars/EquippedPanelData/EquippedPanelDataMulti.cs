// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquippedPanelData
// SMARTTEMPLATE SmartMultiTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.SmartEquippedPanelData.Data;
using SmartData.Abstract;
using SmartData.Interfaces;
using Sigtrap.Relays;

namespace SmartData.SmartEquippedPanelData.Data {
	/// <summary>
	/// Dynamic collection of EquippedPanelDataVar assets.
	/// </summary>
	[CreateAssetMenu(menuName="SmartData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData Multi", order=1)]
	public class EquippedPanelDataMulti: SmartMulti<WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataVar>, ISmartMulti<WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataVar> {
		#if UNITY_EDITOR
		const string VALUETYPE = "WalkingSimFramework.Scriptable_Objects.EquippedPanelData";
		const string DISPLAYTYPE = "WalkingSimFramework.Scriptable_Objects.EquippedPanelData Multi";
		#endif
	}
}

namespace SmartData.SmartEquippedPanelData {
	/// <summary>
	/// Indexed reference into a EquippedPanelDataMulti (read-only access).
	/// For write access make a reference to EquippedPanelDataMultiRefWriter.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataMultiReader : SmartDataMultiRef<EquippedPanelDataMulti, WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataVar>  {
		public static implicit operator WalkingSimFramework.Scriptable_Objects.EquippedPanelData(EquippedPanelDataMultiReader r){
            return r.value;
		}
		
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onUpdate = null;
		
		protected override System.Action<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Indexed reference into a EquippedPanelDataMulti, with a built-in UnityEvent.
	/// For read-only access make a reference to EquippedPanelDataMultiRef.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataMultiWriter : SmartDataMultiRefWriter<EquippedPanelDataMulti, WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataVar> {
		public static implicit operator WalkingSimFramework.Scriptable_Objects.EquippedPanelData(EquippedPanelDataMultiWriter r){
            return r.value;
		}
		
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onUpdate = null;
		
		protected override System.Action<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(WalkingSimFramework.Scriptable_Objects.EquippedPanelData value){
			_onUpdate.Invoke(value);
		}
	}
}