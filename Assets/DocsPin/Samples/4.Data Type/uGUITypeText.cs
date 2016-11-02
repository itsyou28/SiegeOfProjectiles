using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class uGUITypeText : DocsPin.UI.uGUIText
{
	protected override void setValue(object value)
	{
		UnityEngine.UI.Text text = this.gameObject.GetComponent<UnityEngine.UI.Text>();
		if(text == null)
			return;
		if(value == null)
			text.text = "";
		else
			text.text = string.Format("Row[{0}]-Col[{1}]-({2}) : {3}",
			                          this.rowId, this.colId, value.GetType(), value);
	}
}
