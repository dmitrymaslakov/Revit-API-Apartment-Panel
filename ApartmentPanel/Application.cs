using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using ApartmentPanel.Utility;
using APView = ApartmentPanel.Presentation.View;
using System;
using Autodesk.Revit.DB.Events;
using ApartmentPanel.Presentation.ViewModel;
using ApartmentPanel.Infrastructure;

namespace ApartmentPanel
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        public static APView.View View;
        public static Presentation.ViewModel.ViewModel _uiVM;
        internal static Application _thisApp = null;

        public Application()
        {
        }

        /// <summary>
        /// Add UI for registering, showing, and hiding dockable panes.
        /// </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            application.ViewActivated += OnViewActivated;
            CreateWindow();

            if (!DockablePane.PaneIsRegistered(APView.View.PaneId))
                application.RegisterDockablePane(APView.View.PaneId, APView.View.PaneName, View);

            application.ControlledApplication.DocumentClosing +=
                Handler_DocumentClosing;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            try
            {

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Application_ShutDown_Exception", ex.Message);
            }
            finally
            {
                application.ControlledApplication.DocumentClosing -=
                Handler_DocumentClosing;
            }
            return Result.Succeeded;
        }

        private void OnViewActivated(object sender, ViewActivatedEventArgs e)
        {
            if (e.Document == null)
                return;
        }
        /// <summary>
        /// Create the new WPF Page that Revit will dock.
        /// </summary>
        private void CreateWindow()
        {
            RequestHandler handler = new RequestHandler();
            ExternalEvent exEvent = ExternalEvent.Create(handler);
            _uiVM = new ViewModel(exEvent, handler);
            View = new APView.View(_uiVM);
        }

        private void Handler_DocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Handler_DocumentClosing_Exception", ex.Message);
            }
            finally
            {
                _uiVM.EditPanelVM?.SaveLatestConfigCommand?.Execute(_uiVM.EditPanelVM);
            }
        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        public virtual Result Execute(ExternalCommandData commandData,
            ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;

            if (DockablePane.PaneIsRegistered(APView.View.PaneId))
            {
                DockablePane myCustomPane =
                    uiapp.GetDockablePane(APView.View.PaneId);
                myCustomPane.Show();
            }
            else
            {
                return Result.Failed;
            }

            var heightUpdater = new HeightUpdater(uiapp.ActiveAddInId);
            UpdaterRegistry.RegisterUpdater(heightUpdater);

            Document _document = uiapp.ActiveUIDocument.Document;

            double newElevationFeets = UnitUtils.ConvertToInternalUnits(40.0,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());

            FilterRule elevationFromLevelRule = ParameterFilterRuleFactory
                .CreateEqualsRule(new ElementId(BuiltInParameter.INSTANCE_ELEVATION_PARAM),
                newElevationFeets, 0.01);

            var elementFilter = new LogicalAndFilter(new ElementFilter[]
                {
                    new ElementIsElementTypeFilter(true),
                    new ElementParameterFilter(elevationFromLevelRule)
                });

            UpdaterRegistry.AddTrigger(
                heightUpdater.GetUpdaterId(), elementFilter,
                Element.GetChangeTypeParameter(
                    new ElementId(BuiltInParameter.INSTANCE_ELEVATION_PARAM)));

            return Result.Succeeded;
        }
    }
}
