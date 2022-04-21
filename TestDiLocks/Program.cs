using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<MainClass>();
serviceCollection.AddSingleton<ISecondDependency, SecondDependency>();
var provider = serviceCollection.BuildServiceProvider();
// новое поведение, не работает с 5 версией, но работает с 6
Provider.ServiceProvider = provider;
// старое поведение, работает с 5 версией
//Provider.ServiceProvider = serviceCollection.BuildServiceProvider();
var mainClass = provider.GetRequiredService<MainClass>();
mainClass.Do();

public class Provider
{
    public static IServiceProvider ServiceProvider;
}

public class MainClass
{
    private ISecondDependency _dependency;

    public MainClass(IServiceProvider serviceProvider)
    {
        Task.Run(() => { _dependency = Provider.ServiceProvider.GetRequiredService<ISecondDependency>(); }).GetAwaiter()
            .GetResult();
    }

    public void Do()
    {
        Console.WriteLine("MainClass Do");
        _dependency.Do();
    }
}

public interface ISecondDependency
{
    public void Do();
}

public class SecondDependency : ISecondDependency
{
    public void Do()
    {
        Console.WriteLine("SecondDependency Do");
    }
}