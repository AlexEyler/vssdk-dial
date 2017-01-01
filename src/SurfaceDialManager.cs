using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Input;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics;

namespace DialExtension
{
    [Export(typeof(IVsSurfaceDialManager))]
    public class SurfaceDialManager : IVsSurfaceDialManager, IDisposable
    {
        private bool isDisposed = false;
        private Dictionary<RadialControllerMenuItem, IVsSurfaceDialMenuOption> menuOptions;
        private RadialController radialController;

        [ImportingConstructor]
        public SurfaceDialManager([Import(typeof(SVsServiceProvider))]IServiceProvider vsServiceProvider)
        {
            if (vsServiceProvider == null)
            {
                throw new ArgumentNullException(nameof(vsServiceProvider));
            }

            EnvDTE.DTE dteObj = vsServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
            IRadialControllerInterop controllerInterop = (IRadialControllerInterop)System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMarshal.GetActivationFactory(typeof(RadialController));
            Guid iRadialControllerGuid = typeof(RadialController).GetInterface("IRadialController").GUID;

            IntPtr windowHandle = new IntPtr(dteObj.MainWindow.HWnd);
            this.radialController = controllerInterop.CreateForWindow(windowHandle, ref iRadialControllerGuid);
            this.menuOptions = new Dictionary<RadialControllerMenuItem, IVsSurfaceDialMenuOption>();
            this.SubscribeToEvents();
        }

        public void AddMenuOption(IVsSurfaceDialMenuOption menuOption)
        {
            // todo: add support for icon
            var menuItem = RadialControllerMenuItem.CreateFromKnownIcon(menuOption.OptionText, RadialControllerMenuKnownIcon.NextPreviousTrack);
            this.radialController.Menu.Items.Add(menuItem);
            this.menuOptions[menuItem] = menuOption;
        }

        private void SubscribeToEvents()
        {
            this.radialController.RotationChanged += RadialControllerRotationChanged;
            this.radialController.ButtonClicked += RadialControllerButtonClicked;
        }

        private void UnsubscribeFromEvents()
        {
            this.radialController.RotationChanged -= RadialControllerRotationChanged;
            this.radialController.ButtonClicked -= RadialControllerButtonClicked;
        }

        private void RadialControllerButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            var selectedMenuItem = this.radialController.Menu.GetSelectedMenuItem();
            if (this.menuOptions.ContainsKey(selectedMenuItem))
            {
                this.menuOptions[selectedMenuItem].OnDialClicked();
            }
        }

        private void RadialControllerRotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            var selectedMenuItem = this.radialController.Menu.GetSelectedMenuItem();
            if (this.menuOptions.ContainsKey(selectedMenuItem))
            {
                this.menuOptions[selectedMenuItem].OnDialRotated(args.RotationDeltaInDegrees);
            }
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.isDisposed = true;
                this.UnsubscribeFromEvents();
                this.radialController.Menu.Items.Clear();
            }
        }
    }
}
