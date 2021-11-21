// SMARTTYPE WalkingSimFramework.UI_System.HUD.EquippedPanelData
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
	[CreateAssetMenu(menuName="SmartData/WalkingSimFramework.UI_System.HUD.EquippedPanelData/WalkingSimFramework.UI_System.HUD.EquippedPanelData Variable", order=0)]
	public partial class EquippedPanelDataVar : SmartVar<WalkingSimFramework.UI_System.HUD.EquippedPanelData>, ISmartVar<WalkingSimFramework.UI_System.HUD.EquippedPanelData> {	// partial to allow overrides that don't get overwritten on regeneration
		#if UNITY_EDITOR
		const string VALUETYPE = "WalkingSimFramework.UI_System.HUD.EquippedPanelData";
		const string DISPLAYTYPE = "WalkingSimFramework.UI_System.HUD.EquippedPanelData";
		#endif

		[System.Serializable]
		public class EquippedPanelDataEvent : UnityEvent<WalkingSimFramework.UI_System.HUD.EquippedPanelData>{}
	}
}

namespace SmartData.SmartEquippedPanelData {
	/// <summary>
	/// Read-only access to SmartEquippedPanelData or WalkingSimFramework.UI_System.HUD.EquippedPanelData, with built-in UnityEvent.
	/// For write access make a EquippedPanelDataRefWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataReader : SmartDataRefBase<WalkingSimFramework.UI_System.HUD.EquippedPanelData, EquippedPanelDataVar, EquippedPanelDataConst, EquippedPanelDataMulti> {
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onUpdate = null;
		
		protected sealed override System.Action<WalkingSimFramework.UI_System.HUD.EquippedPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
	}
	/// <summary>
	/// Write access to SmartEquippedPanelDataWriter or WalkingSimFramework.UI_System.HUD.EquippedPanelData, with built-in UnityEvent.
	/// For read-only access make a EquippedPanelDataRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataWriter : SmartDataRefWriter<WalkingSimFramework.UI_System.HUD.EquippedPanelData, EquippedPanelDataVar, EquippedPanelDataConst, EquippedPanelDataMulti> {
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onUpdate = null;
		
		protected sealed override System.Action<WalkingSimFramework.UI_System.HUD.EquippedPanelData> GetUnityEventInvoke(){
			return _onUpdate.Invoke;
		}
		protected sealed override void InvokeUnityEvent(WalkingSimFramework.UI_System.HUD.EquippedPanelData value){
			_onUpdate.Invoke(value);
		}
	}
}