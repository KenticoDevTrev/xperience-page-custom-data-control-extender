# Xperience Page Template Utilities

[![NuGet Package](https://img.shields.io/nuget/v/XperienceCommunity.PageCustomDataControlExtender.svg)](https://www.nuget.org/packages/XperienceCommunity.PageCustomDataControlExtender)

A Kentico Xperience Form Control Extender that sync the Form Control value to/from Page CustomData fields

## Dependencies

This package is compatible with Kentico Xperience 13.

## How to Use?

1. First, install the NuGet package in your Kentico Xperience administration `CMSApp` project

   ```bash
   dotnet add package XperienceCommunity.PageCustomDataControlExtender
   ```

1.

## How Does It Work?

Normally, if we want to store data in Pages, we define fields for the Page's custom Page Type. This field often a "Standard Field" which results in a column in the Page Type field database table.

However, Pages already have 2 fields that can be used to store any kind of serializable data/content - `DocumentCustomData` and `NodeCustomData`. Data in these fields is store in XML data structures and the `TreeNode` APIs to access these fields perform the XML serialization/deserialization automatically.

If we want to store data in these fields we need a way to associate a Form Control (eg Check Box, Text Box, Media Selector) with a one of these XML structures and an XML element name in which the value of the Form Control should be stored.

Kentico Xperience's Portal Engine API (still used in the Administration application) allows for Form Control Extender classes to get access to a Form Control's data, Page, and Form during events of the Control's event lifecycle.

This NuGet package exposes a Form Control Extender that can be applied to custom Form Controls that inherit from existing ones - maintaining their original functionality, but intercepting the Value produced by the Form Control and sync it to/from the Page's CustomData field.

Since the Page Type field that the extended Form Control operates on is a "Field without database representation", the Page Type database table does not need modified and Content Managers don't lose any of the standard content management functionality they are used to.

## References

### Kentico Xperience

- [Field Editor: Creating New Fields](https://docs.xperience.io/custom-development/extending-the-administration-interface/developing-form-controls/reference-field-editor#ReferenceFieldeditor-Creatingnewfields)
- [Inheriting from existing form controls](https://docs.xperience.io/custom-development/extending-the-administration-interface/developing-form-controls/inheriting-from-existing-form-controls)
- [Defining form control parameters](https://docs.xperience.io/custom-development/extending-the-administration-interface/developing-form-controls/defining-form-control-parameters)
