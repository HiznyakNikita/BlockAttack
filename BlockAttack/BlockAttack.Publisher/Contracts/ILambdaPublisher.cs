namespace BlockAttack.Publisher.Contracts
{
	public interface ILambdaPublisher
	{
		void Publish(string functionName, string location);
	}
}
