using System;

namespace BernardeSaturdayCakes
{
	public class HtmlChangedException : Exception
	{
		public string Origin { get; set; }

		public HtmlChangedException(string origin)
		{
			Origin = origin;
		}
	}
}
