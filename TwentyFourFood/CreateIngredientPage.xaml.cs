using TwentyFourFood.ViewModels;

namespace TwentyFourFood
{
    public partial class CreateIngredientPage : ContentPage
    {
        public CreateIngredientPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
