// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquipmentPanelData
// SMARTTEMPLATE SmartSetTemplate
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
	/// ScriptableObject data set which fires a Relay on data addition/removal.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData Set", order=2)]
	public class EquipmentPanelDataSet : SmartSet<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData>, ISmartDataSet<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> {
		#if UNITY_EDITOR
		const string VALUETYPE = "WalkingSimFramework.Scriptable_Objects.EquipmentPanelData";
		const string DISPLAYTYPE = "WalkingSimFramework.Scriptable_Objects.EquipmentPanelData Set";
		#endif
	}
}

namespace SmartData.SmartEquipmentPanelData {
	/// <summary>
	/// Read-only access to EquipmentPanelDataSet or List<0>, with built-in UnityEvent.
	/// For write access make a EquipmentPanelDataSetWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquipmentPanelDataSetReader : SmartSetRefBase<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataSet>, ISmartSetRefReader<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> {
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onAdd = null;
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onRemove = null;
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onChange = null;
		
		protected override System.Action<SetEventData<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData>> GetUnityEventInvoke(){
			return (d)=>{
				switch (d.operation){
					case SetOperation.ADDED:
						_onAdd.Invoke(d.value);
						break;
					case SetOperation.REMOVED:
						_onRemove.Invoke(d.value);
						break;
					case SetOperation.CHANGED:
						_onChange.Invoke(d.value);
						break;
				}
			};
		}
	}
	/// <summary>
	/// Write access to EquipmentPanelDataSet or List<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData>, with built-in UnityEvent.
	/// For read-only access make a EquipmentPanelDataSetRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquipmentPanelDataSetWriter : SmartSetRefWriterBase<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataSet>, ISmartSetRefReader<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> {
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onAdd = null;
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onRemove = null;
		[SerializeField]
		Data.EquipmentPanelDataVar.EquipmentPanelDataEvent _onChange = null;
		
		protected override System.Action<SetEventData<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData>> GetUnityEventInvoke(){
			return InvokeUnityEvent;
		}
		
		protected sealed override void InvokeUnityEvent(SetEventData<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData> d){
			switch (d.operation){
				case SetOperation.ADDED:
					_onAdd.Invoke(d.value);
					break;
				case SetOperation.REMOVED:
					_onRemove.Invoke(d.value);
					break;
				case SetOperation.CHANGED:
					_onChange.Invoke(d.value);
					break;
			}
		}
		
	}
}