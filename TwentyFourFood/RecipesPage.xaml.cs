using TwentyFourFood.ViewModels;

namespace TwentyFourFood
{
    public partial class RecipesPage : ContentPage
    {
        public RecipesPage(RecipesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
