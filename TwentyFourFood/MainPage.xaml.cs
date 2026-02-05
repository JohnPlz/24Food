using TwentyFourFood.Services;
using TwentyFourFood.ViewModels;

namespace TwentyFourFood
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel(new LiteDbService());
        }
    }
}
