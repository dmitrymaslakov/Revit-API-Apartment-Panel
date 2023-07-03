using System;
using System.Windows;
using WpfPanel.Domain.Models.RevitMockupModels;
using WpfPanel.View.Components;
using WpfPanel.ViewModel.ComponentsVM;

namespace WpfPanel.Domain
{
    public class RequestHandler
    {
        public Request Request { get; } = new Request();
        public object Props { get; set; }

        public void Execute()
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
                case RequestId.AddElement:
                    {
                        AddElement();
                        break;
                    }
                case RequestId.EditElement:
                    {
                        EditElement();
                        break;
                    }
                case RequestId.RemoveElement:
                    {
                        RemoveElement();
                        break;
                    }
                case RequestId.AddCircuit:
                    {
                        AddCircuit();
                        break;
                    }
                case RequestId.EditCircuit:
                    {
                        EditCircuit();
                        break;
                    }
                case RequestId.RemoveCircuit:
                    {
                        RemoveCircuit();
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

        private void AddElement()
        {
            var addElement = Props as Action<FamilySymbol>;
            new ListElements(addElement).ShowDialog();
            Props = null;
        }

        private void EditElement()
        {
            MessageBox.Show("Edit element");
        }

        private void RemoveElement()
        {
            MessageBox.Show("Remove element");
        }

        private void AddCircuit()
        {
            MessageBox.Show("Add circuit");
        }

        private void EditCircuit()
        {
            MessageBox.Show("Edit circuit");
        }
        
        private void RemoveCircuit()
        {
            MessageBox.Show("Remove circuit");
        }
    }
}
