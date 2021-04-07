namespace DutchTreat.Services
{
	public interface IMailService
	{
		void SendMassage(string to, string subject, string body);
	}
}