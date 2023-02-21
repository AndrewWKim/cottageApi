namespace CottageApi.Core.Domain.Dto.Users
{
	public class CreateUserDto
	{
		public string RegistrationCode { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }
	}
}
