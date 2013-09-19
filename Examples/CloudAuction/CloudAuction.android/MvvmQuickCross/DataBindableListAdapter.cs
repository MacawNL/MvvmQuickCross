using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Android.Views;
using Android.Widget;
using System.Collections.Specialized;
using System;

namespace MvvmQuickCross
{
    public interface IDataBindableListAdapter
    {
        int GetItemPosition(object item);
        object GetItemAsObject(int position);
        void SetList(IList list);
        void AddHandlers();
        void RemoveHandlers();
    }

    public class DataBindableListAdapter<T> : BaseAdapter, IDataBindableListAdapter
    {
        private class ItemDataBinding
        {
            public readonly PropertyInfo ObjectPropertyInfo;
            public readonly FieldInfo ObjectFieldInfo;
            public readonly int ResourceId;

            public string Name { get { return (ObjectPropertyInfo != null) ? ObjectPropertyInfo.Name : ObjectFieldInfo.Name; } }
            public object GetValue(object item) { return (ObjectPropertyInfo != null) ? ObjectPropertyInfo.GetValue(item) : ObjectFieldInfo.GetValue(item); }

            public ItemDataBinding(PropertyInfo objectPropertyInfo, int resourceId)
            {
                this.ObjectPropertyInfo = objectPropertyInfo;
                this.ResourceId = resourceId;
            }

            public ItemDataBinding(FieldInfo objectFieldInfo, int resourceId)
            {
                this.ObjectFieldInfo = objectFieldInfo;
                this.ResourceId = resourceId;
            }
        }

        private readonly LayoutInflater layoutInflater;
        private readonly int itemTemplateResourceId;
        private IList list;
        private readonly int? itemValueResourceId;
        private readonly string idPrefix;
        private List<ItemDataBinding> itemDataBindings;

        public DataBindableListAdapter(LayoutInflater layoutInflater, int itemTemplateResourceId, string idPrefix, int? itemValueResourceId = null)
        {
            this.layoutInflater = layoutInflater;
            this.itemTemplateResourceId = itemTemplateResourceId;
            this.idPrefix = idPrefix;
            this.itemValueResourceId = itemValueResourceId;
        }

        private void AddListHandler()
        {
            if (list is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)list).CollectionChanged += DataBindableListAdapter_CollectionChanged;
            }
        }

        private void RemoveListHandler()
        {
            if (list is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)list).CollectionChanged -= DataBindableListAdapter_CollectionChanged;
            }
        }

        public virtual void AddHandlers() { AddListHandler(); }

        public virtual void RemoveHandlers() { RemoveListHandler(); }

        void DataBindableListAdapter_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged(); // TODO: Check if this should & can be optimized, see for details documentation at http://blog.stephencleary.com/2009/07/interpreting-notifycollectionchangedeve.html
        }

        public int GetItemPosition(object item)
        {
            return (list == null) ? -1 : list.IndexOf(item);
        }

        public object GetItemAsObject(int position)
        {
            return (list == null) ? null : list[position];
        }

        public void SetList(IList list)
        {
            RemoveListHandler();
            this.list = list;
            AddListHandler();
            NotifyDataSetChanged();
        }

        public override int Count
        {
            get { return (list == null) ? 0 : list.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position; // Bogus implementation required by BaseAdapter - nobody wants a Java object.
        }

        public override long GetItemId(int position)
        {
            return position; // Bogus implementation required by BaseAdapter - Id adds nothing to position
        }

        private string IdName(string name) { return idPrefix + name; }

        protected virtual void UpdateView(View view, object value)
        {
            ViewDataBindings.UpdateView(view, value);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var rootView = convertView ?? layoutInflater.Inflate(itemTemplateResourceId, parent, false);
            if (list != null)
            {
                if (itemValueResourceId.HasValue)
                {
                    UpdateView(rootView.FindViewById(itemValueResourceId.Value), list[position]);
                }
                else
                {
                    object itemObject = list[position];
                    if (itemObject != null)
                    {
                        EnsureBindings(itemObject);
                        foreach (var idb in itemDataBindings) UpdateView(rootView.FindViewById(idb.ResourceId), idb.GetValue(itemObject));
                    }
                }
            }
            return rootView;
        }

        private void EnsureBindings(object itemObject)
        {
            if (itemDataBindings == null)
            {
                itemDataBindings = new List<ItemDataBinding>();
                Type itemType = itemObject.GetType();

                foreach (var pi in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var resourceId = AndroidApplication.FindResourceId(IdName(pi.Name));
                    if (resourceId.HasValue) itemDataBindings.Add(new ItemDataBinding(pi, resourceId.Value));
                }

                foreach (var fi in itemType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    var resourceId = AndroidApplication.FindResourceId(IdName(fi.Name));
                    if (resourceId.HasValue) itemDataBindings.Add(new ItemDataBinding(fi, resourceId.Value));
                }
            }
        }
    }
}