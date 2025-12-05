using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancingPlatform.Logic.Models.Runtime;
public class ServoRuntime : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    private object _lock = new object();

    private double _a1;
    private double _a2;
    private double _a3;

    public double ServoAngle1 {
        get {
            lock (_lock) {
                return _a1;
            }
        }
        set {
            lock (_lock) {
                if (_a1 != value) {
                    _a1 = value;
                    OnPropertyChanged(nameof(ServoAngle1));
                }
            }
        }
    }

    public double ServoAngle2 {
        get {
            lock (_lock) {
                return _a2;
            }
        }
        set {
            lock (_lock) {
                if (_a2 != value) {
                    _a2 = value;
                    OnPropertyChanged(nameof(ServoAngle2));
                }
            }
        }
    }

    public double ServoAngle3 {
        get {
            lock (_lock) {
                return _a3;
            }
        }
        set {
            lock (_lock) {
                if (_a3 != value) {
                    _a3 = value;
                    OnPropertyChanged(nameof(ServoAngle3));
                }
            }
        }
    }

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
