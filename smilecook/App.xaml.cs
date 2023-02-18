using smilecook.Models;
using smilecook.Services;

namespace smilecook;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
