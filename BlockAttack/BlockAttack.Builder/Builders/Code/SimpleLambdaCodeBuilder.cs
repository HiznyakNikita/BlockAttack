using BlockAttack.Builder.Contract.Builders.Code;
using BlockAttack.Builder.Contract.Models;
using BlockAttack.Builder.Contract.Models.Code;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace BlockAttack.Builder.Builders.Code
{
	internal class SimpleLambdaCodeBuilder : ILambdaCodeBuilder
	{
		public string Build(LambdaMetadata lambdaMetadata, LambdaCodeMetadata lambdaCodeMetadata)
		{
			var main = CompilationUnit()
				.WithUsings(
				SingletonList(
					UsingDirective(
						QualifiedName(
							QualifiedName(
								IdentifierName("Amazon"),
								IdentifierName("Lambda")),
							IdentifierName("Core")))))
				.WithAttributeLists(
				SingletonList(
					AttributeList(
						SingletonSeparatedList(
							Attribute(
								IdentifierName("LambdaSerializer"))
							.WithArgumentList(
								AttributeArgumentList(
									SingletonSeparatedList(
										AttributeArgument(
											TypeOfExpression(
												QualifiedName(
													QualifiedName(
														QualifiedName(
															QualifiedName(
																IdentifierName("Amazon"),
																IdentifierName("Lambda")),
															IdentifierName("Serialization")),
														IdentifierName("SystemTextJson")),
													IdentifierName("DefaultLambdaJsonSerializer")))))))))
					.WithTarget(
						AttributeTargetSpecifier(
							Token(SyntaxKind.AssemblyKeyword)))));

			var @namespace = FileScopedNamespaceDeclaration(
				IdentifierName(lambdaMetadata.FunctionName));

			var functionClass = ClassDeclaration("Function")
				.WithModifiers(
					TokenList(
						Token(SyntaxKind.PublicKeyword)));

			var handlerMethod = MethodDeclaration(
							PredefinedType(
								Token(SyntaxKind.StringKeyword)),
							Identifier("FunctionHandler"))
						.WithModifiers(
							TokenList(
								Token(SyntaxKind.PublicKeyword)))
						.WithParameterList(
							ParameterList(
								SeparatedList<ParameterSyntax>(
									new SyntaxNodeOrToken[]{
										Parameter(
											Identifier("input"))
										.WithType(
											PredefinedType(
												Token(SyntaxKind.StringKeyword))),
										Token(SyntaxKind.CommaToken),
										Parameter(
											Identifier("context"))
										.WithType(
											IdentifierName("ILambdaContext"))})));

			var syntaxBlocks = new Dictionary<int, IfStatementSyntax>();
			IfStatementSyntax generalIf = null;
			foreach (var blockGroup in lambdaCodeMetadata.BlockGroups)
			{
				var blocks = blockGroup.OrderedById
					? blockGroup.Blocks.OrderBy(b => b.Key).Select(b => b).ToList()
					: blockGroup.Blocks.ToList();

				foreach (var block in blocks)
				{
					ExpressionSyntax condition = null;
					StatementSyntax statement = null;

					SyntaxKind conditionSyntaxKind = block.Value.Condition.Expression switch
					{
						ExpressionType.Equals => SyntaxKind.EqualsExpression,
						ExpressionType.Greater => SyntaxKind.GreaterThanExpression,
						ExpressionType.GreaterOrEquals => SyntaxKind.GreaterThanOrEqualExpression,
						ExpressionType.Less => SyntaxKind.LessThanExpression,
						ExpressionType.LessOrEquals => SyntaxKind.LessThanOrEqualExpression,
						_ => throw new NotImplementedException()
					};

					ExpressionSyntax conditionLiteral1 = block.Value.Condition.Literal1.Type switch
					{
						LiteralType.Numeric => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(block.Value.Condition.Literal1.Value)),
						LiteralType.String => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(block.Value.Condition.Literal1.Value)),
						LiteralType.Identifier => IdentifierName(block.Value.Condition.Literal1.Value),
						_ => throw new NotImplementedException()
					};

					ExpressionSyntax conditionLiteral2 = block.Value.Condition.Literal2.Type switch
					{
						LiteralType.Numeric => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(block.Value.Condition.Literal2.Value)),
						LiteralType.String => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(block.Value.Condition.Literal2.Value)),
						LiteralType.Identifier => IdentifierName(block.Value.Condition.Literal2.Value),
						_ => throw new NotImplementedException()
					};

					condition = BinaryExpression(conditionSyntaxKind, conditionLiteral1, conditionLiteral2);

					SyntaxKind statementSyntaxKind = block.Value.Handler.Expression switch
					{
						ExpressionType.Add => SyntaxKind.AddExpression,
						_ => throw new NotImplementedException()
					};

					ExpressionSyntax statementLiteral1 = block.Value.Handler.Literal1.Type switch
					{
						LiteralType.Numeric => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(block.Value.Handler.Literal1.Value)),
						LiteralType.String => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(block.Value.Handler.Literal1.Value)),
						LiteralType.Identifier => IdentifierName(block.Value.Handler.Literal1.Value),
						_ => throw new NotImplementedException()
					};

					ExpressionSyntax statementLiteral2 = block.Value.Handler.Literal2.Type switch
					{
						LiteralType.Numeric => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(block.Value.Handler.Literal2.Value)),
						LiteralType.String => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(block.Value.Handler.Literal2.Value)),
						LiteralType.Identifier => IdentifierName(block.Value.Handler.Literal2.Value),
						_ => throw new NotImplementedException()
					};

					statement = ReturnStatement(
						BinaryExpression(statementSyntaxKind, statementLiteral1, statementLiteral2));

					var @if = IfStatement(condition, statement);
					syntaxBlocks.Add(block.Key, @if);
				}

				var firstBlock = syntaxBlocks.FirstOrDefault();
				var lastBlock = syntaxBlocks.Last();

				IfStatementSyntax previousBlock = null;
				foreach (var ifBlock in syntaxBlocks)
				{
					if (ifBlock.Key == firstBlock.Key)
						generalIf = ifBlock.Value;
					else if (ifBlock.Key != lastBlock.Key)
						generalIf = generalIf.WithElse(ElseClause(ifBlock.Value));
					else if (ifBlock.Key == lastBlock.Key - 1)
						continue;
					else
					{
						previousBlock = previousBlock.WithElse(ElseClause(ifBlock.Value.Statement));
						generalIf = generalIf.WithElse(ElseClause(previousBlock));
					}
					previousBlock = ifBlock.Value;
				}
			}
			var handlerMethodBody = Block(generalIf);
			handlerMethod = handlerMethod.WithBody(handlerMethodBody);
			functionClass = functionClass.WithMembers(SingletonList<MemberDeclarationSyntax>(handlerMethod));
			@namespace = @namespace.WithMembers(SingletonList<MemberDeclarationSyntax>(functionClass));
			main = main.WithMembers(SingletonList<MemberDeclarationSyntax>(@namespace));

			var code = main.NormalizeWhitespace().ToFullString();

			return code;
		}
	}
}
