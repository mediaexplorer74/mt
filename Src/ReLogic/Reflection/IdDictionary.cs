// Decompiled with JetBrains decompiler
// Type: ReLogic.Reflection.IdDictionary
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReLogic.Reflection
{
  public class IdDictionary
  {
    private Dictionary<string, int> _nameToId = new Dictionary<string, int>();
    private Dictionary<int, string> _idToName;
    public readonly int Count;

    private IdDictionary(int count) => this.Count = count;

    public bool TryGetName(int id, out string name) => this._idToName.TryGetValue(id, out name);

    public bool TryGetId(string name, out int id) => this._nameToId.TryGetValue(name, out id);

    public bool ContainsName(string name) => this._nameToId.ContainsKey(name);

    public bool ContainsId(int id) => this._idToName.ContainsKey(id);

    public string GetName(int id) => this._idToName[id];

    public int GetId(string name) => this._nameToId[name];

    public void Add(string name, int id)
    {
      this._idToName.Add(id, name);
      this._nameToId.Add(name, id);
    }

    public void Remove(string name)
    {
      this._idToName.Remove(this._nameToId[name]);
      this._nameToId.Remove(name);
    }

    public void Remove(int id)
    {
      this._nameToId.Remove(this._idToName[id]);
      this._idToName.Remove(id);
    }

    public static IdDictionary Create(Type idClass, Type idType)
    {
      int count = int.MaxValue;
      FieldInfo fieldInfo = ((IEnumerable<FieldInfo>) idClass.GetFields()).FirstOrDefault<FieldInfo>((Func<FieldInfo, bool>) (field => field.Name == "Count"));
      if (fieldInfo != (FieldInfo) null)
        count = Convert.ToInt32(fieldInfo.GetValue((object) null));
      IdDictionary dictionary = new IdDictionary(count);
      ((IEnumerable<FieldInfo>) idClass.GetFields(BindingFlags.Static | BindingFlags.Public)).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.FieldType == idType)).ToList<FieldInfo>().ForEach((Action<FieldInfo>) (field =>
      {
        int int32 = Convert.ToInt32(field.GetValue((object) null));
        if (int32 >= dictionary.Count)
          return;
        dictionary._nameToId.Add(field.Name, int32);
      }));
      dictionary._idToName = dictionary._nameToId.ToDictionary<KeyValuePair<string, int>, int, string>((Func<KeyValuePair<string, int>, int>) (kp => kp.Value), (Func<KeyValuePair<string, int>, string>) (kp => kp.Key));
      return dictionary;
    }

    public static IdDictionary Create<IdClass, IdType>() => IdDictionary.Create(typeof (IdClass), typeof (IdType));
  }
}
