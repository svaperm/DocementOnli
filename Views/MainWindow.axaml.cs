using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Documently.Models;
using Documently.ViewModels;
namespace Documently.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow () => new MainWindow(ViewModel!);
    public MainWindow (MainWindowViewModel m)
    {
        InitializeComponent();
        this.WhenActivated(a => a(m.ConfirmDialogInteraction.RegisterHandler(ShowMessage)));
        this.WhenActivated(a => a(m.OpenDialogInteraction.RegisterHandler(ShowOpenFileWindow)));
        this.WhenActivated(a => a(m.SaveDialogInteraction.RegisterHandler(ShowSaveFileWindow)));

        this.WhenActivated(a => a(m.NameCategoryInteraction.RegisterHandler(ShowMessage)));

        this.WhenActivated(a => a(m.ConfirmDialogInteraction.RegisterHandler(ShowMessage)));
        this.Closing += m.OnWindowClose;

        DataContext = m;
    }
    private async Task ShowMessage(InteractionContext<MessageBoxViewModel, string> i)
    {
        MessageBox dialog = new MessageBox();
        dialog.DataContext = i.Input;
        string result = await dialog.ShowAsync(this);
        i.SetOutput(result);
    }
    private async Task ShowOpenFileWindow(InteractionContext<FileDialogFilter, string> i)
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filters = new List<FileDialogFilter>() { i.Input };
        string[]? result = await dialog.ShowAsync(this);
        if (result is null)
        {
            i.SetOutput(string.Empty);
        }
        else
        {
            i.SetOutput(result[0]);
        }
    }
    private async Task ShowSaveFileWindow(InteractionContext<FileDialogFilter, string> i)
    {
        SaveFileDialog dialog = new SaveFileDialog();
        dialog.Filters = new List<FileDialogFilter>() { i.Input };
        string? result = await dialog.ShowAsync(this);
        if (result is null)
        {
            i.SetOutput(string.Empty);
        }
        else
        {
            i.SetOutput(result);
        }
    }
}