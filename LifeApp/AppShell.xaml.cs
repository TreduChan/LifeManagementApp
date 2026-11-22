using LifeManagement.Views;

namespace LifeManagement
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // AddTodoPage is not a tab, so Shell needs a route name to navigate to it:
            // GoToAsync("AddTodoPage") pushes it on top of the list page.
            Routing.RegisterRoute(nameof(AddTodoPage), typeof(AddTodoPage));

            // The edit page gets the todo's id as a parameter: GoToAsync("EditTodoPage?id=5")
            Routing.RegisterRoute(nameof(EditTodoPage), typeof(EditTodoPage));
        }
    }
}
