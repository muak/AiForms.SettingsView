<link href="https://raw.githubusercontent.com/muak/AiForms.Renderers/master/css/readme.css" rel="stylesheet"></link>

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

## SettingsView Properties

* BackgroundColor
	* A color of out of region and entire region. They contains header, footer and cell (in case android).
* SeparatorColor
    * row separator color.
* SelectedColor
    * backgraound color when row is selected.
* HeaderPadding
* HeaderTextColor
* HeaderFontSize
* HeaderTextVerticalAlign
* HeaderBackgroundColor
* HeaderHeight
    * they are section header options.
* FooterTextColor
* FooterFontSize
* FooterBackgroundColor
* FooterPadding
    * they are section footer options.
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
    * they are bulk cell options.
* UseDescriptionAsValue (for Android)
	* Whether description field  is used as value field. (like general android app)
    * default true
* ShowSectionTopBottomBorder (for Android)
	* Whether a separator is shown at section top and bottom. (like general android app)
    * default false

## SettingsView Methods

* ClearCache (static)
	* clear all memory cache.

## Section Properties

* Title
	* the same Xamarin.Forms.TableSection.
* FooterText
	* Section footer text.
* IsVisible
	* whether the section is visibled.

## Cells

* [CellBase](#cellbase)
* [LabelCell](#labelcell)
* [CommandCell](#commandcell)
* [SwitchCell](#switchcell)
* [CheckboxCell](#checkboxcell)
* [NumberPickerCell](#numberpickercell)
* [TimePickerCell](#timepickercell)
* [DatePickerCell](#datepickercell)
* [PickerCell](#pickercell)
* [EntryCell](#entrycell)
* [ButtonCell](#buttoncell)

## CellBase

### Layout of cellbase

<div id="outerbox" style="display:flex;width:360px;height:100px;border:solid 1px silver"> 
    <div id="iconbox" style="display:flex;align-items:center;justify-content:center;width:100px;height:100px;border-right:solid 1px silver;">Icon</div>
    <div id="middlebox" style="display:flex;flex-direction:column;flex-grow:100;">
        <div id="middletopbox" style="display:flex;align-items:center;height:50px;border-bottom:solid 1px silver;">
            <div id="titlebox" style="display:flex;padding-left:6px;">Title</div>
            <div id="valuebox" style="display:flex;flex-grow:100;justify-content:flex-end;padding-right:6px;">ValueText</div>            
        </div>
        <div id="descriptionbox" style="display:flex;align-items:center;height:50px;padding-left:6px;">
            Description
        </div>
    </div>
    <div id="accessorybox" style="display:flex;align-items:center;justify-cntent:center;width:30px;height:100;border-left:solid 1px silver;padding:3px;">
        Accessory
    </div>
</div>
<br/>

* Icon
    * If not specify a imagesource, icon will be hidden.
* Description
    * If not specify any text, description will be hidden.
* Accessory
    * Be used by a CheckboxCell and  a SwitchCell; Otherwise will be hidden. 

### Properties

* 

## License

MIT Licensed.
