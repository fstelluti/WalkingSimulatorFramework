// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquipmentPanelData
// SMARTTEMPLATE WriteSmartSetTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartEquipmentPanelData.Components {
	/// <summary>
	/// Serialised write access to a SmartEquipmentPanelDataSet.
	/// </summary>
	[AddComponentMenu("SmartData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData/Write Smart WalkingSimFramework.Scriptable_Objects.EquipmentPanelData Set", 3)]
	public class WriteSmartEquipmentPanelDataSet : WriteSetBase<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataSetWriter> {}
}