using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancingPlatform.Logic.Models.Params;
public class PidParams : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    private object _lock = new object();

    private double _kp;
    private double _ki;
    private double _kd;
    private double _dt;
    private double _minOut;
    private double _maxOut;
    private double _alpha;
    private double _setX;
    private double _setY;
    private bool _disable;

    public double Kp {
        get {
            lock (_lock) {
                return _kp;
            }
        }
        set {
            lock (_lock) {
                if (_kp != value) {
                    _kp = value;
                    OnPropertyChanged(nameof(Kp));
                }
            }
        }
    }

    public double Ki {
        get {
            lock (_lock) {
                return _ki;
            }
        }
        set {
            lock (_lock) {
                if (_ki != value) {
                    _ki = value;
                    OnPropertyChanged(nameof(Ki));
                }
            }
        }
    }

    public double Kd {
        get {
            lock (_lock) {
                return _kd;
            }
        }
        set {
            lock (_lock) {
                if (_kd != value) {
                    _kd = value;
                    OnPropertyChanged(nameof(Kd));
                }
            }
        }
    }

    public double Dt {
        get {
            lock (_lock) {
                return _dt;
            }
        }
        set {
            lock (_lock) {
                if (_dt != value) {
                    _dt = value;
                    OnPropertyChanged(nameof(Dt));
                }
            }
        }
    }

    public double MinOutput {
        get {
            lock (_lock) {
                return _minOut;
            }
        }
        set {
            lock (_lock) {
                if (_minOut != value) {
                    _minOut = value;
                    OnPropertyChanged(nameof(MinOutput));
                }
            }
        }
    }

    public double MaxOutput {
        get {
            lock (_lock) {
                return _maxOut;
            }
        }
        set {
            lock (_lock) {
                if (_maxOut != value) {
                    _maxOut = value;
                    OnPropertyChanged(nameof(MaxOutput));
                }
            }
        }
    }

    public double Alpha {
        get {
            lock (_lock) {
                return _alpha;
            }
        }
        set {
            lock (_lock) {
                if (_alpha != value) {
                    _alpha = value;
                    OnPropertyChanged(nameof(Alpha));
                }
            }
        }
    }

    public double SetpointX {
        get {
            lock (_lock) {
                return _setX;
            }
        }
        set {
            lock (_lock) {
                if (_setX != value) {
                    _setX = value;
                    OnPropertyChanged(nameof(SetpointX));
                }
            }
        }
    }

    public double SetpointY {
        get {
            lock (_lock) {
                return _setY;
            }
        }
        set {
            lock (_lock) {
                if (_setY != value) {
                    _setY = value;
                    OnPropertyChanged(nameof(SetpointY));
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
