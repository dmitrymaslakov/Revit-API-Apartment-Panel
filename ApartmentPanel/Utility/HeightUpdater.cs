using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentPanel.Utility
{
    public class HeightUpdater : IUpdater
    {
        private AddInId _appId;
        private UpdaterId _updaterId;

        public HeightUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("FBFBF6B2-4C06-42d4-97C1-D1B4EB593EFF"));
        }
        
        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();
            var familyInstances = data
                        .GetModifiedElementIds()
                        .Select(doc.GetElement)
                        .Cast<FamilyInstance>();

            foreach (var familyInstance in familyInstances)
            {
                string value = familyInstance.LookupParameter("Elevation from Level").AsValueString();
                familyInstance.LookupParameter("H-UK").Set(value); //UK-HEIGHT
            }
        }

        public string GetAdditionalInformation()
        {
            return "ElectricalFixture updater: updates UK-HEIGHT parameter";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.MEPFixtures;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }

        public string GetUpdaterName()
        {
            return "UK-HEIGHT parameter Updater";
        }
    }
}
