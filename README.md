# HybridScaffolding
[![Build Status](https://ci.appveyor.com/api/projects/status/github/ChadRoesler/hybridscaffolding?retina=true)](https://ci.appveyor.com/api/projects/status/github/ChadRoesler/hybridscaffolding?retina=true)

Wiki is slightly out of date, will fix that later.


Parent process code used from:
http://stackoverflow.com/questions/394816/how-to-get-parent-process-in-net-in-managed-way/3346055#3346055


```
[STAThread]

static void Main(string[] arguments)
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    var yourScaffold = new YourScaffoldModel();

    try
    {
        HybridExecutor.DispatchExecutor(yourScaffold, arguments, typeof(formOfYourChoice), RunType.Gui);
    }
    catch (Exception ex)
    {
        if(yourScaffold.RunType == RunType.Console)
        {
            Console.WriteLine(ex.Message);
        }
        if(yourScaffold.RunType == RunType.Powershell)
        {
            PoshHandler.WriteError(ex.Message)
        }
        if(yourScaffold.RunType == RunType.Gui)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
```
