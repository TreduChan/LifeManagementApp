namespace LifeManagement.Models
{
    // One row in the Todos table. A plain class (no MAUI or UI code) so the
    // database and the unit tests can use it without knowing about pages.
    public class TodoItem
    {
        // EF Core treats a property named "Id" as the primary key automatically
        // and lets SQLite generate the number when a new item is added.
        public int Id { get; set; }

        public string Title { get; set; } = "";

        // Optional extras that the add form already has
        public string Notes { get; set; } = "";
        public string Category { get; set; } = "";

        // 1 = lowest, 5 = highest. The range is enforced by the Stepper in the UI.
        public int Priority { get; set; } = 3;

        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(1);

        // false = still to do. The home page shows only unfinished items.
        public bool IsDone { get; set; }
    }
}
