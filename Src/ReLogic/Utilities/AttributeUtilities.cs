// Decompiled with JetBrains decompiler
// Type: ReLogic.Utilities.AttributeUtilities
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Linq;

namespace ReLogic.Utilities
{
  public static class AttributeUtilities
  {
      public static T GetAttribute<T>(this Enum value) where T : Attribute
      {
        Type type = value.GetType();

        //RnD
        return default;//type.GetField(Enum.GetName(type, (object) value)).GetCustomAttributes(false).OfType<T>().SingleOrDefault<T>();
      }

        public static A GetCacheableAttribute<T, A>() where A : Attribute
        {
            return AttributeUtilities.TypeAttributeCache<T, A>.Value;
        }

        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            //RnD
            return default;//type.GetCustomAttributes(false).OfType<T>().SingleOrDefault<T>();
        }

        private static class TypeAttributeCache<T, A> where A : Attribute
        {
         public static readonly A Value = typeof (T).GetAttribute<A>();
        }
  }
}
