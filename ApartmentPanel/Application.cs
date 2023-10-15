using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using ApartmentPanel.Utility;
using ApartmentPanel.Presentation.View;
using System;
using Autodesk.Revit.DB.Events;
using ApartmentPanel.Presentation.ViewModel;
using ApartmentPanel.Infrastructure;
using ApartmentPanel.Core;
using ApartmentPanel.Presentation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ApartmentPanel
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        public static MainView View;
        public static MainViewModel _uiVM;
        internal static Application _thisApp = null;
        private static IHost _host;

        public Application()
        {
            try
            {
                _host = CreateHostBuilder().Build();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("host_Exception", ex.Message);
            }
        }

        /// <summary>
        /// Add UI for registering, showing, and hiding dockable panes.
        /// </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                _host.Start();

                application.ViewActivated += OnViewActivated;
                //CreateEventHandler();
                CreateWindow();

                if (!DockablePane.PaneIsRegistered(MainView.PaneId))
                    application.RegisterDockablePane(MainView.PaneId, MainView.PaneName, View);

                application.ControlledApplication.DocumentClosing +=
                    Handler_DocumentClosing;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("host_Exception", ex.Message);
            }
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
            /*var handler = new RequestHandler();
            var exEvent = ExternalEvent.Create(handler);*/

            /*_uiVM = new ViewModel(exEvent, handler);
            View = new APView.MainView(_uiVM);*/
            /*_uiVM = new MainViewModel(new ApartmentElementService(new ApartmentElementRepository(exEvent, handler)));
            View = new MainView(_uiVM);*/
            View = _host.Services.GetRequiredService<MainView>();
        }

        /*private void CreateEventHandler()
        {
            var handler = new RequestHandler();
            var exEvent = ExternalEvent.Create(handler);
        }*/

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
                _uiVM.ConfigPanelVM?.SaveLatestConfigCommand?.Execute(_uiVM.ConfigPanelVM);
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddDomainServices()
                .AddInfrastructureServices()
                .AddPresentationServices();
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

            if (DockablePane.PaneIsRegistered(MainView.PaneId))
            {
                DockablePane myCustomPane =
                    uiapp.GetDockablePane(MainView.PaneId);
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
