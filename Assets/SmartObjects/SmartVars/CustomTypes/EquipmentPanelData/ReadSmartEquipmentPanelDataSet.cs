// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquipmentPanelData
// SMARTTEMPLATE ReadSmartSetTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartEquipmentPanelData.Components {
	/// <summary>
	/// Automatically listens to a <cref>SmartEquipmentPanelDataSet</cref> and fires a <cref>UnityEvent<WalkingSimFramework.Scriptable_Objects.EquipmentPanelData></cref> when data changes.
	/// </summary>
	[AddComponentMenu("SmartData/WalkingSimFramework.Scriptable_Objects.EquipmentPanelData/Read Smart WalkingSimFramework.Scriptable_Objects.EquipmentPanelData Set", 2)]
	public class ReadSmartEquipmentPanelDataSet : ReadSmartBase<EquipmentPanelDataSetReader> {}
}