using System;
using System.Collections.Generic;
using System.Reflection;

using Android.Views;
using Android.Widget;

namespace MvvmQuickCross
{
    public interface DataBindableAdapter // TODO: maybe eliminate? But good for extensibility
    {
        int GetItemPosition(object item);
        object GetItemAsObject(int position);
    }

    public class DataBindableToStringListAdapter<T> : DataBindableListAdapter<T>
    {
        public DataBindableToStringListAdapter(LayoutInflater layoutInflater, int viewResourceId, int objectValueResourceId, IList<T> objects, string idPrefix = null)
            : base(layoutInflater, viewResourceId, objectValueResourceId, objects, idPrefix)
        { }

        protected override void UpdateView(View view, T value)
        {
            ViewDataBindings.UpdateView(view, value.ToString());
        }
    }

    public class DataBindableListAdapter<T> : BaseAdapter, DataBindableAdapter
    {
        private class ItemDataBinding
        {
            public readonly PropertyInfo ObjectPropertyInfo;
            public readonly FieldInfo ObjectFieldInfo;
            public readonly int ResourceId;

            public string Name { get { return (ObjectPropertyInfo != null) ? ObjectPropertyInfo.Name : ObjectFieldInfo.Name; } }
            public object GetValue(T item) { return (ObjectPropertyInfo != null) ? ObjectPropertyInfo.GetValue(item) : ObjectFieldInfo.GetValue(item); }

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

        private readonly Type resourceIdType;
        private readonly LayoutInflater layoutInflater;
        private readonly int viewResourceId;
        protected readonly IList<T> objects;
        private readonly int? objectValueResourceId;
        private readonly string idPrefix;
        private readonly List<ItemDataBinding> itemDataBindings;

        private DataBindableListAdapter(LayoutInflater layoutInflater, int viewResourceId, IList<T> objects, Type resourceIdType = null, int? objectValueResourceId = null, string idPrefix = null)
        {
            this.layoutInflater = layoutInflater;
            this.viewResourceId = viewResourceId;
            this.objects = objects;
            this.objectValueResourceId = objectValueResourceId;
            this.idPrefix = idPrefix ?? this.GetType().Name;

            if (!objectValueResourceId.HasValue)
            {
                itemDataBindings = new List<ItemDataBinding>();

                foreach (var pi in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var resourceId = ResourceId(IdName(pi.Name));
                    if (resourceId.HasValue) itemDataBindings.Add(new ItemDataBinding(pi, resourceId.Value));
                }

                foreach (var fi in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    var resourceId = ResourceId(IdName(fi.Name));
                    if (resourceId.HasValue) itemDataBindings.Add(new ItemDataBinding(fi, resourceId.Value));
                }
            }
        }

        public DataBindableListAdapter(LayoutInflater layoutInflater, int viewResourceId, int objectValueResourceId, IList<T> objects, string idPrefix = null)
            : this(layoutInflater, viewResourceId, objects, null, objectValueResourceId, idPrefix)
        { }

        public DataBindableListAdapter(LayoutInflater layoutInflater, int viewResourceId, Type resourceIdType, IList<T> objects, string idPrefix = null)
            : this(layoutInflater, viewResourceId, objects, resourceIdType, null, idPrefix)
        { }

        public int GetItemPosition(object item)
        {
            return objects.IndexOf((T)item);
        }

        public object GetItemAsObject(int position)
        {
            return objects[position];
        }

        public override int Count
        {
            get { return objects.Count; }
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

        private int? ResourceId(string resourceName)
        {
            var fieldInfo = resourceIdType.GetField(IdName(resourceName));
            if (fieldInfo == null) return null;
            return (int)fieldInfo.GetValue(null);
        }

        protected virtual void UpdateView(View view, T value)
        {
            ViewDataBindings.UpdateView(view, value);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var rootView = convertView ?? layoutInflater.Inflate(viewResourceId, parent, false);
            if (objectValueResourceId.HasValue)
            {
                UpdateView(rootView.FindViewById(objectValueResourceId.Value), objects[position]);
            } else {
                foreach (var idb in itemDataBindings) UpdateView(rootView.FindViewById(idb.ResourceId), (T)idb.GetValue(objects[position]));
            }
            return rootView;
        }
    }
}