using System;

namespace MvvmQuickCross
{
    public static class AndroidApplication
    {
        public enum Category { Drawable, Layout, Menu, Values, Id };

        private static Type resourceClassType;

        public static void Initialize(Type resourceClassType) { AndroidApplication.resourceClassType = resourceClassType; }

        public static int? FindResourceId(string name, Category category = Category.Id)
        {
            if (string.IsNullOrEmpty(name) || resourceClassType == null) return null;
            var categoryClassType = resourceClassType.GetNestedType(category.ToString()); // TODO: check if optimize perf by caching type for each category is needed?
            if (categoryClassType == null) return null;
            var fieldInfo = categoryClassType.GetField(name);
            if (fieldInfo == null) return null;
            return (int)fieldInfo.GetValue(null);
        }
    }
}