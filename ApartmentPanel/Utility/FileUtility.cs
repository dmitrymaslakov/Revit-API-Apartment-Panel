using System.IO;
using System.Reflection;

namespace ApartmentPanel.Utility
{
    public class FileUtility
    {
        #region Data
        private static string sm_assemblyPath = null;
        private static string sm_assemblyFullName = null;
        private static string sm_appResourcesPath = null;
        private static string sm_appAnnotationsPath = null;
        #endregion

        public static string GetAssemblyFolder()
        {
            if (string.IsNullOrEmpty(sm_assemblyPath))
                sm_assemblyPath = Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location);
            return sm_assemblyPath;
        }
        public static string GetAssemblyFullName()
        {
            if (string.IsNullOrEmpty(sm_assemblyFullName))
                sm_assemblyFullName = Assembly.GetExecutingAssembly().Location;
            return sm_assemblyFullName;
        }
        public static string GetApplicationResourcesPath()
        {
            if (string.IsNullOrEmpty(sm_appResourcesPath))
                sm_appResourcesPath = GetAssemblyFolder() + "\\Resources";
            return sm_appResourcesPath;
        }
        public static string GetApplicationAnnotationsFolder()
        {
            if (string.IsNullOrEmpty(sm_appAnnotationsPath))
                sm_appAnnotationsPath = GetApplicationResourcesPath() + "\\Annotations";
            return sm_appAnnotationsPath;
        }
    }
}
