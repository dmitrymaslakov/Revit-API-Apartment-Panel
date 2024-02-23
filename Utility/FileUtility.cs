using System.Reflection;

namespace Utility
{
    public class FileUtility
    {
        #region Data
        private static string sm_assemblyPath = null;
        private static string sm_assemblyFullName = null;
        private static string sm_appResourcesPath = null;
        private static string sm_appAnnotationsPath = null;
        #endregion

        public static string GetAssemblyPath()
        {
            if (string.IsNullOrEmpty(sm_assemblyPath))
                sm_assemblyPath = System.IO.Path.GetDirectoryName(
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
                sm_appResourcesPath = GetAssemblyPath() + "\\Resources";
            return sm_appResourcesPath;
        }
        public static string GetApplicationAnnotationsPath()
        {
            if (string.IsNullOrEmpty(sm_appAnnotationsPath))
                sm_appAnnotationsPath = GetApplicationResourcesPath() + "\\Annotations";
            return sm_appAnnotationsPath;
        }
    }
}
