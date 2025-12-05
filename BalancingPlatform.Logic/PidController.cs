using BalancingPlatform.Logic.Models;
using BalancingPlatform.Logic.Models.Params;
using BalancingPlatform.Logic.Models.Runtime;
using SystBalancingPlatform.Logicem.Models.Runtime;
using System.Diagnostics;

namespace BalancingPlatform.Logic;
public class PidController {
    protected readonly PidParams _pidParams;
    protected readonly CvRuntime _cvRuntime;
    protected readonly PidRuntime _pidRuntime;

    private double prevErrorX;
    private double prevErrorY;
    private double integralX;
    private double integralY;
    private double prevOutputX;
    private double prevOutputY;

    public PidController(PidParams pidParams, PidRuntime pidRuntime, CvRuntime cvRuntime) {
        _pidParams = pidParams;
        _pidRuntime = pidRuntime;
        _cvRuntime = cvRuntime;
    }

    public async Task Run(CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            if (_pidParams.Dt <= 0)
                continue;

            //Read parameters
            double kp = _pidParams.Kp;
            double ki = _pidParams.Ki;
            double kd = _pidParams.Kd;
            double dt = _pidParams.Dt;
            double minOut = _pidParams.MinOutput;
            double maxOut = _pidParams.MaxOutput;
            double alpha = _pidParams.Alpha;

            //Read ballpos and setpoint
            double ballPosX = _cvRuntime.BallPosX;
            double ballPosY = _cvRuntime.BallPosY;
            double setpointX = _pidParams.SetpointX;
            double setpointY = _pidParams.SetpointY;

            //Calculate error
            double errorX = setpointX - ballPosX;
            double errorY = setpointY - ballPosY;

            //Calculate integral
            integralX += errorX * dt;
            integralY += errorY * dt;

            //Calculate derivative
            double derivativeX = (errorX - prevErrorX) / dt;
            double derivativeY = (errorY - prevErrorY) / dt;

            //Calculate output
            double outputX = kp * errorX + ki * integralX + kd * derivativeX;
            double outputY = kp * errorY + ki * integralY + kd * derivativeY;

            //Limit output
            outputX = outputX > maxOut ? maxOut : outputX < minOut ? minOut : outputX;
            outputY = outputY > maxOut ? maxOut : outputY < minOut ? minOut : outputY;

            //Apply filter
            outputX = alpha * outputX + (1 - alpha) * prevOutputX;
            outputY = alpha * outputY + (1 - alpha) * prevOutputY;

            //Save values to application storage
            _pidRuntime.OutputX = outputX;
            _pidRuntime.OutputY = outputY;
            _pidRuntime.ErrorX = errorX;
            _pidRuntime.ErrorY = errorY;

            //Save previous values
            prevOutputX = outputX;
            prevOutputY = outputY;
            prevErrorX = errorX;
            prevErrorY = errorY;

            await Task.Delay((int)(dt*1000));
        }
    }
}
