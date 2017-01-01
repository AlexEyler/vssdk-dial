using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using System;

namespace DialExtension
{
    public class SelectionMenuItem : IVsSurfaceDialMenuOption
    {
        private IEditorOperations editorOperations;
        private bool selectionEnabled;

        public SelectionMenuItem(IEditorOperations editorOptions)
        {
            this.editorOperations = editorOptions;
            this.selectionEnabled = false;
        }

        public string OptionText => "Visual Studio Selection";

        public void OnActivated()
        {
        }

        public void OnDeactivated()
        {
        }

        public void OnDialClicked()
        {
            this.selectionEnabled = !this.selectionEnabled;
        }

        public void OnDialRotated(double rotationInDegrees)
        {
            if (rotationInDegrees > 0)
            {
                this.editorOperations.MoveLineDown(this.selectionEnabled);
            }
            else
            {
                this.editorOperations.MoveLineUp(this.selectionEnabled);
            }
        }
    }
}
