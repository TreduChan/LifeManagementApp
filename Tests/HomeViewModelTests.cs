using LifeManagement.Models;
using LifeManagement.ViewModels;

namespace LifeManagement.Tests;

// Tests for the two rules on the home page. Both are static methods,
// so no database, network or MAUI page is needed to run them.
public class HomeViewModelTests
{
    [Fact]
    public void PickTop3_SortsByPriorityThenDueDate_AndSkipsDoneItems()
    {
        var today = DateTime.Today;
        var todos = new List<TodoItem>
        {
            new() { Title = "Low",           Priority = 1, DueDate = today },
            new() { Title = "High later",    Priority = 5, DueDate = today.AddDays(3) },
            new() { Title = "High sooner",   Priority = 5, DueDate = today.AddDays(1) },
            new() { Title = "Done, highest", Priority = 5, DueDate = today, IsDone = true },
            new() { Title = "Middle",        Priority = 3, DueDate = today },
        };

        var top = HomeViewModel.PickTop3(todos);

        // Same priority 5: the earlier due date wins. The done item is never shown.
        Assert.Equal(new[] { "High sooner", "High later", "Middle" }, top.Select(t => t.Title));
    }

    [Fact]
    public void PickTop3_ReturnsFewerThanThree_WhenThereAreFewerTodos()
    {
        var todos = new List<TodoItem> { new() { Title = "Only one", Priority = 2 } };

        Assert.Single(HomeViewModel.PickTop3(todos));
    }

    // Theory = the same test run once per InlineData row
    [Theory]
    [InlineData(4.9, false)]
    [InlineData(5.0, false)]   // exactly the limit is not "above 5"
    [InlineData(5.1, true)]
    public void IsKitesurfWind_IsTrueOnlyAboveFiveMetresPerSecond(double wind, bool expected)
    {
        Assert.Equal(expected, HomeViewModel.IsKitesurfWind(wind));
    }

    // The empty-title rule used by the add and edit pages
    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("Buy milk", true)]
    public void IsValidTitle_RejectsEmptyTitles(string title, bool expected)
    {
        Assert.Equal(expected, TodoItem.IsValidTitle(title));
    }
}
