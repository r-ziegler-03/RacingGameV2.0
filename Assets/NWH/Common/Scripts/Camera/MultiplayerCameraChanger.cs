using UnityEngine;

namespace NWH.Common.Cameras
{
    public class MultiplayerCameraChanger : CameraChanger
    {
        [Tooltip("Which split-screen slot this vehicle will use (0..n-1).")]
        public int playerIndex = 0;

        /// <summary>
        /// Force the currently selected camera for this vehicle to be active and set its viewport rect
        /// according to the provided totalPlayers. This does not try to disable cameras on other vehicles.
        /// </summary>
        public void ForceEnable(int totalPlayers)
        {
            if (cameras == null || cameras.Count == 0)
            {
                return;
            }

            // Make sure currentCameraIndex is in range
            currentCameraIndex = Mathf.Clamp(currentCameraIndex, 0, cameras.Count - 1);

            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i] == null) continue;

                // Activate only the currently selected camera for this vehicle
                cameras[i].SetActive(i == currentCameraIndex);

                var cam = cameras[i].GetComponent<Camera>();
                if (cam != null)
                {
                    cam.rect = ScreenSplitUtility.GetSplitScreenRect(playerIndex, totalPlayers);

                    // Ensure other cameras in scene aren't left with enabled audio listeners.
                    var al = cam.GetComponent<AudioListener>();
                    if (al != null)
                    {
                        // only enable audio listener for the first player (optional)
                        al.enabled = (playerIndex == 0);
                    }
                }
            }
        }
    }
}