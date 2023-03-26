namespace BlockAttack.Builder.Contract.Models
{
	public class LambdaMetadata
	{
		public string[] Information { get; set; }
		public string Profile { get; set; }
		public string Region { get; set; }
		public string Configuration { get; set; }
		public string FunctionArchitecture { get; set; }
		public string FunctionRuntime { get; set; }
		public int? FunctionMemorySize { get; set; }
		public int? FunctionTimeout { get; set; }
		public string FunctionRole { get; set; }
		public string FunctionName { get; set; }
		public string Location { get; set; }
	}
}
