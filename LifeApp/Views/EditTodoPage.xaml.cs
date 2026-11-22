using LifeManagement.ViewModels;

namespace LifeManagement.Views
{
    public partial class EditTodoPage : ContentPage
    {
        private readonly EditTodoViewModel _viewModel;

        public EditTodoPage(EditTodoViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        // By now Shell has already given the ViewModel the id (ApplyQueryAttributes)
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
