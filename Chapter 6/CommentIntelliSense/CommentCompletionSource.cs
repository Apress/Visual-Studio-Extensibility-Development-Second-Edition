using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace CommentIntellisense
{
    internal class CommentCompletionSource : IAsyncCompletionSource
    {
        private static readonly ImageElement _icon = new ImageElement(KnownMonikers.Cloud.ToImageId(), "elementIcon");        
        private ImmutableArray<CompletionItem> _commentItems;

        public CommentCompletionSource()
        {
            var list = new List<CompletionItem>
            {
              new CompletionItem("Book 1 - .NET Core 2.0 By Example.", this, _icon),
              new CompletionItem("Book 2 - Parallel Programming With C# and .NET Core", this, _icon),
              new CompletionItem("Book 3 - Visual Studio Extensibility Development", this, _icon)
            };
            _commentItems = list.ToImmutableArray<CompletionItem>();
        }

        public Task<CompletionContext> GetCompletionContextAsync(IAsyncCompletionSession session, 
            CompletionTrigger trigger, SnapshotPoint triggerLocation, SnapshotSpan applicableToSpan, 
            CancellationToken token)
        {
            var containingLine = triggerLocation.GetContainingLine();
            var text = containingLine.Extent.GetText();

            if (!string.IsNullOrWhiteSpace(text) && text.IndexOf("// RV", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // Show list of available comments.
                return Task.FromResult(new CompletionContext(_commentItems));
            }

            return Task.FromResult(CompletionContext.Empty);
        }

        public CompletionStartData InitializeCompletion(CompletionTrigger trigger, SnapshotPoint triggerLocation, 
            CancellationToken token)
        {
            var containingLine = triggerLocation.GetContainingLine();
            var text = containingLine.Extent.GetText();
            if (text.HasRvTrigger())
            {
                return CompletionStartData.ParticipatesInCompletionIfAny;
            }

            return CompletionStartData.DoesNotParticipateInCompletion;
        }

        public async Task<object> GetDescriptionAsync(IAsyncCompletionSession session, CompletionItem item, CancellationToken token)
        {
            var descriptionRun = new ClassifiedTextRun(PredefinedClassificationTypeNames.Other, item.InsertText + ". ");
            var descriptionTextElement = new ClassifiedTextElement(descriptionRun);
            var content = new ContainerElement(ContainerElementStyle.Wrapped, _icon, descriptionTextElement);
            return new ContainerElement(ContainerElementStyle.Stacked, content);
        }
    }
}