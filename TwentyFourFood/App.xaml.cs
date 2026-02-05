using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace TwentyFourFood
{
    public partial class App : Application
    {
        public App()
        {
            var culture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}