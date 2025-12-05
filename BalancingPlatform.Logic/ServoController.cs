using System;
using System.Collections.Generic;
using System.Linq;
using BalancingPlatform.Logic.Models.Params;
using BalancingPlatform.Logic.Models.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace BalancingPlatform.Logic;
public class ServoController {
    protected readonly ServoParams _kinematicsParams;
    protected readonly ServoRuntime _kinematicsRuntime;
    protected readonly PidRuntime _pidRuntime;

    public ServoController(ServoParams kinematicsParams, ServoRuntime kinematicsRuntime, PidRuntime pidRuntime) {
        _kinematicsParams = kinematicsParams;
        _kinematicsRuntime = kinematicsRuntime;
        _pidRuntime = pidRuntime;
    }

    public async Task Run(CancellationToken cancellationToken) {
        while (!cancellationToken.IsCancellationRequested) {
            double l = _kinematicsParams.L;
            double r = _kinematicsParams.R;

            double roll = _pidRuntime.OutputX * Math.PI / 180;
            double pitch = _pidRuntime.OutputY * Math.PI / 180;

            double z0 = ((Math.Sqrt(3) * l) / 6) * Math.Sin(pitch) * Math.Cos(roll) + (l / 2) * Math.Sin(roll);
            double z1 = ((Math.Sqrt(3) * l) / 6) * Math.Sin(pitch) * Math.Cos(roll) - (l / 2) * Math.Sin(roll);
            double z2 = ((-Math.Sqrt(3) * l) / 3) * Math.Sin(pitch) * Math.Cos(roll);
            double s0 = 155 - (Math.Asin(z0 / r)) * Math.PI / 180;
            double s1 = 150 - (Math.Asin(z1 / r)) * Math.PI / 180;
            double s2 = 150 - (Math.Asin(z2 / r)) * Math.PI / 180;

            _kinematicsRuntime.ServoAngle1 = s0;
            _kinematicsRuntime.ServoAngle2 = s1;
            _kinematicsRuntime.ServoAngle3 = s2;

            await Task.Delay(100);
        }
    }
}
