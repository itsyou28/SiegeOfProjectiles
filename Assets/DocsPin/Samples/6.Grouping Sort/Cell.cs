using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
	public UnityEngine.UI.Text _indexText = null;
	public DocsPin.UI.uGUIText _nameText = null;
	public DocsPin.UI.uGUIText _priceText = null;
	public DocsPin.UI.uGUIText _limitText = null;

	// Use this for initialization
	void Start()
	{}
	
	// Update is called once per frame
	void Update()
	{}

	public void setValue(DocsPin.DocsData data, int index, string rowId)
	{
		if(data == null || string.IsNullOrEmpty(rowId) == true)
			return;
		this._indexText.text = index.ToString();
		// Setting name.
		this.updateValue(this._nameText, rowId);
		// Setting price.
		this.updateValue(this._priceText, rowId);
		// Setting limit.
		this.updateValue(this._limitText, rowId);
	}
	private void updateValue(DocsPin.UI.uGUIText text, string rowId)
	{
		if(text == null)
			return;
		text.rowId = rowId;
		text.updateValue();
	}
}
