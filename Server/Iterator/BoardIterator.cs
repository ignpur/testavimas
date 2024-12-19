using System;

namespace Server.Iterator
{
	public class BoardIterator : IIterator<(int x, int y)>
	{
		private readonly int _startX, _startY;
		private readonly int _size;
		private readonly bool _isVertical;
		private int _currentStep;

		public BoardIterator(int startX, int startY, int size, bool isVertical)
		{
			_startX = startX;
			_startY = startY;
			_size = size;
			_isVertical = isVertical;
			_currentStep = 0;
		}

		public bool HasNext()
		{
			if (_isVertical)
				return _startY + _currentStep < _startY + _size;
			else
				return _startX + _currentStep < _startX + _size;
		}

		public (int x, int y) Next()
		{
			if (!HasNext()) throw new InvalidOperationException("No more elements to iterate.");

			var position = _isVertical
				? (_startX, _startY + _currentStep)
				: (_startX + _currentStep, _startY);

			_currentStep++;
			return position;
		}
	}
}
