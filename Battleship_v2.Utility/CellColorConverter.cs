using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Battleship_v2.Utility;

public class CellColorConverter : IMultiValueConverter
{
	object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values[0] is DataGridCell dataGridCell && values[1] is DataRow row)
		{
			try
			{
				string columnName = (string)dataGridCell.Column.Header;
				if (dataGridCell.Column.DisplayIndex == 0)
				{
					dataGridCell.FontStyle = FontStyles.Italic;
					dataGridCell.FontWeight = FontWeights.Bold;
					dataGridCell.FontSize = 20.0;
					return new SolidColorBrush(Colors.Yellow);
				}
				switch (row.Field<string>(columnName))
				{
				case "w":
					return new SolidColorBrush(Colors.Blue);
				case "h":
					return new SolidColorBrush(Colors.Red);
				case "m":
					return new SolidColorBrush(Colors.LightGray);
				case "c":
				case "b":
				case "s":
				case "d":
				case "p":
					return new SolidColorBrush(Colors.Gray);
				}
			}
			catch (Exception)
			{
				return new SolidColorBrush(Colors.Black);
			}
		}
		return new SolidColorBrush(Colors.Green);
	}

	object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
