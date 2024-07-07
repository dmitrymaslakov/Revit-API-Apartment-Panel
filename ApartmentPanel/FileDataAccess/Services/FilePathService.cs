using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.Utility;

namespace ApartmentPanel.FileDataAccess.Services
{
    internal class FilePathService
    {
        private static readonly string _dbNameSuffix = "LatestConfig";
        private static readonly string _emptySeparator = "";
        private static readonly string _deshSeparator = "-";
        private static readonly string _assemblyFolder = FileUtility.GetAssemblyFolder();
        private static readonly string _annotationFolder = FileUtility.GetApplicationAnnotationsFolder();
        
        public static string GetFileDbPath(string fileDbName)
        {
            return new PathBuilder()
                .AddFolders(_assemblyFolder)
                .AddPartsOfName(_emptySeparator, fileDbName, _dbNameSuffix)
                .AddJsonExtension()
                .Build();
        }
        public static string GetElementAnnotationPath(ApartmentElement apartmentElement, string dbName)
        {
            string dbNameWithoutSuffix = dbName.Replace(_dbNameSuffix, string.Empty);
            return new PathBuilder()
                    .AddFolders(_annotationFolder, dbNameWithoutSuffix)
                    .AddPartsOfName(_deshSeparator, apartmentElement.Family, apartmentElement.Name)
                    .AddPngExtension()
                    .Build();
        }
        public static string GetElementAnnotationPath(BatchedElement apartmentElement, string dbName)
        {
            return new PathBuilder()
                    .AddFolders(_annotationFolder, dbName)
                    .AddPartsOfName(_deshSeparator, apartmentElement.Family, apartmentElement.Name)
                    .AddPngExtension()
                    .Build();
        }

        public static string GetBatchAnnotationPath(ElementBatch batch, string dbName)
        {
            return new PathBuilder()
                    .AddFolders(dbName)
                    .AddPartsOfName(_emptySeparator, batch.Name)
                    .AddPngExtension()
                    .Build();
        }

    }
}
