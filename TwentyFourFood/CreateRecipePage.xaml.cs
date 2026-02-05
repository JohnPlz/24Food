using TwentyFourFood.ViewModels;

namespace TwentyFourFood
{
    public partial class CreateRecipePage : ContentPage
    {
        public CreateRecipePage(RecipesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
