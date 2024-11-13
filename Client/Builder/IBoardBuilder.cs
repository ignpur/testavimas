using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Builder
{
	public interface IBoardBuilder
	{
		IBoardBuilder SetDimensions(int width, int height);
		IBoardBuilder AddColumnLabels(List<string> labels);
		IBoardBuilder AddRowLabels(List<string> labels);
		Board Build();
	}
}
