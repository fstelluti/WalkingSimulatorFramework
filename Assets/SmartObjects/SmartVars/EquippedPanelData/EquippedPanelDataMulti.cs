// SMARTTYPE WalkingSimFramework.UI_System.HUD.EquippedPanelData
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
	[CreateAssetMenu(menuName="SmartData/WalkingSimFramework.UI_System.HUD.EquippedPanelData/WalkingSimFramework.UI_System.HUD.EquippedPanelData Multi", order=1)]
	public class EquippedPanelDataMulti: SmartMulti<WalkingSimFramework.UI_System.HUD.EquippedPanelData, EquippedPanelDataVar>, ISmartMulti<WalkingSimFramework.UI_System.HUD.EquippedPanelData, EquippedPanelDataVar> {
		#if UNITY_EDITOR
		const string VALUETYPE = "WalkingSimFramework.UI_System.HUD.EquippedPanelData";
		const string DISPLAYTYPE = "WalkingSimFramework.UI_System.HUD.EquippedPanelData Multi";
		#endif
	}
}

namespace SmartData.SmartEquippedPanelData {
	/// <summary>
	/// Indexed reference into a EquippedPanelDataMulti (read-only access).
	/// For write access make a reference to EquippedPanelDataMultiRefWriter.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataMultiReader : SmartDataMultiRef<EquippedPanelDataMulti, WalkingSimFramework.UI_System.HUD.EquippedPanelData, EquippedPanelDataVar>  {
		public static implicit operator WalkingSimFramework.UI_System.HUD.EquippedPanelData(EquippedPanelDataMultiReader r){
            return r.value;
		}
		
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onUpdate = null;
		
		protected override System.Action<WalkingSimFramework.UI_System.HUD.EquippedPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Indexed reference into a EquippedPanelDataMulti, with a built-in UnityEvent.
	/// For read-only access make a reference to EquippedPanelDataMultiRef.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataMultiWriter : SmartDataMultiRefWriter<EquippedPanelDataMulti, WalkingSimFramework.UI_System.HUD.EquippedPanelData, EquippedPanelDataVar> {
		public static implicit operator WalkingSimFramework.UI_System.HUD.EquippedPanelData(EquippedPanelDataMultiWriter r){
            return r.value;
		}
		
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onUpdate = null;
		
		protected override System.Action<WalkingSimFramework.UI_System.HUD.EquippedPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(WalkingSimFramework.UI_System.HUD.EquippedPanelData value){
			_onUpdate.Invoke(value);
		}
	}
}