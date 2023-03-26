using Newtonsoft.Json;

namespace BlockAttack.Builder.Models
{
	internal class AwsLambdaToolsDefaults
	{
		[JsonProperty("Information")]
		public string[] Information { get; set; } = new[] {
			"This file provides default values for the deployment wizard inside Visual Studio and the AWS Lambda commands added to the .NET Core CLI.",
			"To learn more about the Lambda commands with the .NET Core CLI execute the following command at the command line in the project root directory.",
			"dotnet lambda help",
			"All the command line options for the Lambda command can be specified in this file."
		};
		
		[JsonProperty("profile")]
		public string Profile { get; set; } = "default"; 

		[JsonProperty("region")]
		public string Region { get; set; } = "us-east-1";
		
		[JsonProperty("configuration")]
		public string Configuration { get; set; } = "Release";

		[JsonProperty("function-architecture")]
		public string FunctionArchitecture { get; set; } = "x86_64";

		[JsonProperty("function-runtime")]
		public string FunctionRuntime { get; set; } = "dotnet6";

		[JsonProperty("function-memory-size")]
		public int FunctionMemorySize { get; set; } = 256;
		
		[JsonProperty("function-timeout")]
		public int FunctionTimeout { get; set; } = 30;
		
		[JsonProperty("function-handler")]
		public string FunctionHandler => $"{FunctionName}::{FunctionName}.Function::FunctionHandler";

		[JsonProperty("function-role")]
		public string FunctionRole { get; set; } = "basicExecutionRole";

		[JsonIgnore]
		public string FunctionName { get; set; }
	}
}
