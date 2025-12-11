using BalancingPlatform.Logic.Models.Params;
using BalancingPlatform.Logic.Models.Runtime;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MudBlazor.Interfaces;
using SystBalancingPlatform.Logicem.Models.Runtime;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;

namespace BalancingPlatform.WEB.ViewModels;
public partial class MainWindowViewModel : ObservableObject {
    public CvParams _cvParams { get; set; }
    public PidParams _pidParams { get; set; }
    public ServoParams _servoParams { get; set; }
    public CvRuntime _cvRuntime { get; set; }
    public PidRuntime _pidRuntime { get; set; }
    public ServoRuntime _servoRuntime { get; set; }

    private System.Timers.Timer timer = new System.Timers.Timer(50);

    public ObservableCollection<double> BallPosArrX { get; set; } = new ObservableCollection<double>();
    public ObservableCollection<double> BallPosArrY { get; set; } = new ObservableCollection<double>();
    public ObservableCollection<double> SetpointArrX { get; set; } = new ObservableCollection<double>();
    public ObservableCollection<double> SetpointArrY { get; set; } = new ObservableCollection<double>();

    public object Sync { get; set; } = new object();

    public MainWindowViewModel(CvParams cvParams, PidParams pidParams, ServoParams servoParams, CvRuntime cvRuntime, PidRuntime pidRuntime, ServoRuntime servoRuntime) {
        _cvParams = cvParams;
        _pidParams = pidParams;
        _servoParams = servoParams;
        _cvRuntime = cvRuntime;
        _pidRuntime = pidRuntime;
        _servoRuntime = servoRuntime;

        _cvRuntime.PropertyChanged += PropChanged;

        timer.Elapsed += TimerElapsed;
        timer.Enabled = true;
        timer.AutoReset = true;
        timer.Start();
    }

    private void PropChanged(object sender, PropertyChangedEventArgs a) {
        
    }

    private void TimerElapsed(object sender, ElapsedEventArgs args) {
        lock (Sync) {
            AddToCollection(BallPosArrX, Math.Round(_cvRuntime.BallPosX, 2));
            AddToCollection(BallPosArrY, Math.Round(_cvRuntime.BallPosY, 2));
            AddToCollection(SetpointArrX, _pidParams.SetpointX);
            AddToCollection(SetpointArrY, _pidParams.SetpointY);
            OnPropertyChanged(nameof(_cvRuntime.BallPosX));
        }
    }

    private void AddToCollection(ObservableCollection<double> collection, double value) {
        collection.Add(value);
        if(collection.Count > 1024)
            collection.RemoveAt(0);
    }

    [RelayCommand]
    private async Task SavePidParams() {
        Utility.SavePidParams(_pidParams);
    }

    [RelayCommand]
    private async Task SaveCvParams() {
        Utility.SaveCvParams(_cvParams);
    }

    [RelayCommand]
    private void ChangeSetpointAbsolute(ValueTuple<double, double> point) {
        //X and Y coord is relative to the image and is specified as a percentage of the width and height

        var size = _cvParams.Resolution;
        var centerX = _cvParams.PlatformCenterX;
        var centerY = _cvParams.PlatformCenterY;
        var radius = _cvParams.PlatformRadius;

        //Get actual position
        var actX = size * point.Item1 / 100;
        var actY = size * point.Item2 / 100;

        //If point is outside of the platform area, do nothing
        var lowerX = (int)centerX - (int)radius;
        var upperX = (int)centerX + (int)radius;
        var lowerY = (int)centerY - (int)radius;
        var upperY = (int)centerY + (int)radius;

        if (actX < lowerX || actX > upperX || actY < lowerY || actY > upperY)
            return;

        //Find point relative to platform center
        var x = actX - centerX;
        var y = actY - centerY;

        var radi = Math.Sqrt(x * x + y * y);
        if (radi > radius)
            return;

        _pidParams.SetpointX = x;
        _pidParams.SetpointY = y;
    }
}
