using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancingPlatform.Logic.Models.Params;
public class ServoParams : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    private object _lock = new object();

    private double _l;
    private double _r;
    private bool _disable;

    public double L {
        get {
            lock (_lock) {
                return _l;
            }
        }
        set {
            lock (_lock) {
                if (_l != value) {
                    _l = value;
                    OnPropertyChanged(nameof(L));
                }
            }
        }
    }

    public double R {
        get {
            lock (_lock) {
                return _r;
            }
        }
        set {
            lock (_lock) {
                if (_r != value) {
                    _r = value;
                    OnPropertyChanged(nameof(R));
                }
            }
        }
    }

    public bool Disable {
        get {
            lock (_lock) {
                return _disable;
            }
        }
        set {
            lock (_lock) {
                if (_disable != value) {
                    _disable = value;
                    OnPropertyChanged(nameof(Disable));
                }
            }
        }
    }

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
