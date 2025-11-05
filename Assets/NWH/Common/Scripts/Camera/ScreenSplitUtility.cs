using UnityEngine;

namespace NWH.Common.Cameras
{
    public static class ScreenSplitUtility
    {
        // Returns viewport rect for given player index and total players (supports 1-4)
        public static Rect GetSplitScreenRect(int index, int total)
        {
            switch (Mathf.Clamp(total, 1, 4))
            {
                case 1:
                    return new Rect(0f, 0f, 1f, 1f);
                case 2:
                    // 0 = top, 1 = bottom
                    return index == 0 ? new Rect(0f, 0.5f, 1f, 0.5f) : new Rect(0f, 0f, 1f, 0.5f);
                case 3:
                    if (index == 0) return new Rect(0f, 0.5f, 0.5f, 0.5f);
                    if (index == 1) return new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    return new Rect(0f, 0f, 1f, 0.5f);
                case 4:
                default:
                    if (index == 0) return new Rect(0f, 0.5f, 0.5f, 0.5f);
                    if (index == 1) return new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    if (index == 2) return new Rect(0f, 0f, 0.5f, 0.5f);
                    return new Rect(0.5f, 0f, 0.5f, 0.5f);
            }
        }
    }
}