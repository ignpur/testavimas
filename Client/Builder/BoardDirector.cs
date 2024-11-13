using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Builder
{
	public class BoardDirector
	{
		private readonly IBoardBuilder _builder;


		public BoardDirector(IBoardBuilder builder)
		{
			_builder = builder;
		}

		public Board BuildStandardBoard()
		{
			return _builder
				.SetDimensions(10, 10)
				.AddColumnLabels(new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" })
				.AddRowLabels(new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" })
				.Build();
		}

		public Board BuildLargeBoard()
		{
			return _builder
				.SetDimensions(15, 15)
				.AddColumnLabels(new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O" })
				.AddRowLabels(new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" })
				.Build();
		}

		public Board BuildExtraLargeBoard()
		{
			return _builder
				.SetDimensions(20, 20)
				.AddColumnLabels(new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T" })
				.AddRowLabels(new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" })
				.Build();
		}

		// servs turi atsiust nuo 1 iki 3. tada pagal tai koki atsiuncia toki buildina
		public Board BuildRandomBoard(int choice)
		{
			return choice switch
			{
				1 => BuildStandardBoard(),
				2 => BuildLargeBoard(),
				3 => BuildExtraLargeBoard(),
				_ => BuildStandardBoard() // Default to standard in case of any issue
			};
		}
	}
}
