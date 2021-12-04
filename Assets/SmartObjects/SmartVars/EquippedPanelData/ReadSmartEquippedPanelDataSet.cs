// SMARTTYPE WalkingSimFramework.Scriptable_Objects.EquippedPanelData
// SMARTTEMPLATE ReadSmartSetTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartEquippedPanelData.Components {
	/// <summary>
	/// Automatically listens to a <cref>SmartEquippedPanelDataSet</cref> and fires a <cref>UnityEvent<WalkingSimFramework.Scriptable_Objects.EquippedPanelData></cref> when data changes.
	/// </summary>
	[AddComponentMenu("SmartData/WalkingSimFramework.Scriptable_Objects.EquippedPanelData/Read Smart WalkingSimFramework.Scriptable_Objects.EquippedPanelData Set", 2)]
	public class ReadSmartEquippedPanelDataSet : ReadSmartBase<EquippedPanelDataSetReader> {}
}