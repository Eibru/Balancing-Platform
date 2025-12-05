using OpenCvSharp;
using BalancingPlatform.Logic.Models.Params;
using BalancingPlatform.Logic.Models.Runtime;
using SystBalancingPlatform.Logicem.Models.Runtime;

namespace BalancingPlatform.Logic;

public class CvController {
    protected readonly CvParams _cvParams;
    protected readonly CvRuntime _cvRuntime;
    protected readonly PidParams _pidParams;

    public CvController(CvParams cvParams, CvRuntime cvRuntime, PidParams pidParams) {
        _cvParams = cvParams;
        _cvRuntime = cvRuntime;
        _pidParams = pidParams;
    }

    public async Task Run(CancellationToken cancellationToken) {
        var testSource = new Mat("./ball.jpg");
        var cameraSource = Cv2.CreateFrameSource_Camera(0);
        
        while (!cancellationToken.IsCancellationRequested) {
            //Read params
            var delay = _cvParams.SampleDelay;
            var lower1_h = _cvParams.Lower1_H;
            var lower1_s = _cvParams.Lower1_S;
            var lower1_v = _cvParams.Lower1_V;
            var lower2_h = _cvParams.Lower2_H;
            var lower2_s = _cvParams.Lower2_S;
            var lower2_v = _cvParams.Lower2_V;
            var upper1_h = _cvParams.Upper1_H;
            var upper1_s = _cvParams.Upper1_S;
            var upper1_v = _cvParams.Upper1_V;
            var upper2_h = _cvParams.Upper2_H;
            var upper2_s = _cvParams.Upper2_S;
            var upper2_v = _cvParams.Upper2_V;
            var platformCenterX = _cvParams.PlatformCenterX;
            var platformCenterY = _cvParams.PlatformCenterY;
            var platformRadius = _cvParams.PlatformRadius;
            var size = _cvParams.Resolution;
            var setpointX = _pidParams.SetpointX;
            var setpointY = _pidParams.SetpointY;

            var platformColor = new Scalar(255, 0, 255);
            var setpointColor = new Scalar(255, 0, 0);
            var objectColor = new Scalar(0, 255, 0);
            var hsvObjectColor = new Scalar(0, 0, 0);


            //TODO Read camera frame
            //var actSource = testSource;
            Mat actSource = new Mat();
            cameraSource.NextFrame(actSource);

            var width = actSource.Width;
            var height = actSource.Height;
            var v = width < height ? width : height;

            var src = actSource[new Rect((width - v) / 2, (height - v) / 2, v, v)];

            if(size < v)
                src = src.Resize(new Size(size, size));

            //Convert to hsv color space
            var hsv = src.CvtColor(ColorConversionCodes.BGR2HSV);

            //TODO: apply circular mask

            //Create mask frame
            var m1 = hsv.InRange(new Scalar(lower1_h, lower1_s, lower1_v), new Scalar(upper1_h, upper1_s, upper1_v));
            var m2 = hsv.InRange(new Scalar(lower2_h, lower2_s, lower2_v), new Scalar(upper2_h, upper2_s, upper2_v));
            var mask = (m1 | m2).ToMat();
            mask = mask.Erode(InputArray.Create(Mat.Ones(5, 5)));
            mask = mask.Dilate(InputArray.Create(Mat.Ones(5, 5)));

            var cnts = mask.FindContoursAsMat(RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            //Draw platform
            src.Circle(new Point(platformCenterX, platformCenterY), (int)platformRadius, platformColor, 1);

            //Draw setpoint
            src.Circle(new Point(platformCenterX + setpointX, platformCenterY + setpointY), 3, setpointColor, 2);

            if (cnts.Length > 0) {
                //Find the contour with the biggest area
                var c = cnts.MaxBy(x => x.ContourArea());
                Point2f cent;
                float radius;
                c.MinEnclosingCircle(out cent, out radius);

                var moments = c.Moments();

                if (moments.M00 > 0) {
                    var center = new Point2d(moments.M10 / moments.M00, moments.M01 / moments.M00);

                    if (radius > 1) {
                        src.Circle((int)center.X, (int)center.Y, (int)radius, objectColor, 2);
                        src.Circle((int)center.X, (int)cent.Y, 2, objectColor, -1);
                        hsv.Circle((int)center.X, (int)center.Y, (int)radius, hsvObjectColor, 2);
                        hsv.Circle((int)center.X, (int)cent.Y, 2, hsvObjectColor, -1);

                        _cvRuntime.BallPosX = -(platformCenterX - center.X);
                        _cvRuntime.BallPosY = -(platformCenterY - center.Y);
                    }
                }
            }

            _cvRuntime.SrcFrame = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(src);
            _cvRuntime.HsvFrame = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(hsv);
            _cvRuntime.MaskFrame = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mask);

            await Task.Delay(delay);
        }
    }
}
