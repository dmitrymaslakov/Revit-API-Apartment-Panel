using ApartmentPanel.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ApartmentPanel.Infrastructure.Models
{
    public class BatchedInstanceRow : RevitInfrastructureBase, IEnumerable
    {
        BatchedInstance _head; // головной/первый элемент
        BatchedInstance _tail; // последний/хвостовой элемент
        int _count;

        public BatchedInstanceRow(UIApplication uiapp) : base(uiapp) { }

        public int Count { get { return _count; } }
        public bool IsEmpty { get { return _count == 0; } }

        public double GetOffset()
        {
            double offset = 0;
            IEnumerator enumerator = GetEnumerator();
            bool isHead = true;
            while (enumerator.MoveNext())
            {
                var batchedInstance = (BatchedInstance)enumerator.Current;
                offset += GetOffsetFromFamilyInstance(batchedInstance, isHead);
                isHead = false;
            }
            return offset;
        }
        public void Add(BatchedInstance batchedInstance)
        {
            if (_head == null)
                _head = batchedInstance;
            else
                _tail.Next = batchedInstance;
            _tail = batchedInstance;

            _count++;
        }

        public bool Remove(BatchedInstance batchedInstance)
        {
            BatchedInstance current = _head;
            BatchedInstance previous = null;

            while (current != null && current.Instance != null)
            {
                if (current.Instance.Id.Equals(batchedInstance.Instance.Id))
                {
                    // Если узел в середине или в конце
                    if (previous != null)
                    {
                        // убираем узел current, теперь previous ссылается не на current, а на current.Next
                        previous.Next = current.Next;

                        // Если current.Next не установлен, значит узел последний,
                        // изменяем переменную tail
                        if (current.Next == null)
                            _tail = previous;
                    }
                    else
                    {
                        // если удаляется первый элемент
                        // переустанавливаем значение head
                        _head = _head.Next;

                        // если после удаления список пуст, сбрасываем tail
                        if (_head == null)
                            _tail = null;
                    }
                    _count--;
                    return true;
                }

                previous = current;
                current = current.Next;
            }
            return false;
        }
        // очистка списка
        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }
        // содержит ли список элемент
        public bool Contains(BatchedInstance batchedInstance)
        {
            BatchedInstance current = _head;
            while (current != null && current.Instance != null)
            {
                if (current.Instance.Id.Equals(batchedInstance.Instance.Id)) return true;
                current = current.Next;
            }
            return false;
        }
        // добвление в начало
        public void AppendFirst(BatchedInstance batchedInstance)
        {
            batchedInstance.Next = _head;
            _head = batchedInstance;
            if (_count == 0)
                _tail = _head;
            _count++;
        }
        public IEnumerator GetEnumerator()
        {
            BatchedInstance current = _head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private double GetOffsetFromFamilyInstance(BatchedInstance batchedInstance, bool isHead)
        {
            var poinCounter = new FamilyInstacePointCounter(_uiapp, batchedInstance.Instance);
            var (basePoint, maxPoint, minPoint) = (poinCounter.Location, poinCounter.Max, poinCounter.Min);
            double instanceWidth;
            
            if (isHead)
                instanceWidth = Math.Abs(basePoint.X - maxPoint.X);
            else
                instanceWidth = Math.Abs(minPoint.X - maxPoint.X);

            double leftMarginInFeets = UnitUtils.ConvertToInternalUnits(batchedInstance.Margin.Left,
                _document.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId());
            return instanceWidth + leftMarginInFeets;
        }
    }
}
