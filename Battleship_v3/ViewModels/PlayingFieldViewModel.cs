using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Ships;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels;

public sealed class PlayingFieldViewModel : BaseViewModel
{
	private GameManagerService m_GameManager;

	private DataGrid m_DataGrid;

	private DataTable m_Grid;

	private DataGridCellInfo m_CurrentCell;

	private ObservableCollection<Ship> m_Ships;

	private Brush m_BackgroundColor;

	public PlayingFieldModel Model { get; set; }

	public PlayerType Owner { get; }

	public DataGrid DataGrid
	{
		get
		{
			return m_DataGrid;
		}
		set
		{
			m_DataGrid = value;
			NotifyPropertyChanged("Model");
		}
	}

	public DataTable Grid
	{
		get
		{
			return m_Grid;
		}
		set
		{
			m_Grid = value;
			NotifyPropertyChanged("Grid");
		}
	}

	public DataGridCellInfo CurrentCell
	{
		get
		{
			return m_CurrentCell;
		}
		set
		{
			if (Owner != 0)
			{
				m_CurrentCell = value;
				if (m_CurrentCell.Column != null && m_CurrentCell.Item != null)
				{
					Model.GridCellClicked(m_CurrentCell.Column.DisplayIndex + 1, DataGrid.Items.IndexOf((DataRowView)m_CurrentCell.Item) + 1);
				}
			}
		}
	}

	public ObservableCollection<Ship> Ships
	{
		get
		{
			return m_Ships;
		}
		set
		{
			m_Ships = value;
			NotifyPropertyChanged("Ships");
		}
	}

	public Brush BackgroundColor
	{
		get
		{
			return m_BackgroundColor;
		}
		set
		{
			m_BackgroundColor = value;
			NotifyPropertyChanged("BackgroundColor");
		}
	}

	public PlayingFieldViewModel(PlayerType theOwner, List<ushort> theShipList)
	{
		Owner = theOwner;
		DataGrid = createDataGrid();
		Grid = createDataTable();
		DataGrid.BeginInit();
		DataGrid.ItemsSource = Grid.DefaultView;
		DataGrid.EndInit();
		Model = new PlayingFieldModel(this, theOwner == PlayerType.You, theShipList);
		BackgroundColor = new SolidColorBrush(Color.FromRgb(byte.MaxValue, 0, 0));
	}

	private DataGrid createDataGrid()
	{
		DataGridLength columnWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
		double num = 50.0;
		DataGrid dataGrid = new DataGrid();
		dataGrid.AutoGenerateColumns = false;
		dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
		dataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
		dataGrid.SelectionMode = DataGridSelectionMode.Single;
		dataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
		dataGrid.BorderThickness = new Thickness(4.0);
		dataGrid.SetBinding(Control.BorderBrushProperty, new Binding("BackgroundColor"));
		Binding binding = new Binding("CurrentCell");
		binding.Source = this;
		binding.Mode = BindingMode.OneWayToSource;
		binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
		BindingOperations.SetBinding(dataGrid, DataGrid.CurrentCellProperty, binding);
		dataGrid.HeadersVisibility = DataGridHeadersVisibility.All;
		dataGrid.RowHeaderWidth = num;
		dataGrid.ColumnHeaderHeight = num;
		dataGrid.CanUserAddRows = false;
		dataGrid.CanUserDeleteRows = false;
		dataGrid.CanUserResizeRows = false;
		dataGrid.RowHeight = columnWidth.DesiredValue;
		dataGrid.ColumnWidth = columnWidth;
		dataGrid.CanUserReorderColumns = false;
		dataGrid.CanUserResizeColumns = false;
		dataGrid.LoadingRow += DataGrid_LoadingRow;
		for (char c = 'A'; c <= 'J'; c = (char)(c + 1))
		{
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Image));
			frameworkElementFactory.SetBinding(Image.SourceProperty, new Binding($"{c}"));
			DataGridColumn item = new DataGridTemplateColumn
			{
				CellTemplate = new DataTemplate
				{
					VisualTree = frameworkElementFactory
				},
				Header = c
			};
			dataGrid.Columns.Add(item);
		}
		return dataGrid;
	}

	private static DataTable createDataTable()
	{
		DataTable dataTable = new DataTable();
		for (char c = 'A'; c <= 'J'; c = (char)(c + 1))
		{
			DataColumn dataColumn = new DataColumn($"{c}", typeof(byte[]));
			dataColumn.DefaultValue = TileService.GetTile(TileType.Water);
			dataTable.Columns.Add(dataColumn);
		}
		for (int i = 0; i < 10; i++)
		{
			DataRow row = dataTable.NewRow();
			dataTable.Rows.Add(row);
		}
		return dataTable;
	}

	public void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
	{
		e.Row.Header = (e.Row.GetIndex() + 1).ToString();
	}
}
