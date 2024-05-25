using System.IO;

namespace MegaPint.Editor.Scripts
{

/// <summary> Partial lookup table for constants containing PlayModeStartScene values  </summary>
internal static partial class Constants
{
    public static class PlayModeStartScene
    {
        private static readonly string s_basePath = Path.Combine("MegaPint", "PlayModeStartScene");

        private static readonly string s_resourcesPath = Path.Combine(s_basePath, "Resources");
        
        public static class Resources
        {
            private static readonly string s_userInterfacePath = Path.Combine(s_resourcesPath, "User Interface");
            
            public static class UserInterface
            {
                public static readonly string WindowsPath = Path.Combine(s_userInterfacePath, "Windows");
            }
        }
    }
}

}
