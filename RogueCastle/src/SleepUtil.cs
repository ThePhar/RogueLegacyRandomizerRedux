using System;
using System.Runtime.InteropServices;

namespace RogueCastle;

public static class SleepUtil {
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

    public static void DisableScreensaver() {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
            return;
        }

        if (SetThreadExecutionState(ExecutionState.Continuous
                                    | ExecutionState.DisplayRequired
                                    | ExecutionState.SystemRequired
                                    | ExecutionState.AwayModeRequired) == 0) //Away mode for Windows >= Vista
        {
            SetThreadExecutionState(ExecutionState.Continuous
                                    | ExecutionState.DisplayRequired
                                    | ExecutionState.SystemRequired);
        }
    }

    public static void EnableScreensaver() {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT) {
            return;
        }

        SetThreadExecutionState(ExecutionState.Continuous);
    }

    [Flags]
    private enum ExecutionState : uint {
        AwayModeRequired = 0x00000040,
        Continuous = 0x80000000,
        DisplayRequired = 0x00000002,
        SystemRequired = 0x00000001,
    }
}
