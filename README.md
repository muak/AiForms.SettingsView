# AiForms.Renderes.SettingsView for Xamarin.Forms

This is a flexble TableView specialized in settings for Android / iOS.

## What SettingsView is capable of

### General

* To set separator color.

### Sections

* To set IsVisible each section.
* To set section a footer.
* To set various options of a header and  a footer.

### Cells

* To set options of all the cells as options of SettingsView at once.
* To set options of indivisual cell. (indivisual cell options  is superior to SettingsView options.)
* To set a cell HintText.
* To use a icon  cached in memory at all cells.
* To use various defined cells.
* To use Xamarin.Forms.ViewCell and the others.
* To use the cells of SettingsView in ListView and TableView.

## Minimum Device and Version etc

iOS:iPhone5s,iPod touch6,iOS9.3  
Android:version 5.1.1 (only FormsAppcompatActivity) / API22

## Nuget Installation

```bash
Install-Package AiForms.SettingsView
```

You need to install this nuget package to PCL project and each platform project.

### for iOS project

To use by iOS, you need to write some code in AppDelegate.cs.

```csharp
public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
    global::Xamarin.Forms.Forms.Init();

    AiForms.Renderers.iOS.SettingsViewInit.Init(); //need to write here

    LoadApplication(new App(new iOSInitializer()));

    return base.FinishedLaunching(app, options);
}
```

## How to use

hogehoge...

## Properties of SettingsView

* BackgroundColor
* SeparatorColor
* HeaderPadding
* HeaderTextColor
* HeaderFontSize
* HeaderTextVerticalAlign
* HeaderBackgroundColor
* HeaderHeight
* FooterTextColor
* FooterFontSize
* FooterBackgroundColor
* FooterPadding
* RowHeight
* HasUnevenRows
* CellTitleColor
* CellTitleFontSize
* CellValueTextColor
* CellValueTextFontSize
* CellDescriptionColor
* CellDescriptionFontSize
* CellBackgroundColor
* CellIconSize
* CellAccentColor
* CellHintTextColor
* CellHintFontSize

## Method of SettingsView

* ClearCache (static)
	* clear all memory cache.

## Properties of Section

* Title
	* same Xamarin.Forms.TableSection.
* FooterText
	* Section footer text.
* IsVisible
	* whether the section is visibled.

## Cells

* [CellBase](#CellBase)
* [LabelCell](#labelcell)
* [CommandCell](#commandcell)
* [SwitchCell](#switchcell)
* [CheckboxCell](#checkboxcell)
* [NumberPickerCell](#numberpickercell)
* [TimePickerCell](#timepickercell)
* [DatePickerCell](#datepickercell)
* [PickerCell](#pickercell)
* [EntryCell](#entrycell)



## License

MIT Licensed.
