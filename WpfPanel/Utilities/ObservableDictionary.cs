﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPanel.Utilities
{
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            foreach (var item in Items)
            {
                if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
                {
                    return Remove(item);
                }
            }
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                foreach (var item in Items)
                {
                    if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
                    {
                        return item.Value;
                    }
                }
                throw new KeyNotFoundException();
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (EqualityComparer<TKey>.Default.Equals(Items[i].Key, key))
                    {
                        SetItem(i, new KeyValuePair<TKey, TValue>(key, value));
                        return;
                    }
                }
                Add(new KeyValuePair<TKey, TValue>(key, value));
            }
        }
        public bool ContainsKey(TKey key)
        {
            foreach (var item in Items)
            {
                if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
                {
                    return true;
                }
            }
            return false;
        }
    }
    /*public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                TValue oldValue;
                bool keyExists = TryGetValue(key, out oldValue);
                base[key] = value;

                if (keyExists)
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue));
                else
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
            }
        }

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, 
                new KeyValuePair<TKey, TValue>(key, value)));
        }

        public new bool Remove(TKey key)
        {
            TValue value;
            if (base.TryGetValue(key, out value) && base.Remove(key))
            {
                var removedItem = new KeyValuePair<TKey, TValue>(key, value);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, IndexOf(removedItem)));
                return true;
            }

            return false;
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private int IndexOf(KeyValuePair<TKey, TValue> item)
        {
            int index = 0;
            foreach (var pair in this)
            {
                if (EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(pair, item))
                    return index;
                index++;
            }
            return -1;
        }
    }*/
}