using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.FileDataAccess.Models;
using ApartmentPanel.FileDataAccess.Services.FileCommunicator.Interfaces;
using ApartmentPanel.Utility.AnnotationUtility;
using ApartmentPanel.Utility.AnnotationUtility.FileAnnotationService;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ApartmentPanel.FileDataAccess.Services.FileCommunicator
{
    internal class FileDbModelReader : IDbModelReader, IDisposable
    {
        private bool _disposed;
        private readonly string _fullPath;

        public FileDbModelReader(string fullPath) => _fullPath = fullPath;

        public void Dispose() => _disposed = true;

        public FileDbModel Get()
        {
            if (string.IsNullOrEmpty(_fullPath)) return null;

            string json = File.ReadAllText(_fullPath);
            var dbModel = JsonSerializer.Deserialize<FileDbModel>(json);
            var dbName = Path.GetFileNameWithoutExtension(_fullPath);

            if (dbModel.ApartmentElements != null)
                foreach (var apartmentElement in dbModel.ApartmentElements)
                {
                    var annotationPath = FilePathService.GetElementAnnotationPath(apartmentElement, dbName);
                    var annotationService = new AnnotationService(
                        new FileAnnotationCommunicatorFactory(annotationPath));
                    apartmentElement.Annotation = annotationService.IsAnnotationExists()
                        ? annotationService.Get() : null;
                };
            if (dbModel.ElementBatches != null)
                foreach (var batch in dbModel.ElementBatches)
            {
                var annotationPath = FilePathService.GetBatchAnnotationPath(batch, dbName);
                var annotationService = new AnnotationService(
                    new FileAnnotationCommunicatorFactory(annotationPath));
                batch.Annotation = annotationService.IsAnnotationExists()
                    ? annotationService.Get() : null;

                foreach (var row in batch.BatchedRows)
                {
                    foreach (BatchedElement element in row.RowElements)
                    {
                        var batchedElementPath = FilePathService.GetElementAnnotationPath(element, dbName);
                        var batchedAnnotationService = new AnnotationService(
                            new FileAnnotationCommunicatorFactory(batchedElementPath));
                        element.Annotation = batchedAnnotationService.IsAnnotationExists()
                            ? batchedAnnotationService.Get() : null;
                    }
                }
            }

            //List<Circuit> newLC = new List<Circuit>();
            if (dbModel.Circuits != null)
                foreach (var circuit in dbModel.Circuits)
            {
                var circuitElements = circuit.Elements;
                foreach (var apartmentElement in dbModel.ApartmentElements)
                {
                    var matchingCircuitElement = circuitElements
                        .FirstOrDefault(c => c.Name == apartmentElement.Name && c.Family == apartmentElement.Family);
                    if (matchingCircuitElement != null)
                    {
                        matchingCircuitElement.Annotation = apartmentElement.Annotation;
                        //apartmentElement.AnnotationChanged += matchingCircuitElement.AnnotationChanged_Handler;
                    }
                }
                //newLC.Add(new Circuit { Number = circuit.Number, Elements = circuitElements });
            }
            //dbModel.Circuits = newLC;
            return dbModel;
            /*for (int i = 0; i < dbModel.Circuits.Count; i++)
            {
                var listCircuits = dbModel.Circuits.ToList();
                var circuitElements = listCircuits[i].Elements;
                //var circuitElements = PanelCircuitsVM.PanelCircuits[i].Elements;

                foreach (var apartmentElement in dbModel.ApartmentElements)
                {
                    var matchingCircuitElement = circuitElements
                        .FirstOrDefault(c => c.Name == apartmentElement.Name && c.Family == apartmentElement.Family);

                    if (matchingCircuitElement != null)
                    {
                        matchingCircuitElement.Annotation = apartmentElement.Annotation;
                        apartmentElement.AnnotationChanged += matchingCircuitElement.AnnotationChanged_Handler;
                    }
                }
                listCircuits[i] =
                    new Circuit { Number = listCircuits[i].Number, Elements = circuitElements };
            }*/
        }
    }
}