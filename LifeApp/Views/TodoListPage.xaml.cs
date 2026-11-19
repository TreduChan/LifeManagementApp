using LifeManagement.ViewModels;

namespace LifeManagement.Views
{
    public partial class TodoListPage : ContentPage
    {
        private readonly TodoListViewModel _viewModel;

        // Same as AddTodoPage: DI passes the ViewModel in
        public TodoListPage(TodoListViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        // Runs every time the page becomes visible, also when coming back from AddTodoPage,
        // so the list is reloaded and shows the new todo
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
