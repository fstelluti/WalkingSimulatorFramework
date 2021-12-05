// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquipmentPanelData
// SMARTTEMPLATE WriteSmartVarTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartEquipmentPanelData.Components {
	/// <summary>
	/// Serialised write access to a SmartEquipmentPanelData.
	/// </summary>
	[AddComponentMenu("SmartData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData/Write Smart WalkingSimFramework.Scriptable_Objects.EquipmentPanelData", 1)]
	public class WriteSmartEquipmentPanelData : WriteSmartBase<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData, EquipmentPanelDataWriter> {}
}