using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancingPlatform.Logic.Models.Runtime;
public class PidRuntime : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    private object _lock = new object();

    private double _outX;
    private double _outY;
    private double _errX;
    private double _errY;

    public double OutputX {
        get {
            lock (_lock) {
                return _outX;
            }
        }
        set {
            lock (_lock) {
                if (_outX != value) {
                    _outX = value;
                    OnPropertyChanged(nameof(OutputX));
                }
            }
        }
    }

    public double OutputY {
        get {
            lock (_lock) {
                return _outY;
            }
        }
        set {
            lock (_lock) {
                if (_outY != value) {
                    _outY = value;
                    OnPropertyChanged(nameof(OutputY));
                }
            }
        }
    }

    public double ErrorX {
        get {
            lock (_lock) {
                return _errX;
            }
        }
        set {
            lock (_lock) {
                if (_errX != value) {
                    _errX = value;
                    OnPropertyChanged(nameof(ErrorX));
                }
            }
        }
    }

    public double ErrorY {
        get {
            lock (_lock) {
                return _errY;
            }
        }
        set {
            lock (_lock) {
                if (_errY != value) {
                    _errY = value;
                    OnPropertyChanged(nameof(ErrorY));
                }
            }
        }
    }

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
