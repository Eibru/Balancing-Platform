using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalancingPlatform.Logic.Models.Params;
public class CvParams : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    private object _lock = new object();

    private byte _lower1_h;
    private byte _lower1_s;
    private byte _lower1_v;
    private byte _lower2_h;
    private byte _lower2_s;
    private byte _lower2_v;
    private byte _upper1_h;
    private byte _upper1_s;
    private byte _upper1_v;
    private byte _upper2_h;
    private byte _upper2_s;
    private byte _upper2_v;
    private uint _platformCenterX;
    private uint _platformCenterY;
    private uint _platformRadius = 100;
    private uint _res = 300;
    private int _sampleDelay = 20;
    private double _setpointX;
    private double _setpointY;
    private bool _disable;

    public byte Lower1_H {
        get {
            lock (_lock) {
                return _lower1_h;
            }
        } 
        set {
            lock (_lock) {
                if (_lower1_h != value) {
                    _lower1_h = value;
                    OnPropertyChanged(nameof(Lower1_H));
                }
            }
        }
    }

    public byte Lower1_S {
        get {
            lock (_lock) {
                return _lower1_s;
            }
        }
        set {
            lock (_lock) {
                if (_lower1_s != value) {
                    _lower1_s = value;
                    OnPropertyChanged(nameof(Lower1_S));
                }
            }
        }
    }

    public byte Lower1_V {
        get {
            lock (_lock) {
                return _lower1_v;
            }
        }
        set {
            lock (_lock) {
                if (_lower1_v != value) {
                    _lower1_v = value;
                    OnPropertyChanged(nameof(Lower1_V));
                }
            }
        }
    }

    public byte Lower2_H {
        get {
            lock (_lock) {
                return _lower2_h;
            }
        }
        set {
            lock (_lock) {
                if (_lower2_h != value) {
                    _lower2_h = value;
                    OnPropertyChanged(nameof(Lower2_H));
                }
            }
        }
    }

    public byte Lower2_S {
        get {
            lock (_lock) {
                return _lower2_s;
            }
        }
        set {
            lock (_lock) {
                if (_lower2_s != value) {
                    _lower2_s = value;
                    OnPropertyChanged(nameof(Lower2_S));
                }
            }
        }
    }

    public byte Lower2_V {
        get {
            lock (_lock) {
                return _lower2_v;
            }
        }
        set {
            lock (_lock) {
                if (_lower2_v != value) {
                    _lower2_v = value;
                    OnPropertyChanged(nameof(Lower2_V));
                }
            }
        }
    }

    public byte Upper1_H {
        get {
            lock (_lock) {
                return _upper1_h;
            }
        }
        set {
            lock (_lock) {
                if (_upper1_h != value) {
                    _upper1_h = value;
                    OnPropertyChanged(nameof(Upper1_H));
                }
            }
        }
    }

    public byte Upper1_S {
        get {
            lock (_lock) {
                return _upper1_s;
            }
        }
        set {
            lock (_lock) {
                if (_upper1_s != value) {
                    _upper1_s = value;
                    OnPropertyChanged(nameof(Upper1_S));
                }
            }
        }
    }

    public byte Upper1_V {
        get {
            lock (_lock) {
                return _upper1_v;
            }
        }
        set {
            lock (_lock) {
                if (_upper1_v != value) {
                    _upper1_v = value;
                    OnPropertyChanged(nameof(Upper1_V));
                }
            }
        }
    }

    public byte Upper2_H {
        get {
            lock (_lock) {
                return _upper2_h;
            }
        }
        set {
            lock (_lock) {
                if (_upper2_h != value) {
                    _upper2_h = value;
                    OnPropertyChanged(nameof(Upper2_H));
                }
            }
        }
    }

    public byte Upper2_S {
        get {
            lock (_lock) {
                return _upper2_s;
            }
        }
        set {
            lock (_lock) {
                if (_upper2_s != value) {
                    _upper2_s = value;
                    OnPropertyChanged(nameof(Upper2_S));
                }
            }
        }
    }

    public byte Upper2_V {
        get {
            lock (_lock) {
                return _upper2_v;
            }
        }
        set {
            lock (_lock) {
                if (_upper2_v != value) {
                    _upper2_v = value;
                    OnPropertyChanged(nameof(Upper2_V));
                }
            }
        }
    }

    public uint PlatformCenterX {
        get {
            lock (_lock) {
                return _platformCenterX;
            }
        }
        set {
            lock (_lock) {
                if (_platformCenterX != value) {
                    _platformCenterX = value;
                    OnPropertyChanged(nameof(PlatformCenterX));
                }
            }
        }
    }

    public uint PlatformCenterY {
        get {
            lock (_lock) {
                return _platformCenterY;
            }
        }
        set {
            lock (_lock) {
                if (_platformCenterY != value) {
                    _platformCenterY = value;
                    OnPropertyChanged(nameof(PlatformCenterY));
                }
            }
        }
    }

    public uint PlatformRadius {
        get {
            lock (_lock) {
                return _platformRadius;
            }
        }
        set {
            lock (_lock) {
                if (_platformRadius != value) {
                    _platformRadius = value;
                    OnPropertyChanged(nameof(PlatformRadius));
                }
            }
        }
    }

    public uint Resolution {
        get {
            lock (_lock) {
                return _res;
            }
        }
        set {
            lock (_lock) {
                if (_res != value) {
                    _res = value;
                    OnPropertyChanged(nameof(Resolution));
                }
            }
        }
    }

    public int SampleDelay {
        get {
            lock (_lock) {
                return _sampleDelay;
            }
        }
        set {
            lock (_lock) {
                if (_sampleDelay != value) {
                    _sampleDelay = value;
                    OnPropertyChanged(nameof(SampleDelay));
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
