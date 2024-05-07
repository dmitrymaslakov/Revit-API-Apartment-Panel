namespace ApartmentPanel.Core.Services
{
    public class AnnotationNameBuilder
    {
        private string _folder;
        private string _name;

        public AnnotationNameBuilder AddFolders(params string[] folders)
        {
            _folder = string.Join("\\", folders);
            return this;
        }
        public AnnotationNameBuilder AddPartsOfName(string separator, params string[] partsOfName)
        {
            _name = string.Join(separator, partsOfName);
            return this;
        }
        public string Build()
        {
            return string.Join("\\", _folder, _name);
        }
    }
}
