using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PurpleRock.Application.Common;
using PurpleRock.Application.Common.Configuration;
using Swashbuckle.AspNetCore.Filters;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PurpleRock.WebApi.Common.Swagger;

/// <summary>
/// Generate code on build to create Swagger Examples for classes that implement <see cref="IExampleProvider{T}"/>.
/// </summary>
public static class SwaggerExamplesGenerator
{
  private static Type ExampleProviderType { get; } = typeof(IExampleProvider<object>);

  /// <summary>
  /// Generate Assembly with swagger examples built in.
  /// </summary>
  /// <param name="appOptions">Application options are used to pull out the Application Name.</param>
  /// <param name="assemblies">The list of assemblies to search for implementations of <see cref="IExampleProvider{T}"/>.</param>
  /// <returns></returns>
  public static Assembly Generate(AppOptions appOptions, Assembly[] assemblies)
  {
    var appName = appOptions.ApplicationName.Split('.')[1];

    var exampleProviderTypes = GetExampleProviderTypes(assemblies);

    var syntaxFactory = BuildSyntax(appName, exampleProviderTypes);

    var syntaxTree = CSharpSyntaxTree.Create(syntaxFactory.NormalizeWhitespace());

    return GenerateAssembly(syntaxTree, GetDllName(appName));
  }

  private static string GetDllName(string appName)
  {
    return $"PurpleRock.{appName}.SwaggerExamples.dll";
  }

  private static Type[] GetExampleProviderTypes(Assembly[] assemblies)
  {
    return assemblies
        .SelectMany(s => s.GetTypes())
        .Where(p => ExampleProviderType.IsAssignableFrom(p))
        .ToArray();
  }

  private static CompilationUnitSyntax BuildSyntax(
      string appName,
      Type[] exampleProviderTypes)
  {
    BaseListSyntax BuildInterface(Type type)
    {
      return BaseList(
          SingletonSeparatedList<BaseTypeSyntax>(
              SimpleBaseType(
                  GenericName(Identifier(nameof(IExamplesProvider<object>)))
                      .WithTypeArgumentList(
                          TypeArgumentList(
                              SingletonSeparatedList<TypeSyntax>(IdentifierName(type.Name)))))));
    }

    SyntaxList<MemberDeclarationSyntax> BuildMethodNameAndParameters(Type type)
    {
      return SingletonList<MemberDeclarationSyntax>(
          MethodDeclaration(IdentifierName(type.Name), Identifier(nameof(IExamplesProvider<object>.GetExamples)))
              .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
              .WithBody(BuildReturn(type)));
    }

    BlockSyntax BuildReturn(Type type)
    {
      return Block(
          SingletonList<StatementSyntax>(ReturnStatement(
              InvocationExpression(
                  MemberAccessExpression(
                      SyntaxKind.SimpleMemberAccessExpression,
                      ObjectCreationExpression(IdentifierName(type.Name)).WithArgumentList(ArgumentList()),
                      IdentifierName(nameof(IExampleProvider<object>.Generate)))))));
    }

    var classUsings = BuildClassUsings(exampleProviderTypes);

    var syntaxFactory = CompilationUnit()
        .WithUsings(List(classUsings))
        .WithMembers(BuildClassNamespace(appName));

    var swaggerExampleClassesSyntax = exampleProviderTypes
        .Select(type =>
            SingletonList<MemberDeclarationSyntax>(
                ClassDeclaration(type.Name + "Example")
                    .WithModifiers(TokenList(Token(SyntaxKind.InternalKeyword)))
                    .WithBaseList(BuildInterface(type))
                    .WithMembers(BuildMethodNameAndParameters(type)))[0]).ToArray();

    return syntaxFactory.AddMembers(swaggerExampleClassesSyntax);
  }

  private static UsingDirectiveSyntax[] BuildClassUsings(Type[] exampleProviderTypes)
  {
    IEnumerable<string> DistinctTypeNamespaces(Type[] types)
    {
      return types
      .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
      .Select(type => type.Namespace!)
      .Distinct();
    }

    UsingDirectiveSyntax UsingDirectiveFromUsingString(string usingDirectiveString)
    {
      return UsingDirective(BuildQualifiedNameSyntaxFromString(usingDirectiveString));
    }

    var usings = new List<UsingDirectiveSyntax>
        {
            UsingDirectiveFromUsingString("Swashbuckle.AspNetCore.Filters")
        };

    usings.AddRange(DistinctTypeNamespaces(exampleProviderTypes)
        .Select(UsingDirectiveFromUsingString));

    return usings.ToArray();
  }

  private static QualifiedNameSyntax BuildQualifiedNameSyntaxFromString(string usingString)
  {
    var split = usingString.Split('.');
    var count = split.Length;

    var firstName = IdentifierName(split[0]);
    QualifiedNameSyntax? nameSyntax = null;

    for (var i = 1; i < count; i++)
    {
      nameSyntax = nameSyntax == null
          ? QualifiedName(firstName, IdentifierName(split[i]))
          : QualifiedName(nameSyntax, IdentifierName(split[i]));
    }

    return nameSyntax!;
  }

  private static SyntaxList<MemberDeclarationSyntax> BuildClassNamespace(string appName)
  {
    var nameSpaceString = "PurpleRock." + appName + ".WebApi.SwaggerExamples";

    var namespaceNameSyntax = BuildQualifiedNameSyntaxFromString(nameSpaceString);

    return SingletonList<MemberDeclarationSyntax>(FileScopedNamespaceDeclaration(namespaceNameSyntax)
        .WithNamespaceKeyword(Token(TriviaList(), SyntaxKind.NamespaceKeyword, TriviaList(Space)))
        .WithSemicolonToken(Token(TriviaList(), SyntaxKind.SemicolonToken, TriviaList(LineFeed))));
  }

  private static Assembly GenerateAssembly(SyntaxTree syntaxTree, string dllName)
  {
    var references = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic)
            .Select(a => a.Location)
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => MetadataReference.CreateFromFile(s))
            .ToList();

    var compilation = CSharpCompilation.Create(
        dllName,
        new[] { syntaxTree },
        references,
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    using var stream = new MemoryStream();

    var emitResult = compilation.Emit(stream);

    if (!emitResult.Success)
    {
      throw new InvalidOperationException($"Failed to compile a dll for {dllName}");
    }

    stream.Seek(0, SeekOrigin.Begin);

    return Assembly.Load(stream.ToArray());
  }
}