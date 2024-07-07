using System.IO;

namespace ApartmentPanel.Utility
{
    public class PathBuilder
    {
        private string _folder;
        private string _name;
        private readonly string _directorySeparator;
        private string _fileExtension; 

        public PathBuilder() => _directorySeparator = $"{Path.DirectorySeparatorChar}";

        public PathBuilder AddFolders(params string[] folders)
        {
            _folder = string.Join(_directorySeparator, folders);
            return this;
        }
        public PathBuilder AddPartsOfName(string nameSeparator, params string[] partsOfName)
        {
            _name = string.Join(nameSeparator, partsOfName);
            return this;
        }
        public PathBuilder AddJsonExtension()
        {
            _fileExtension = ".json"; 
            return this;
        }
        public PathBuilder AddPngExtension()
        {
            _fileExtension = ".png";
            return this;
        }

        public string Build() => string.Join(_directorySeparator, _folder, $"{_name}{_fileExtension}");
    }
}
