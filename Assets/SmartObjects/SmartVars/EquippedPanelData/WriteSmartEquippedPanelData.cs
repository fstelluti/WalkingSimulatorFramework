// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquippedPanelData
// SMARTTEMPLATE WriteSmartVarTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartEquippedPanelData.Components {
	/// <summary>
	/// Serialised write access to a SmartEquippedPanelData.
	/// </summary>
	[AddComponentMenu("SmartData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData/Write Smart WalkingSimFramework.Scriptable_Objects.EquippedPanelData", 1)]
	public class WriteSmartEquippedPanelData : WriteSmartBase<WalkingSimFramework.Scriptable_Objects.EquippedPanelData, EquippedPanelDataWriter> {}
}