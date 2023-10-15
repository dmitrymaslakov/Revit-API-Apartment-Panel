using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfPanel.View.Components;
using WpfPanel.ViewModel.ComponentsVM;

namespace WpfPanel.Utilities.NewFolder1
{
    public class Ser
    {
        private readonly MasterClass _masterClass;
        private readonly string _path = @"e:\Different\Study\Programming\C-sharp\Revit-API\Apartment-Panel\MasterClass.json";
        private readonly JsonSerializerOptions _options;

        public Ser()
        {
            var elementCollection = new List<IElement>
            {
                new ElementA{ AValue = "AValue1", Key = "AKey1"},
                new ElementA{ AValue = "AValue2", Key = "AKey2"},
                //new ElementB{ BValue = "BValue", Key = "BKey"},
            };
            _masterClass = new MasterClass { ElementCollection = elementCollection };
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new TypeMappingConverter<IElement, ElementA>(),
                }
            };
        }

        public void ExecuteSer()
        {            
            string json = JsonSerializer.Serialize(_masterClass, _options);
            File.WriteAllText(_path, json);
        }
        public void ExecuteDeser()
        {
            if (File.Exists(_path))
            {
                string json = File.ReadAllText(_path);
                var deso = JsonSerializer.Deserialize<MasterClass>(json, _options);
            }
        }
    }
}
