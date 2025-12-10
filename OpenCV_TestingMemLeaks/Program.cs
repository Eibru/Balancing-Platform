

using OpenCvSharp;
using System.Threading;

var platformColor = new Scalar(255, 0, 255);
var setpointColor = new Scalar(255, 0, 0);
var objectColor = new Scalar(0, 255, 0);
var hsvObjectColor = new Scalar(0, 0, 0);

//Initialize camera
var cameraSource = Cv2.CreateFrameSource_Camera(0);

var element = InputArray.Create(Mat.Ones(5, 5));

long lastGc = 0;

while (true) {
    //Read params
    var lower1_h = 0;
    var lower1_s = 0;
    var lower1_v = 0;
    var lower2_h = 0;
    var lower2_s = 0;
    var lower2_v = 0;
    var upper1_h = 0;
    var upper1_s = 0;
    var upper1_v = 0;
    var upper2_h = 0;
    var upper2_s = 0;
    var upper2_v = 0;
    var platformCenterX = 0;
    var platformCenterY = 0;
    var platformRadius = 0;
    var size = 600;
    var setpointX = 0;
    var setpointY = 0;
    var ballPosX = 0.0;
    var ballPosY = 0.0;

    using(var actSrc = new Mat()) {
        //Read camera frame
        cameraSource.NextFrame(actSrc);

        var width = actSrc.Width;
        var height = actSrc.Height;
        var v = width < height ? width : height;

        using (var src = actSrc[new Rect((width - v) / 2, (height - v) / 2, v, v)].Resize(new Size(size < v ? size : v, size < v ? size : v))) {

            //Convert to hsv color space
            using (var hsv = src.CvtColor(ColorConversionCodes.BGR2HSV)) {

                //TODO: apply circular mask

                //Create mask frame
                using (var m1 = hsv.InRange(new Scalar(lower1_h, lower1_s, lower1_v), new Scalar(upper1_h, upper1_s, upper1_v))) {
                    using (var m2 = hsv.InRange(new Scalar(lower2_h, lower2_s, lower2_v), new Scalar(upper2_h, upper2_s, upper2_v))) {
                        using (var mask = (m1 | m2).ToMat().Erode(element).Dilate(element)) {
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

                                        ballPosX = -(platformCenterX - center.X);
                                        ballPosY = -(platformCenterY - center.Y);
                                    }
                                }
                            }

                            await Task.Delay(10);
                        }
                    }
                }
            }
        }
    }

    

    if(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastGc >= 10000) {
        GC.Collect();
        lastGc = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    //dispose all Mats
    actSource.Dispose();
    src.Dispose();
    hsv.Dispose();
    m1.Dispose();
    m2.Dispose();
    mask.Dispose();

    foreach (var cnt in cnts) {
        cnt.Dispose();
    }

    //GC.Collect();

    
}