// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JObject
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Linq
{
  public class JObject : 
    JContainer,
    IDictionary<string, JToken>,
    ICollection<KeyValuePair<string, JToken>>,
    IEnumerable<KeyValuePair<string, JToken>>,
    IEnumerable,
    INotifyPropertyChanged,
    ICustomTypeDescriptor,
    INotifyPropertyChanging
  {
    private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

    protected override IList<JToken> ChildrenTokens => (IList<JToken>) this._properties;

    public event PropertyChangedEventHandler PropertyChanged;

    public event PropertyChangingEventHandler PropertyChanging;

    public JObject()
    {
    }

    public JObject(JObject other)
      : base((JContainer) other)
    {
    }

    public JObject(params object[] content)
      : this((object) content)
    {
    }

    public JObject(object content) => this.Add(content);

    internal override bool DeepEquals(JToken node) => node is JObject jobject && this._properties.Compare(jobject._properties);

    internal override int IndexOfItem(JToken item) => this._properties.IndexOfReference(item);

    internal override void InsertItem(int index, JToken item, bool skipParentCheck)
    {
      if (item != null && item.Type == JTokenType.Comment)
        return;
      base.InsertItem(index, item, skipParentCheck);
    }

    internal override void ValidateToken(JToken o, JToken existing)
    {
      ValidationUtils.ArgumentNotNull((object) o, nameof (o));
      JProperty jproperty1 = o.Type == JTokenType.Property ? (JProperty) o : throw new ArgumentException("Can not add {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType(), (object) this.GetType()));
      if (existing != null)
      {
        JProperty jproperty2 = (JProperty) existing;
        if (jproperty1.Name == jproperty2.Name)
          return;
      }
      if (this._properties.TryGetValue(jproperty1.Name, out existing))
        throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jproperty1.Name, (object) this.GetType()));
    }

    internal override void MergeItem(object content, JsonMergeSettings settings)
    {
      if (!(content is JObject jobject))
        return;
      foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
      {
        JProperty jproperty = this.Property(keyValuePair.Key);
        if (jproperty == null)
          this.Add(keyValuePair.Key, keyValuePair.Value);
        else if (keyValuePair.Value != null)
        {
          if (!(jproperty.Value is JContainer jcontainer) || jcontainer.Type != keyValuePair.Value.Type)
          {
            if (keyValuePair.Value.Type != JTokenType.Null || settings != null && settings.MergeNullValueHandling == MergeNullValueHandling.Merge)
              jproperty.Value = keyValuePair.Value;
          }
          else
            jcontainer.Merge((object) keyValuePair.Value, settings);
        }
      }
    }

    internal void InternalPropertyChanged(JProperty childProperty)
    {
      this.OnPropertyChanged(childProperty.Name);
      if (this._listChanged != null)
        this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.IndexOfItem((JToken) childProperty)));
      if (this._collectionChanged == null)
        return;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, (IList) childProperty, (IList) childProperty, this.IndexOfItem((JToken) childProperty)));
    }

    internal void InternalPropertyChanging(JProperty childProperty) => this.OnPropertyChanging(childProperty.Name);

    internal override JToken CloneToken() => (JToken) new JObject(this);

    public override JTokenType Type => JTokenType.Object;

    public IEnumerable<JProperty> Properties() => this._properties.Cast<JProperty>();

    public JProperty Property(string name)
    {
      if (name == null)
        return (JProperty) null;
      JToken jtoken;
      this._properties.TryGetValue(name, out jtoken);
      return (JProperty) jtoken;
    }

    public JEnumerable<JToken> PropertyValues() => new JEnumerable<JToken>(this.Properties().Select<JProperty, JToken>((Func<JProperty, JToken>) (p => p.Value)));

    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        return key is string propertyName ? this[propertyName] : throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, nameof (key));
        if (!(key is string propertyName))
          throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this[propertyName] = value;
      }
    }

    public JToken this[string propertyName]
    {
      get
      {
        ValidationUtils.ArgumentNotNull((object) propertyName, nameof (propertyName));
        return this.Property(propertyName)?.Value;
      }
      set
      {
        JProperty jproperty = this.Property(propertyName);
        if (jproperty != null)
        {
          jproperty.Value = value;
        }
        else
        {
          this.OnPropertyChanging(propertyName);
          this.Add((object) new JProperty(propertyName, (object) value));
          this.OnPropertyChanged(propertyName);
        }
      }
    }

    public static JObject Load(JsonReader reader) => JObject.Load(reader, (JsonLoadSettings) null);

    public static JObject Load(JsonReader reader, JsonLoadSettings settings)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
      reader.MoveToContent();
      if (reader.TokenType != JsonToken.StartObject)
        throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JObject jobject = new JObject();
      jobject.SetLineInfo(reader as IJsonLineInfo, settings);
      jobject.ReadTokenFrom(reader, settings);
      return jobject;
    }

    public static JObject Parse(string json) => JObject.Parse(json, (JsonLoadSettings) null);

    public static JObject Parse(string json, JsonLoadSettings settings)
    {
      using (JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json)))
      {
        JObject jobject = JObject.Load(reader, settings);
        do
          ;
        while (reader.Read());
        return jobject;
      }
    }

    public static JObject FromObject(object o) => JObject.FromObject(o, JsonSerializer.CreateDefault());

    public static JObject FromObject(object o, JsonSerializer jsonSerializer)
    {
      JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
      return jtoken == null || jtoken.Type == JTokenType.Object ? (JObject) jtoken : throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) jtoken.Type));
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartObject();
      for (int index = 0; index < this._properties.Count; ++index)
        this._properties[index].WriteTo(writer, converters);
      writer.WriteEndObject();
    }

    public JToken GetValue(string propertyName) => this.GetValue(propertyName, StringComparison.Ordinal);

    public JToken GetValue(string propertyName, StringComparison comparison)
    {
      if (propertyName == null)
        return (JToken) null;
      JProperty jproperty = this.Property(propertyName);
      if (jproperty != null)
        return jproperty.Value;
      if (comparison != StringComparison.Ordinal)
      {
        foreach (JProperty property in (Collection<JToken>) this._properties)
        {
          if (string.Equals(property.Name, propertyName, comparison))
            return property.Value;
        }
      }
      return (JToken) null;
    }

    public bool TryGetValue(string propertyName, StringComparison comparison, out JToken value)
    {
      value = this.GetValue(propertyName, comparison);
      return value != null;
    }

    public void Add(string propertyName, JToken value) => this.Add((object) new JProperty(propertyName, (object) value));

    bool IDictionary<string, JToken>.ContainsKey(string key) => this._properties.Contains(key);

    ICollection<string> IDictionary<string, JToken>.Keys => this._properties.Keys;

    public bool Remove(string propertyName)
    {
      JProperty jproperty = this.Property(propertyName);
      if (jproperty == null)
        return false;
      jproperty.Remove();
      return true;
    }

    public bool TryGetValue(string propertyName, out JToken value)
    {
      JProperty jproperty = this.Property(propertyName);
      if (jproperty == null)
      {
        value = (JToken) null;
        return false;
      }
      value = jproperty.Value;
      return true;
    }

    ICollection<JToken> IDictionary<string, JToken>.Values => throw new NotImplementedException();

    void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item) => this.Add((object) new JProperty(item.Key, (object) item.Value));

    void ICollection<KeyValuePair<string, JToken>>.Clear() => this.RemoveAll();

    bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
    {
      JProperty jproperty = this.Property(item.Key);
      return jproperty != null && jproperty.Value == item.Value;
    }

    void ICollection<KeyValuePair<string, JToken>>.CopyTo(
      KeyValuePair<string, JToken>[] array,
      int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), "arrayIndex is less than 0.");
      if (arrayIndex >= array.Length && arrayIndex != 0)
        throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
      if (this.Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
      int num = 0;
      foreach (JProperty property in (Collection<JToken>) this._properties)
      {
        array[arrayIndex + num] = new KeyValuePair<string, JToken>(property.Name, property.Value);
        ++num;
      }
    }

    bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly => false;

    bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
    {
      if (!((ICollection<KeyValuePair<string, JToken>>) this).Contains(item))
        return false;
      this.Remove(item.Key);
      return true;
    }

    internal override int GetDeepHashCode() => this.ContentsHashCode();

    public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
    {
      foreach (JProperty property in (Collection<JToken>) this._properties)
        yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void OnPropertyChanging(string propertyName)
    {
      PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
      if (propertyChanging == null)
        return;
      propertyChanging((object) this, new PropertyChangingEventArgs(propertyName));
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => ((ICustomTypeDescriptor) this).GetProperties((Attribute[]) null);

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
    {
      PropertyDescriptorCollection properties = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
      foreach (KeyValuePair<string, JToken> keyValuePair in this)
        properties.Add((PropertyDescriptor) new JPropertyDescriptor(keyValuePair.Key));
      return properties;
    }

    AttributeCollection ICustomTypeDescriptor.GetAttributes() => AttributeCollection.Empty;

    string ICustomTypeDescriptor.GetClassName() => (string) null;

    string ICustomTypeDescriptor.GetComponentName() => (string) null;

    TypeConverter ICustomTypeDescriptor.GetConverter() => new TypeConverter();

    EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() => (EventDescriptor) null;

    PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() => (PropertyDescriptor) null;

    object ICustomTypeDescriptor.GetEditor(System.Type editorBaseType) => (object) null;

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) => EventDescriptorCollection.Empty;

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents() => EventDescriptorCollection.Empty;

    object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) => (object) null;

    protected override DynamicMetaObject GetMetaObject(Expression parameter) => (DynamicMetaObject) new DynamicProxyMetaObject<JObject>(parameter, this, (DynamicProxy<JObject>) new JObject.JObjectDynamicProxy());

    private class JObjectDynamicProxy : DynamicProxy<JObject>
    {
      public override bool TryGetMember(
        JObject instance,
        GetMemberBinder binder,
        out object result)
      {
        result = (object) instance[binder.Name];
        return true;
      }

      public override bool TrySetMember(JObject instance, SetMemberBinder binder, object value)
      {
        if (!(value is JToken jtoken))
          jtoken = (JToken) new JValue(value);
        instance[binder.Name] = jtoken;
        return true;
      }

      public override IEnumerable<string> GetDynamicMemberNames(JObject instance) => instance.Properties().Select<JProperty, string>((Func<JProperty, string>) (p => p.Name));
    }
  }
}
