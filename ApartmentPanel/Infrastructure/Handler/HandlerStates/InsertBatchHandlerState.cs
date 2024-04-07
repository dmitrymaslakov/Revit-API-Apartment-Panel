using ApartmentPanel.Core.Infrastructure.Interfaces.DTO;
using ApartmentPanel.Infrastructure.Models;
using Autodesk.Revit.UI;

namespace ApartmentPanel.Infrastructure.Handler.HandlerStates
{
    internal class InsertBatchHandlerState : HandlerState
    {
        internal override void Handle(UIApplication uiapp)
        {
            SetInfrastructure(uiapp);
            InsertBatch();
        }
        private void InsertBatch()
        {
            var batchData = _handler.Props as InsertBatchDTO;
            new BatchInstaller(_uiapp, batchData).Install();
        }
    }
}
