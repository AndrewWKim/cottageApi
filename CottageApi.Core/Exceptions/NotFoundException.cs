namespace CottageApi.Core.Exceptions
{
	public class NotFoundException : BusinessException
	{
		public NotFoundException(string message = null)
            : base(message == null ? message : "Cottage not found.")
		{
		}
	}
}
