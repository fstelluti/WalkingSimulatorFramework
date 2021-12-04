// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquippedPanelData
// SMARTTEMPLATE SmartSetTemplate
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
	/// ScriptableObject data set which fires a Relay on data addition/removal.
	/// <summary>
	[CreateAssetMenu(menuName="SmartData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData Set", order=2)]
	public class EquippedPanelDataSet : SmartSet<WalkingSimFramework.Scriptable_Objects.EquippedPanelData>, ISmartDataSet<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> {
		#if UNITY_EDITOR
		const string VALUETYPE = "WalkingSimFramework.Scriptable_Objects.EquippedPanelData";
		const string DISPLAYTYPE = "WalkingSimFramework.Scriptable_Objects.EquippedPanelData Set";
		#endif
	}
}

namespace SmartData.SmartEquippedPanelData {
	/// <summary>
	/// Read-only access to EquippedPanelDataSet or List<0>, with built-in UnityEvent.
	/// For write access make a EquippedPanelDataSetWriter reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataSetReader : SmartSetRefBase<WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataSet>, ISmartSetRefReader<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> {
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onAdd = null;
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onRemove = null;
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onChange = null;
		
		protected override System.Action<SetEventData<WalkingSimFramework.Scriptable_Objects.EquippedPanelData>> GetUnityEventInvoke(){
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
	/// Write access to EquippedPanelDataSet or List<WalkingSimFramework.Scriptable_Objects.EquippedPanelData>, with built-in UnityEvent.
	/// For read-only access make a EquippedPanelDataSetRef reference.
	/// UnityEvent disabled by default. If enabled, remember to disable at end of life.
	/// </summary>
	[System.Serializable]
	public class EquippedPanelDataSetWriter : SmartSetRefWriterBase<WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataSet>, ISmartSetRefReader<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> {
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onAdd = null;
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onRemove = null;
		[SerializeField]
		Data.EquippedPanelDataVar.EquippedPanelDataEvent _onChange = null;
		
		protected override System.Action<SetEventData<WalkingSimFramework.Scriptable_Objects.EquippedPanelData>> GetUnityEventInvoke(){
			return InvokeUnityEvent;
		}
		
		protected sealed override void InvokeUnityEvent(SetEventData<WalkingSimFramework.Scriptable_Objects.EquippedPanelData> d){
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