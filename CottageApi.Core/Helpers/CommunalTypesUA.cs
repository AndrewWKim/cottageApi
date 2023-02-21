using System.Collections.Generic;

namespace CottageApi.Core.Helpers
{
	public static class CommunalTypesUA
	{
		private static readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>
			{
				{ "ok", "Компенсація витрат обслуговуючого кооперативу" },
				{ "water", "Компенсація водопостачання" },
				{ "sewerage", "Компенсація каналізації" },
				{ "electricity", "Компенсація електропостачання" },
			};

		public static string GetCommunalType(string type)
		{
			if (_dictionary.TryGetValue(type, out string result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}
	}
}
