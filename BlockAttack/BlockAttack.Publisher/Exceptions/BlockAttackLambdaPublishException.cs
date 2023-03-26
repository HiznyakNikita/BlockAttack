using System.Runtime.Serialization;

namespace BlockAttack.Publisher.Exceptions
{
	public class BlockAttackLambdaPublishException : Exception
	{
		public string FunctionName { get; init; }

		public BlockAttackLambdaPublishException(string functionName) : base()
		{
			FunctionName = functionName;
		}

		public BlockAttackLambdaPublishException(string functionName, string message) : base(message)
		{
			FunctionName = functionName;
		}

		public BlockAttackLambdaPublishException(string functionName, string message, Exception innerException) : base(message, innerException)
		{
			FunctionName = functionName;
		}

		protected BlockAttackLambdaPublishException(string functionName, SerializationInfo info, StreamingContext context) : base(info, context)
		{
			FunctionName = functionName;
		}
	}
}
