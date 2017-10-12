# AiForms.Renderes.SettingsView for Xamarin.Forms

This is a flexble TableView specialized in settings for Android / iOS.

## What SettingsView is capable of

### General

* To set separator color.
* To set selected cell color.

### Sections

* To set IsVisible each section.
* To set section a footer.
* To set various options of a header and  a footer.

### Cells

* To set options of all the cells as SettingsView options at once.
* To set indivisual cell options. (indivisual cell options  is superior to SettingsView options.)
* To set a cell HintText.
* To use an icon  cached in memory at all cells.
* To change corner radius of an icon.
* To use various defined cells.
* To use Xamarin.Forms.ViewCell and the others.
* To use SettingsView cells in ListView and TableView.

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
	* A color of out of region and entire region. They contains header, footer and cell (in case android).
* SeparatorColor
* SelectedColor
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
	* If HasUnevenRows is false, this value apply to each row height;
	* otherwise this value is used as minimum row height. 
* HasUnevenRows
	* Whether row height is fixed.
* CellTitleColor
* CellTitleFontSize
* CellValueTextColor
* CellValueTextFontSize
* CellDescriptionColor
* CellDescriptionFontSize
* CellBackgroundColor
* CellIconSize
* CellIconRadius
* CellAccentColor
* CellHintTextColor
* CellHintFontSize
* UseDescriptionAsValue (for Android)
	* Whether description field  is used as value field. (like general android app)
* ShowSectionTopBottomBorder (for Android)
	* Whether a separator is shown at section top and bottom. (like general android app)

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
