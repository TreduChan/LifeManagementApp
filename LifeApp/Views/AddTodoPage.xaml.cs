using LifeManagement.ViewModels;

namespace LifeManagement.Views
{
    public partial class AddTodoPage : ContentPage
    {
        // The page does not create its ViewModel with "new": DI passes it in,
        // together with everything the ViewModel itself needs (the repository).
        public AddTodoPage(AddTodoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // pure UI reaction → an event is still the right tool
        protected override void OnAppearing()
        {
            base.OnAppearing();
            TitleEntry.Focus();
        }
    }
}
