using BlockAttack.Builder;
using BlockAttack.Builder.Contract.Builders;
using BlockAttack.Builder.Contract.Models;
using BlockAttack.Builder.Contract.Models.Code;
using BlockAttack.Publisher;
using BlockAttack.Publisher.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

var serviceProvider = new ServiceCollection()
			.AddLambdaBuilders()
			.AddLambdaPublishers()
			.BuildServiceProvider();

var builder = serviceProvider
	.GetService<ILambdaBuilder>();

var publisher = serviceProvider
	.GetService<ILambdaPublisher>();

var configBuilder = new ConfigurationBuilder()
	.AddJsonFile($"appsettings.json", false, true)
	.AddJsonFile($"appsettings.dev.json", true, true);
var config = configBuilder.Build();

var functionName = config["functionName"];
var location = config["functionLocation"];
var functionCodeMetadataJsonPath = config["functionCodeMetadataJsonPath"];

var codeMetadataJson = File.ReadAllText(functionCodeMetadataJsonPath);
var lambdaCodeMetadata = JsonConvert.DeserializeObject<LambdaCodeMetadata>(codeMetadataJson);
var lambdaMetadata = new LambdaMetadata()
{
	FunctionName = functionName,
	Location = location
};

builder.Build(lambdaMetadata, lambdaCodeMetadata);
publisher.Publish(functionName, location);
