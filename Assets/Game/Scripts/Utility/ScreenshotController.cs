using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenshotController : MonoBehaviour
{
#if UNITY_EDITOR

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            Capture();
        }
    }

    private void Capture()
    {
        string directory = Path.Combine(
            Application.dataPath,
            "Game",
            "Generated",
            "ScreenImage"
        );

        Directory.CreateDirectory(directory);

        string fileName =
            $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss_fff}.png";

        string filePath = Path.Combine(directory, fileName);

        ScreenCapture.CaptureScreenshot(filePath);

        Debug.Log($"Screenshot requested: {filePath}");
    }
#endif
}
