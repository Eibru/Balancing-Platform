using BalancingPlatform.Logic.Models.Params;
using System.Text.Json;

namespace BalancingPlatform.WEB;

public static class Utility {
    private static string CVPARAMS_PATH = "./CvParams.json";
    private static string PIDPARAMS_PATH = "./PidParams.json";
    private static string SERVOPARAMS_PATH = "./ServoParams.json";

    public static void SavePidParams(PidParams pidParams) {
        File.WriteAllText(PIDPARAMS_PATH, JsonSerializer.Serialize(pidParams));
    }

    public static void SaveCvParams(CvParams cvParams) {
        File.WriteAllText(CVPARAMS_PATH, JsonSerializer.Serialize(cvParams));
    }

    public static void SaveServoParams(ServoParams servoParams) {
        //TODO
    }

    public static PidParams ReadPidParams() {
        PidParams parms = null;

        try {
            var jsonStr = File.ReadAllText(PIDPARAMS_PATH);
            parms = JsonSerializer.Deserialize<PidParams>(jsonStr);
        } catch (Exception ex) { }

        if (parms == null) {
            parms = new PidParams {
                Dt = 1
            };
        }

        return parms;
    }

    public static CvParams ReadCvParams() {
        CvParams parms = null;
        try {
            var jsonStr = File.ReadAllText(CVPARAMS_PATH);
            parms = JsonSerializer.Deserialize<CvParams>(jsonStr);
        } catch (Exception ex) { }

        if (parms == null) {
            parms = new CvParams {
                SampleDelay = 30
            };
        }

        return parms;
    }

    public static ServoParams ReadServoParams() {
        ServoParams parms = null;
        try {
            var jsonStr = File.ReadAllText(SERVOPARAMS_PATH);
            parms = JsonSerializer.Deserialize<ServoParams>(jsonStr);
        } catch (Exception ex) { }

        if (parms == null) {
            parms = new ServoParams {
                L = 12,
                R = 12
            };
        }

        return parms;
    }
}
