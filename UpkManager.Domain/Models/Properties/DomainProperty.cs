﻿using System;
using System.Threading.Tasks;

using UpkManager.Domain.Constants;
using UpkManager.Domain.Helpers;
using UpkManager.Domain.Models.Tables;


namespace UpkManager.Domain.Models.Properties {

  public class DomainProperty {

    #region Constructor

    public DomainProperty() {
      NameIndex = new DomainNameTableIndex();

      TypeNameIndex = new DomainNameTableIndex();
    }

    #endregion Constructor

    #region Properties

    public DomainNameTableIndex NameIndex { get; set; }

    public DomainNameTableIndex TypeNameIndex { get; set; }

    public int Size { get; set; }

    public int ArrayIndex { get; set; }

    public DomainPropertyValueBase Value { get; set; }

    #endregion Properties

    #region Domain Methods

    public async Task ReadProperty(ByteArrayReader reader, DomainHeader header) {
      await Task.Run(() => NameIndex.ReadNameTableIndex(reader, header));

      if (NameIndex.Name == ObjectType.None.ToString()) return;

      await Task.Run(() => TypeNameIndex.ReadNameTableIndex(reader, header));

      Size       = reader.ReadInt32();
      ArrayIndex = reader.ReadInt32();

      Value = propertyValueFactory();

      await Value.ReadPropertyValue(reader, Size, header);
    }

    #endregion Domain Methods

    #region Private Methods

    private DomainPropertyValueBase propertyValueFactory() {
      PropertyType type;

      Enum.TryParse(TypeNameIndex?.Name, true, out type);

      switch(type) {
        case PropertyType.BoolProperty:      return new DomainPropertyBoolValue();
        case PropertyType.IntProperty:       return new DomainPropertyIntValue();
        case PropertyType.FloatProperty:     return new DomainPropertyFloatValue();
        case PropertyType.ObjectProperty:    return new DomainPropertyObjectValue();
        case PropertyType.InterfaceProperty: return new DomainPropertyInterfaceValue();
        case PropertyType.ComponentProperty: return new DomainPropertyComponentValue();
        case PropertyType.ClassProperty:     return new DomainPropertyClassValue();
        case PropertyType.GuidProperty:      return new DomainPropertyGuidValue();
        case PropertyType.NameProperty:      return new DomainPropertyNameValue();
        case PropertyType.ByteProperty:      return new DomainPropertyByteValue();
        case PropertyType.StrProperty:       return new DomainPropertyStrValue();
        case PropertyType.StructProperty:    return new DomainPropertyStructValue();
        case PropertyType.ArrayProperty:     return new DomainPropertyArrayValue();

        default: return new DomainPropertyValueBase();
      }
    }

    #endregion Private Methods

  }

}
