// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquippedPanelData
// SMARTTEMPLATE WriteSmartSetTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartEquippedPanelData.Components {
	/// <summary>
	/// Serialised write access to a SmartEquippedPanelDataSet.
	/// </summary>
	[AddComponentMenu("SmartData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData/Write Smart WalkingSimFramework.Scriptable_Objects.EquippedPanelData Set", 3)]
	public class WriteSmartEquippedPanelDataSet : WriteSetBase<WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataSetWriter> {}
}