// SMARTTYPE WalkingSimFramework.UI_System.HUD.EquippedPanelData
// SMARTTEMPLATE ReadSmartVarTemplate
// Do not move or delete the above lines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmartData.Abstract;

namespace SmartData.SmartEquippedPanelData.Components {
	/// <summary>
	/// Automatically listens to a <cref>SmartEquippedPanelData</cref> and fires a <cref>UnityEvent<WalkingSimFramework.UI_System.HUD.EquippedPanelData></cref> when data changes.
	/// </summary>
	[AddComponentMenu("SmartData/WalkingSimFramework.UI_System.HUD.EquippedPanelData/Read Smart WalkingSimFramework.UI_System.HUD.EquippedPanelData", 0)]
	public class ReadSmartEquippedPanelData : ReadSmartBase<EquippedPanelDataReader> {}
}