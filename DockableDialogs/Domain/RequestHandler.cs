using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using DockableDialogs.Utility;
using DockableDialogs.ViewModel.ComponentsVM;
using DockableDialogs.View.Components;

namespace DockableDialogs.Domain
{
    public class RequestHandler : IExternalEventHandler
    {
        public Request Request { get; } = new Request();
        public object Props { get; set; }

        public void Execute(UIApplication uiapp)
        {
            switch (Request.Take())
            {
                case RequestId.None:
                    {
                        return;
                    }
                case RequestId.Configure:
                    {
                        ShowEditPanel();
                        break;
                    }
                case RequestId.Insert:
                    {
                        InsertElement(uiapp);
                        break;
                    }
                case RequestId.AddElement:
                    {
                        AddElement(uiapp);
                        break;
                    }
                case RequestId.EditElement:
                    {
                        break;
                    }
                case RequestId.RemoveElement:
                    {
                        break;
                    }
                case RequestId.AddCircuit:
                    {
                        break;
                    }
                case RequestId.EditCircuit:
                    {
                        break;
                    }
                case RequestId.RemoveCircuit:
                    {
                        break;
                    }
            }

            return;
        }
        private void ShowEditPanel()
        {
            var editPanelVM = Props as EditPanelVM;
            new EditPanel(editPanelVM).ShowDialog();
            Props = null;
        }

        private void AddElement(UIApplication uiapp)
        {
            var addElement = Props as Action<FamilySymbol>;
            new ListElements(addElement).ShowDialog();
            Props = null;
        }

        private void InsertElement(UIApplication uiapp)
        {
            
            UIDocument uiDocument = uiapp.ActiveUIDocument;
            Document document = uiDocument.Document;
            Selection selection = uiDocument.Selection;
            try
            {
                var elementData = Props as Dictionary<string, string>;
                string elementName = elementData[nameof(elementName)];
                string elementCategory = elementData[nameof(elementCategory)];
                string circuit = elementData[nameof(circuit)];
                string switchHeight = elementData[nameof(switchHeight)];
                string socketHeight = elementData[nameof(socketHeight)];
                string lampSuffix = elementData[nameof(lampSuffix)];

                switch (elementCategory)
                {
                    case StaticData.LIGHTING_FIXTURES:
                        new FamilyInstanceBuilder(uiapp)
                            .WithCircuit(circuit)
                            .WithCurrentLevel()
                            .WithLampSuffix(lampSuffix)
                            .Build(elementName);
                        break;
                    case StaticData.LIGHTING_DEVICES:
                        new FamilyInstanceBuilder(uiapp)
                            .WithElevationFromLevel(switchHeight)
                            .WithCircuit(circuit)
                            .WithSwitchSuffixes()
                            .WithCurrentLevel()
                            .Build(elementName);
                        break;
                    case StaticData.ELECTRICAL_FIXTURES:
                    case StaticData.TELEPHONE_DEVICES:
                    case StaticData.FIRE_ALARM_DEVICES:
                    case StaticData.COMMUNICATION_DEVICES:
                        new FamilyInstanceBuilder(uiapp)
                            .WithElevationFromLevel(socketHeight)
                            .WithCircuit(circuit)
                            .WithCurrentLevel()
                            .Build(elementName);
                        break;
                    default:
                        break;
                }
                Props = null;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }

        public string GetName()
        {
            return "Placement triss";
        }
    }
}
