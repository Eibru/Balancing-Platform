using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using BalancingPlatform.GUI.ViewModels;
using BalancingPlatform.Logic;
using BalancingPlatform.Logic.Models.Params;
using BalancingPlatform.Logic.Models.Runtime;
using Microsoft.Extensions.DependencyInjection;
using SystBalancingPlatform.Logicem.Models.Runtime;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace BalancingPlatform.GUI;
public partial class App : Application {
    private static string CVPARAMS_PATH = "./CvParams.json";
    private static string PIDPARAMS_PATH = "./PidParams.json";
    private CvController CvController;
    private PidController PidController;
    private ServoController KinematicsController;
    private Thread CvThread;
    private Thread PidThread;
    private Thread KinematicsThread;
    private CancellationToken CancellationToken = new CancellationToken();

    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        BindingPlugins.DataValidators.RemoveAt(0);

        var collection = CreateServiceCollection();

        var services = collection.BuildServiceProvider();

        StartThreads(services);

        var vm = services.GetRequiredService<MainWindowViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow {
                DataContext = vm
            };
        } 
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
            singleViewPlatform.MainView = new MainWindow {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation() {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove) {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private ServiceCollection CreateServiceCollection() {
        var collection = new ServiceCollection();
        var cvParams = ReadCvParams();
        var pidParams = ReadPidParams();

        var kinematicsParams = new ServoParams { L = 12, R = 12 };

        var cvRuntime = new CvRuntime();
        var pidRuntime = new PidRuntime();
        var kinematicsRuntime = new ServoRuntime();

        collection.AddSingleton(cvParams);
        collection.AddSingleton(pidParams);
        collection.AddSingleton(kinematicsParams);
        collection.AddSingleton(cvRuntime);
        collection.AddSingleton(pidRuntime);
        collection.AddSingleton(kinematicsRuntime);

        collection.AddSingleton<MainWindowViewModel>();

        return collection;
    }

    private void StartThreads(ServiceProvider services) {
        var cvParams = services.GetRequiredService<CvParams>();
        var cvRuntime = services.GetRequiredService<CvRuntime>();
        var pidParams = services.GetRequiredService<PidParams>();
        var pidRuntime = services.GetRequiredService<PidRuntime>();
        var kinematicsParams = services.GetRequiredService<ServoParams>();
        var kinematicsRuntime = services.GetRequiredService<ServoRuntime>();

        CvController = new CvController(cvParams, cvRuntime, pidParams);
        PidController = new PidController(pidParams, pidRuntime, cvRuntime);
        KinematicsController = new ServoController(kinematicsParams, kinematicsRuntime, pidRuntime);

        CvThread = new Thread(async delegate () {
            await CvController.Run(CancellationToken);
        });

        PidThread = new Thread(async delegate () {
            await PidController.Run(CancellationToken);
        });

        KinematicsThread = new Thread(async delegate () {
            await KinematicsController.Run(CancellationToken);
        });

        CvThread.Start();
        PidThread.Start();
        KinematicsThread.Start();
    }

    public static void SavePidParams(PidParams pidParams) {
        File.WriteAllText(PIDPARAMS_PATH, JsonSerializer.Serialize(pidParams));
    }

    public static void SaveCvParams(CvParams cvParams) {
        File.WriteAllText(CVPARAMS_PATH, JsonSerializer.Serialize(cvParams));
    }

    public static PidParams ReadPidParams() {
        PidParams parms = null;

        try {
            var jsonStr = File.ReadAllText(PIDPARAMS_PATH);
            parms = JsonSerializer.Deserialize<PidParams>(jsonStr);
        } catch (Exception ex) { }

        if (parms == null) {
            parms = new PidParams {
                Dt = 1
            };
        }

        return parms;
    }

    public static CvParams ReadCvParams() {
        CvParams parms = null;
        try {
            var jsonStr = File.ReadAllText(CVPARAMS_PATH);
            parms = JsonSerializer.Deserialize<CvParams>(jsonStr);
        } catch (Exception ex) { }

        if (parms == null) {
            parms = new CvParams {
                SampleDelay = 30
            };
        }

        return parms;
    }
}