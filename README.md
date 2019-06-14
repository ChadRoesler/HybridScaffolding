# HybridScaffolding
[![Build Status](https://ci.appveyor.com/api/projects/status/github/ChadRoesler/hybridscaffolding?retina=true)](https://ci.appveyor.com/api/projects/status/github/ChadRoesler/hybridscaffolding?retina=true)

Wiki is slightly out of date, will fix that later.


Parent process code used from:
http://stackoverflow.com/questions/394816/how-to-get-parent-process-in-net-in-managed-way/3346055#3346055


```
[STAThread]

static void Main(string[] arguments)
{
    var yourScaffold = new YourScaffoldModel();

    try
    {
        HybridExecutor.DispatchExecutor(yourScaffold, arguments, typeof(formOfYourChoice));
    }
    catch (Exception ex)
    {
        if(yourScaffold.RunType == RunTypes.Console)
        {
            Console.WriteLine(ex.Message);
        }
        if(yourScaffold.RunType == RunTypes.Powershell)
        {
            PoshHandler.WriteError(ex.Message)
        }
        if(yourScaffold.RunType == RunTypes.Gui)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
```
