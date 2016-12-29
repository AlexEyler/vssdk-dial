using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.UI.Input;

namespace DialExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [Guid(DialPackage.PackageGuidString)]
    public sealed class DialPackage : Package
    {
        public const string PackageGuidString = "6d65cd7f-3b7c-488a-9a4d-d81c8adfd776";
        private DTE dte;
        private List<RadialControllerMenuItem> menuItems;
        private RadialController radialController;

        protected override void Initialize()
        {
            base.Initialize();
            this.dte = this.GetService(typeof(DTE)) as DTE;

            IRadialControllerInterop controllerInterop = (IRadialControllerInterop)System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.GetActivationFactory(typeof(RadialController));
            Guid iRadialControllerGuid = typeof(RadialController).GetInterface("IRadialController").GUID;

            IntPtr windowHandle =new IntPtr(dte.ActiveWindow.HWnd);
            this.radialController = controllerInterop.CreateForWindow(windowHandle, ref iRadialControllerGuid);
            this.menuItems = new List<RadialControllerMenuItem>
            {
                // todo: Come up with a better name than the "Visual Studio Line Mover"
                RadialControllerMenuItem.CreateFromKnownIcon("Visual Studio Line Mover", RadialControllerMenuKnownIcon.Scroll)
            };
            foreach (var item in this.menuItems)
            {
                this.radialController.Menu.Items.Add(item);
            }

            this.radialController.RotationChanged += RadialControllerRotationChanged;
            this.radialController.ButtonClicked += RadialControllerButtonClicked;
            this.dte.Events.SolutionEvents.AfterClosing += () =>
            {
                this.radialController.Menu.Items.Clear();
            };
        }

        private void RadialControllerButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            // Do nothing because I can't think of anything to do yet
        }

        private void RadialControllerRotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            TextSelection selection = this.dte.ActiveDocument.Selection as TextSelection;
            if (selection != null)
            {
                if (args.RotationDeltaInDegrees > 0)
                {
                    selection.LineDown();
                }
                else
                {
                    selection.LineUp();
                }
            }
        }
    }
}
