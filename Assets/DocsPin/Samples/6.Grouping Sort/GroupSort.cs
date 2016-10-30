using UnityEngine;
using System.Collections;

public class GroupSort : MonoBehaviour
{
	public enum eGroup
	{
		WALL = 0,
		FLOOR,
		MOUSE,
		COMPUTER
	}
	public const string GROUP_COL_ID = "group";
	public const string ORDER_COL_ID = "order";

	// Cell and Sheet File.
	public GameObject _cells = null;
	public DocsPin.DocsData _docsFile = null;

	// Informations.
	public UnityEngine.UI.Text _rowCountText = null;
	public UnityEngine.UI.Text _groupRowCountText = null;

	// Order.
	public UnityEngine.UI.Toggle _descending = null;
	
	public eGroup _selectedGroup = eGroup.COMPUTER;

	// Use this for initialization
	void Start()
	{
		this.hideAllCells();

		// Show informations.
		this.showTotalRowCount();

		// Show cell by group.
		this.changeGroupCells(this._selectedGroup);
	}
	
	// Update is called once per frame
	void Update()
	{
	}

	#region Group button event.
	public void onComputerGroup()
	{
		this.hideAllCells();
		this.changeGroupCells(eGroup.COMPUTER);
	}
	public void onMouseGroup()
	{
		this.hideAllCells();
		this.changeGroupCells(eGroup.MOUSE);
	}
	public void onWallGroup()
	{
		this.hideAllCells();
		this.changeGroupCells(eGroup.WALL);
	}
	public void onFloorGroup()
	{
		this.hideAllCells();
		this.changeGroupCells(eGroup.FLOOR);
	}
	#endregion

	#region Order toggle event.
	public void onDesendingToggle()
	{
		if(this._descending == null)
			return;

		this.changeGroupCells(this._selectedGroup);
	}
	private bool isAscending()
	{
		if(this._descending == null)
			return true;
		return (this._descending.isOn == false);
	}
	#endregion

	#region Show Informations.
	private void showTotalRowCount()
	{
		if(this._docsFile == null || this._rowCountText == null)
			return;

		// Show total row counts.
		int rows = this._docsFile.getRowCount();
		this._rowCountText.text = rows.ToString();
	}
	private void showGroupRowCount()
	{
		if(this._docsFile == null || this._groupRowCountText == null)
			return;
		
		// Show total row counts.
		int rows = this._docsFile.getRowCount(GROUP_COL_ID, (byte)this._selectedGroup);
		this._groupRowCountText.text = rows.ToString();
	}
	#endregion

	#region Cell Inteface.
	private void changeGroupCells(eGroup group)
	{
		this._selectedGroup = group;

		// Update group count;
		this.showGroupRowCount();

		// Show group cells.
		ArrayList groups = this.getGroupRowKeys(this._selectedGroup);
		this.showCells(groups);
	}
	// Show cells from row id list.
	private void showCells(ArrayList rowIds)
	{
		if(this._cells == null || rowIds == null || rowIds.Count <= 0)
			return;

		for(int i=0; i<rowIds.Count; i++)
		{
			Cell cell = this.getCellByName(i.ToString());
			if(cell == null)
				continue;
			// Show cell.
			cell.gameObject.SetActive(true);
			cell.setValue(this._docsFile, i, (string)rowIds[i]);
		}
	}
	// Hide all cells.
	private void hideAllCells()
	{
		if(this._cells == null)
			return;
		foreach(Transform child in this._cells.transform)
		{
			child.gameObject.SetActive(false);
		}
	}
	private Cell getCellByName(string name)
	{
		foreach(Transform child in this._cells.transform)
		{
			Cell cell = child.gameObject.GetComponent<Cell>();
			if(cell == null)
				continue;
			if(cell.name == name)
				return cell;
		}
		return null;
	}
	#endregion

	// Getting row keys from group.
	private ArrayList getGroupRowKeys(eGroup group)
	{
		if(this._docsFile == null)
			return null;

		// #1. Only grouping.
		//return this._docsFile.getRowKeyList(GROUP_COL_ID, (byte)group);

		// #2. Grouping and order.
		bool asc = this.isAscending();
		return this._docsFile.getRowKeyListOrder(GROUP_COL_ID, (byte)group, ORDER_COL_ID, asc);
	}
}
