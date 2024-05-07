using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfTest.Commands;
using System.IO;
using System.Text.RegularExpressions;
using Utility;
using System.Text;
using System.Windows;
using WpfTest.Models;
using WpfTest.Utility;

namespace WpfTest.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        #region MockFields
        #endregion

        #region MockProps
        #endregion

        public MainViewModel()
        {
        }
        public ObservableCollection<string> List1 { get; set; } = new ObservableCollection<string>
        (
            Enumerable.Range(1, 3).Select(i => $"String-{i}")
        );
        public ObservableCollection<string> List2 { get; set; } = new ObservableCollection<string>
(
    Enumerable.Range(1, 3).Select(i => $"OtherString-{i}")
);

        private RelayCommand findFiles;

        public ICommand FindFiles
        {
            get
            {
                if (findFiles == null)
                {
                    findFiles = new RelayCommand(PerformFindFiles);
                }

                return findFiles;
            }
        }

        public ObservableCollection<ApartmentElement> AllElements { get; set; } = new ObservableCollection<ApartmentElement>
            (
                Enumerable.Range(1, 20).Select(i => 
                new ApartmentElement 
                { 
                    Name = $"Name {i}" ,
                    Category = StaticData.GetCategory(i),
                    Family = StaticData.GetFamily(i),
                })
            );
        /*private ObservableCollection<ApartmentElement> _allElements;
        public ObservableCollection<ApartmentElement> AllElements
        {
            get => _allElements;
            set => Set(ref _allElements, value);
        }*/

        private void PerformFindFiles(object commandParameter)
        {
            bool b = TryFindConfigs(out ObservableCollection<string> configs);
            if (b)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in configs)
                {
                    stringBuilder.Append(item);
                }
                MessageBox.Show(stringBuilder.ToString());
            }
        }
        private bool TryFindConfigs(out ObservableCollection<string> configs)
        {
            configs = null;
            string assemblyPath = FileUtility.GetAssemblyPath();
            var files = Directory.GetFiles(assemblyPath, "*.json").Select(Path.GetFileName);

            string pattern = @"LatestConfig\.json$";

            foreach (var file in files)
            {
                if (Regex.IsMatch(file, pattern))
                {
                    if (configs == null) configs = new ObservableCollection<string> { file };
                    else configs.Add(file);
                }
            }
            return configs != null;
        }
    }
}
