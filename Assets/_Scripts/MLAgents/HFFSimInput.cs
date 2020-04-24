using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;

/// <summary>
/// Simulate keyboard and mouse input by going through the native Windows OS layer.
/// Independent of Unity, and Windows-only.
/// This is now based on the ViGEm bus tool, so the corresponding system driver needs
/// to be installed. (Installer is in repository.)
/// This approach still requires the editor's game window to be in focus for it to work!
/// So using it for Agent training means the user is blocked from using the machine
/// for anything else.
/// TODO: can we "capture" the gamepad, so it sends input even if window is inactive
/// </summary>

// HISTORY:
// I previously tried to use Unity's experimental new InputSystem to queue events
// but those events didn't register with the HFF character.
// Then I successfully used the approach found here:
// https://answers.unity.com/questions/564664/how-i-can-move-mouse-cursor-without-mouse-but-with.html
// then used the package InputSimulator https://github.com/michaelnoonan/inputsimulator/
// which is similar but uses the modern API not a deprecated one and adds keyboard input.
// However, both of those are problematic as the input isn't restricted to the current process or window,
// so using this could mean accidentally clicking anything on the screen!
// This could be worked around by capturing the mouse cursor, but simulating a gamepad seemed safer.


public class HFFSimInput {
    static ViGEmClient vigemClient = new ViGEmClient();
    private Xbox360Controller xboxController = new Xbox360Controller(vigemClient);

    public void ConnectController() {
        xboxController.Connect();
        xboxController.AutoSubmitReport = false;
    }

    public void DisconnectController() {
        xboxController.Disconnect();
        vigemClient.Dispose();
    }

    public void SubmitInput() {
        xboxController.SubmitReport();
    }

    // Functions corresponding to the Input Manager mapping in Unity:

    // Jump is Button A
    public void JumpSet(bool state) {
        xboxController.SetButtonState(Xbox360Button.A, state);
    }

    // LeftGrab is LT
    public void LeftGrabSet(bool state)
    {
        xboxController.SetSliderValue(Xbox360Slider.LeftTrigger, (byte) (state ? 0xFF : 0x00));
    }

    // RightGrab is RT
    public void RightGrabSet(bool state) {
        xboxController.SetSliderValue(Xbox360Slider.RightTrigger, (byte) (state ? 0xFF : 0x00));
    }

    // LookHorizontal is right stick
    // LookVertical is right stick
    public void Look(short whereX, short whereY) {
        xboxController.SetAxisValue(Xbox360Axis.RightThumbX, whereX);
        xboxController.SetAxisValue(Xbox360Axis.RightThumbY, whereY);
    }

    // WalkHorizontal is left stick
    // WalkVertical is left stick
    public void Walk(short whereX, short whereY) {
        xboxController.SetAxisValue(Xbox360Axis.LeftThumbX, whereX);
        xboxController.SetAxisValue(Xbox360Axis.LeftThumbY, whereY);
    }

}
