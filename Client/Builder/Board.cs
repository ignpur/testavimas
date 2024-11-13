using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Builder
{
	public class Board
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public List<string> ColumnLabels { get; private set; }
		public List<string> RowLabels { get; private set; }

		public Board(int width, int height, List<string> columnLabels, List<string> rowLabels)
		{
			Width = width;
			Height = height;
			ColumnLabels = columnLabels;
			RowLabels = rowLabels;
		}
	}
}
