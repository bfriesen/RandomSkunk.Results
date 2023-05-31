using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace RandomSkunk.Results.Analyzers;

/// <summary>
/// An code fix provider that simplifies a condition for maybe.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SimplifyResultExpressionCodeFixProvider))]
[Shared]
public class SimplifyResultExpressionCodeFixProvider : CodeFixProvider
{
    /// <inheritdoc/>
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticIds.ResultExpressionCanBeSimplified);

    /// <inheritdoc/>
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = (await context.Document.GetSyntaxRootAsync(context.CancellationToken))!;
        var semanticModel = (await context.Document.GetSemanticModelAsync(context.CancellationToken))!;

        foreach (var diagnostic in context.Diagnostics)
        {
            var node = root.FindNode(diagnostic.Location.SourceSpan);
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Simplify result expression",
                    createChangedDocument: cancellationToken => SimplifyResultExpression(context.Document, node, diagnostic, cancellationToken),
                    equivalenceKey: "CBEB70F1-2C34-493C-BCD3-4B192DEB03D5"),
                diagnostic);
        }
    }

    /// <inheritdoc/>
    public override FixAllProvider? GetFixAllProvider()
    {
        return WellKnownFixAllProviders.BatchFixer;
    }

    private async Task<Document> SimplifyResultExpression(Document document, SyntaxNode node, Diagnostic diagnostic, CancellationToken cancellationToken)
    {
        var propertyName = diagnostic.Properties["PropertyName"]!;
        var instanceName = diagnostic.Properties["InstanceName"]!;
        var propertyValue = bool.Parse(diagnostic.Properties["PropertyValue"]);

        var property = SyntaxFactory.IdentifierName(propertyName);
        var instance = SyntaxFactory.IdentifierName(instanceName);

        ExpressionSyntax replacementNode = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, instance, property);

        if (!propertyValue)
            replacementNode = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, replacementNode);

        var root = (await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false))!;

        root = root.ReplaceNode(node, replacementNode);

        return document.WithSyntaxRoot(root);
    }
}
