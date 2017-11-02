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
    * default false
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
* [ButtonCell](#buttoncell)
* [SwitchCell](#switchcell)
* [CheckboxCell](#checkboxcell)
* [NumberPickerCell](#numberpickercell)
* [TimePickerCell](#timepickercell)
* [DatePickerCell](#datepickercell)
* [PickerCell](#pickercell)
* [EntryCell](#entrycell)


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

### Properties (all cell types)

* Title
    * Title text.
* TitleColor
    * Title text color.
* TitleFontSize
    * Title text font size.
* Description
    * Description text.
* DescriptionColor
    * Description text color.
* DescriptionFontSize
    * Description text font size.
* HintText
    * Hint text.(for some information, validation error and so on)
* HintTextColor
    * Hint text color.
* HintFontSize
    * Hint text font size.
* BackgroundColor
    * Cell background color.
* IconSource
    * Icon image source. (any ImageSource object)
* IconSize
    * Icon size. (width,height)
* IconRadius
    * Icon corners radius.

## LabelCell

This is a cell showing read only text.

### Properties

* ValueText
    * Value text.
* ValueTextColor
    * Value text color.
* ValueTextFontSize
    * Value text font size.
* IgnoreUseDescriptionAsValue
    * Whether ignore the setting that SettingsView property of UseDescriptionAsValue.

## CommandCell

This is a Labelcell invoked an action.

### Properties

* Command
    * Invoked action.
* CommandParameter
* KeepSelectedUntilBack
    * When moving next page, whether keep the cell selected until being back to the page.

the others are the same as LabelText.

## ButtonCell

This is a simple cell invoked an action like a button. 

### Properties

* TitleAlignment
    * Button title horizontal alignment.
* Command
* CommandParameter

This cell don't use Description property.

## SwitchCell

This is a LabelCell equipped a switch.

### Properties

* On
    * Switch toggle on / off. On is true, Off is false. 
* AccentColor
    * Swich accent color. (background color and so on)

## CheckboxCell

This is a LabelCell equipped a checkbox.

### Properties

* Checked
    * Check on / off. On is true, Off is false.
* AccentColor
    * Checkbox accent color. (frame and background)

## NumberPickerCell

This is a LabelCell calling a number picker.

### Properties

* Number
    * Current number.(default two way binding)
* Min
    * Minimum number.
* Max
    * Maximum number.
* PickerTitle
    * Picker title text.
* SelectedCommand
    * A command invoked when a number is selected.

This cell can't use ValueText propertiy.

## TimePickerCell

This is a LabelCell calling a time picker.

### Properties

* Time
    * Current time (default two way binding)
* Format
    * Time format. (for example "hh:mm")
* PickerTitle
    * Picker title text.

This cell can't use ValueText propertiy.

## DatePickerCell

This is a LabelCell calling a date picker.

### Properties

* Date
    * Current date. (default two way binding)
* MinimumDate
* MaximumDate
* Format
    * Date format. (for example "ddd MMM d yyyy")
* TodayText
    * Text of the button selecting  today's date. (only iOS)
    * If this text is empty, the button will be hidden.

This cell can't use ValueText propertiy.

## PickerCell

This is a LabelCell calling a multiple select picker.
When tapped on iOS, move next page and show picker there.
When tapped on Android, show the picker on a dialog. 

### Properties

* PageTitle
    * Picker page title text.
* ItemsSource
    * Picker data source implementing IEnumerable.
    * This have to assing a instance and must not null.
* DisplayMember
    * Class member(property) name Displayed on the picker.
* SelectedItems
    * IList where selected items is stored.
    * This have to assing a instance and must not null.
* SelectedItemsOrderKey
    * Class member(Property) name that becomes a order key when selected items is displayed  as text.
    * If this property is null, order type becomes naturalsort.
* MaxSelectedNumber
    * Selectable items number.
    * If zero, unlimited multi select mode. Else if One, single select mode. Otherwise limited multi select mode.
* KeepSelectedUntilBack
    * When moving next page or showing a dialog, whether keep the cell selected until being back to the page.
* AccentColor
    * Picker checkbox color.


## EntryCell

This is a cell inputing some text.

### Properties

* ValueText
    * Input text. (default two way binding)
* ValueTextColor
    * Input text color.
* ValueTextFontSize
    * Input text font size.
* MaxLength
    * Input text maximum length.
* Keyboard
    * Keyboard type.
* Placeholder
    * Placeholder text.
* TextAlignment
    * Input text horizontal alignment.
* AccentColor
    * Under line color on focus. (only android)

## License

MIT Licensed.
