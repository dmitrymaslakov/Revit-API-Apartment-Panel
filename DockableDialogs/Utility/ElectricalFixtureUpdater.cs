﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockableDialogs.Utility
{
    public class ElectricalFixtureUpdater : IUpdater
    {
        private AddInId _appId;
        private UpdaterId _updaterId;

        public ElectricalFixtureUpdater(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("FBFBF6B2-4C06-42d4-97C1-D1B4EB593EFF"));
        }
        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();
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
