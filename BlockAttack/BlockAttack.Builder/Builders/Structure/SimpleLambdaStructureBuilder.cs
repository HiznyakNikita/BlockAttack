using BlockAttack.Builder.Contract.Builders.Structure;
using BlockAttack.Builder.Contract.Models;
using BlockAttack.Builder.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace BlockAttack.Builder.Builders.Structure
{
	internal class SimpleLambdaStructureBuilder : ILambdaStructureBuilder
	{
		private const string _awsLambdaToolsDefaultsFileName = "aws-lambda-tools-defaults.json";
		private const string _csprojTemplateFileName = "Template.csproj";

		public void Build(LambdaMetadata lambdaMetadata, string lambdaCode)
		{
			if (string.IsNullOrEmpty(lambdaMetadata.FunctionName))
				throw new ArgumentException("Invalid function name");
			if (string.IsNullOrEmpty(lambdaMetadata.Location))
				throw new ArgumentException("Invalid function location");

			var rootFolder = $"{lambdaMetadata.Location}\\{lambdaMetadata.FunctionName}";
			var srcFolder = $"{rootFolder}\\src";
			var functionFolder = $"{srcFolder}\\{lambdaMetadata.FunctionName}";
			string workPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var templatesFolder = $"{workPath}\\templates\\Simple";

			if (Directory.Exists(rootFolder))
			{
				Console.WriteLine($"{rootFolder}");
				Directory.Delete(rootFolder, recursive: true);
			}

			if (!Directory.Exists(rootFolder))
			{
				Directory.CreateDirectory(rootFolder);
				Console.WriteLine($"Root function {lambdaMetadata.FunctionName} directory created Successfully");

				Directory.CreateDirectory(srcFolder);
				Console.WriteLine($"Src function {lambdaMetadata.FunctionName} directory created Successfully");

				Directory.CreateDirectory(functionFolder);
				Console.WriteLine($"Function function {lambdaMetadata.FunctionName} directory created Successfully");

				var awsLambdaToolsDefaultsJson = CreateAwsLambdaToolsDefaults(lambdaMetadata);
				WriteFile(functionFolder, _awsLambdaToolsDefaultsFileName, awsLambdaToolsDefaultsJson);
				Console.WriteLine($"aws-lambda-tools-defaults.json for function {lambdaMetadata.FunctionName} created successfully");

				File.Copy(Path.Combine(templatesFolder, _csprojTemplateFileName), Path.Combine(functionFolder, $"{lambdaMetadata.FunctionName}.csproj"), true);
				Console.WriteLine($"csproj for function {lambdaMetadata.FunctionName} copied successfully");

				WriteFile(functionFolder, "Function.cs", lambdaCode);
			}
		}

		private static string CreateAwsLambdaToolsDefaults(LambdaMetadata metadata)
		{
			var defaults = new AwsLambdaToolsDefaults();

			defaults.Information = metadata.Information ?? defaults.Information;
			defaults.Profile = metadata.Profile ?? defaults.Profile;
			defaults.Region = metadata.Region ?? defaults.Region;
			defaults.Configuration = metadata.Configuration ?? defaults.Configuration;
			defaults.FunctionArchitecture = metadata.FunctionArchitecture ?? defaults.FunctionArchitecture;
			defaults.FunctionRuntime = metadata.FunctionRuntime ?? defaults.FunctionRuntime;
			defaults.FunctionMemorySize = metadata.FunctionMemorySize ?? defaults.FunctionMemorySize;
			defaults.FunctionTimeout = metadata.FunctionTimeout ?? defaults.FunctionTimeout;
			defaults.FunctionRole = metadata.FunctionRole ?? defaults.FunctionRole;
			defaults.FunctionName = metadata.FunctionName;

			var defaultsJson = JsonConvert.SerializeObject(defaults);

			return defaultsJson;
		}

		private static void WriteFile(string folder, string fileName, string fileContent)
		{
			using (FileStream fs = File.Create(Path.Combine(folder, fileName)))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(fileContent);
				fs.Write(info, 0, info.Length);
			}
		}
	}
}
