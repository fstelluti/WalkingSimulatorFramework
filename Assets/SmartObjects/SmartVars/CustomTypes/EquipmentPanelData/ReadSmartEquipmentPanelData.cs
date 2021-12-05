// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquipmentPanelData
// SMARTTEMPLATE ReadSmartVarTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartEquipmentPanelData.Components {
	/// <summary>
	/// Automatically listens to a <cref>SmartEquipmentPanelData</cref> and fires a <cref>UnityEvent<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData></cref> when data changes.
	/// </summary>
	[AddComponentMenu("SmartData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData/Read Smart WalkingSimFramework.Scriptable_Objects.EquipmentPanelData", 0)]
	public class ReadSmartEquipmentPanelData : ReadSmartBase<EquipmentPanelDataReader> {}
}