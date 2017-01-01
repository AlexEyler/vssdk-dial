using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialExtension
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class TextViewListener : IWpfTextViewCreationListener
    {
        [Import]
        private IEditorOperationsFactoryService editorOperationsFactoryService = null;

        [Import]
        private IVsSurfaceDialManager surfaceDialManager = null;

        public void TextViewCreated(IWpfTextView textView)
        {
            var editorOperations = this.editorOperationsFactoryService.GetEditorOperations(textView);
            this.surfaceDialManager.AddMenuOption(new SelectionMenuItem(editorOperations));
        }
    }
}
