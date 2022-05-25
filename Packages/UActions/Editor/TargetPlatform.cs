using System;

namespace UActions.Editor
{
    [Flags]
    public enum TargetPlatform
    {
        None = 0,
        Android = 1 << 0,
        iOS = 1 << 1,
        All = Int32.MaxValue,
    }
}