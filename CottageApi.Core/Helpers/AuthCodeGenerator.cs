using System;

namespace CottageApi.Core.Helpers
{
	public static class AuthCodeGenerator
	{
        public static string GenerateNewCode(int minValue, int maxValue)
        {
            Random random = new Random();
            var code = random.Next(minValue, maxValue);
            return code.ToString("000000");
        }
    }
}
