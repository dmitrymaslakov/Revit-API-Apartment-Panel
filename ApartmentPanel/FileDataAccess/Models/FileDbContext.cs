using ApartmentPanel.Core.Models;
using ApartmentPanel.Core.Models.Batch;
using ApartmentPanel.FileDataAccess.Services;
using ApartmentPanel.Infrastructure.Interfaces.DataAccess;
using ApartmentPanel.Utility.Extensions;
using System.Collections.Generic;

namespace ApartmentPanel.FileDataAccess.Models
{
    internal class FileDbContext : FileDbModel
    {
        private FileDatabase _batabase;

        public FileDbContext(FileDbProvider dbProvider)
        {
            _batabase = dbProvider.UseFileDatabase();
            InitializeDbContext();
        }

        private void InitializeDbContext()
        {
            var dbModel = _batabase.GetModel();
            ApartmentElements = dbModel?.ApartmentElements ?? new List<ApartmentElement>();
            ElementBatches = dbModel?.ElementBatches ?? new List<ElementBatch>();
            Circuits = dbModel?.Circuits ?? new List<Circuit>();
            Heights = dbModel?.Heights ?? new List<Height>();
            ResponsibleForCircuits = dbModel?.ResponsibleForCircuits ?? new List<string>();
            ResponsibleForHeights = dbModel?.ResponsibleForHeights ?? new List<string>();
        }

        public ICollection<T> Set<T>()
        {
            var props = GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.PropertyType.Equals(typeof(ICollection<T>)))
                    return prop.GetValue(this) as ICollection<T>;
            }
            return null;
        }
        public ICollection<T> Set<T>(string propertyName)
        {
            var props = GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.PropertyType.Equals(typeof(ICollection<T>)) && string.Equals(prop.Name, propertyName))
                    return prop.GetValue(this) as ICollection<T>;
            }
            return null;
        }

        public bool CreateDatabase(string dbName) => _batabase.Create(dbName);
        public bool SaveChanges() => _batabase.SaveChanges(this);

        public IDbContext GetDbContext() => _batabase.GetModel();

        internal void UpdateContext(FileDbProvider dbProvider)
        {
            _batabase = dbProvider.UseFileDatabase();
            var dbModel = _batabase.GetModel();
            ApartmentElements.Clear();
            if (dbModel.ApartmentElements is ICollection<ApartmentElement> ae)
                ApartmentElements.AddRange(ae);
            ElementBatches.Clear();
            if (dbModel.ElementBatches is ICollection<ElementBatch> eb)
                ElementBatches.AddRange(eb);
            Circuits.Clear();
            if (dbModel.Circuits is ICollection<Circuit> c)
                Circuits.AddRange(c);
            Heights.Clear();
            if (dbModel.Heights is ICollection<Height> h)
                Heights.AddRange(h);
            ResponsibleForCircuits.Clear();
            if (dbModel.ResponsibleForCircuits is ICollection<string> rfc)
                ResponsibleForCircuits.AddRange(rfc);
            ResponsibleForHeights.Clear();
            if (dbModel.ResponsibleForHeights is ICollection<string> rfh)
                ResponsibleForHeights.AddRange(rfh);
        }
    }
}
