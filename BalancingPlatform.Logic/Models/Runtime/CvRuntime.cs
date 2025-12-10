using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystBalancingPlatform.Logicem.Models.Runtime;
public class CvRuntime : INotifyPropertyChanged{
    public event PropertyChangedEventHandler PropertyChanged;

    private object _lock = new object();

    private double _ballPosX;
    private double _ballPosY;
    private byte[] _src;
    private byte[] _hsv;
    private byte[] _mask;

    public double BallPosX {
        get {
            lock (_lock) {
                return _ballPosX;
            }
        }
        set {
            lock (_lock) {
                if (_ballPosX != value) {
                    _ballPosX = value;
                    OnPropertyChanged(nameof(BallPosX));
                }
            }
        }
    }

    public double BallPosY {
        get {
            lock (_lock) {
                return _ballPosY;
            }
        }
        set {
            lock (_lock) {
                if (_ballPosY != value) {
                    _ballPosY = value;
                    OnPropertyChanged(nameof(BallPosY));
                }
            }
        }
    }

    public byte[] SrcFrame {
        get {
            lock (_lock) {
                return _src;
            }
        }
        set {
            lock (_lock) {
                if (_src != value) {
                    _src = value;
                    OnPropertyChanged(nameof(SrcFrame));
                }
            }
        }
    }

    public byte[] HsvFrame {
        get {
            lock (_lock) {
                return _hsv;
            }
        }
        set {
            lock (_lock) {
                if (_hsv != value) {
                    _hsv = value;
                    OnPropertyChanged(nameof(HsvFrame));
                }
            }
        }
    }

    public byte[] MaskFrame {
        get {
            lock (_lock) {
                return _mask;
            }
        }
        set {
            lock (_lock) {
                if (_mask != value) {
                    _mask = value;
                    OnPropertyChanged(nameof(MaskFrame));
                }
            }
        }
    }

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
