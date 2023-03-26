using BlockAttack.Publisher.Contracts;
using BlockAttack.Publisher.Exceptions;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace BlockAttack.Publisher
{
	internal class SimpleLambdaPublisher : ILambdaPublisher
	{
		public void Publish(string functionName, string location)
		{
			try
			{
				string workPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				var scriptfile = $"{workPath}\\scripts\\publish_function.ps1";
				var sessionState = InitialSessionState.CreateDefault();
				sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;
				PowerShell psinstance = PowerShell.Create(sessionState);
				psinstance.AddCommand(scriptfile)
				.AddArgument(functionName)
				.AddArgument(location);

				Console.WriteLine($"Publish {functionName}...");
				var output = psinstance.Invoke();
				if (output is not null)
				{
					foreach (var obj in output)
						Console.WriteLine(obj?.ToString());
				}
			}
			catch(Exception ex)
			{
				throw new BlockAttackLambdaPublishException(functionName, "Error occurred while function publish", ex);
			}
		}
	}
}