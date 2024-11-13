using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Builder
{
	public class BoardBuilder : IBoardBuilder
	{
		private int _width;
		private int _height;
		private List<string> _columnLabels = new List<string>();
		private List<string> _rowLabels = new List<string>();

		public IBoardBuilder SetDimensions(int width, int height)
		{
			_width = width;
			_height = height;
			return this;
		}

		public IBoardBuilder AddColumnLabels(List<string> labels)
		{
			_columnLabels = labels;
			return this;
		}

		public IBoardBuilder AddRowLabels(List<string> labels)
		{
			_rowLabels = labels;
			return this;
		}

		public Board Build()
		{
			return new Board(_width, _height, _columnLabels, _rowLabels);
		}
	}
}
