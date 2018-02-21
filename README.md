# SettingsView for Xamarin.Forms

This is a flexible TableView specialized in settings for Android / iOS.

[Japanese](./README-ja.md)

## What SettingsView can do.

### General

* To set separator color.
* To set selected cell color.
* To scroll to screen top and bottom.

### Sections

* To set IsVisible each section.
* To set section a footer.
* To set various options of a header and  a footer.
* To use DataTemplate and DataTemplateSelector in a section.
* To reorder items by drag and drop in a section.

### Cells

* To set options of all the cells as SettingsView options at once.
* To set indivisual cell options. (indivisual cell options  is superior to SettingsView options.)
* To set a cell HintText.
* To use an icon  cached in memory at all cells.
* To change corner radius of an icon.
* To use various defined cells.
* To use Xamarin.Forms.ViewCell and the others.


<img src="images/iOS_SS.png" height="1200" /> <img src="images/AndroidSS.png" height="1200" />

### Demo movie

[https://youtu.be/FTMOqNILxBE](https://youtu.be/FTMOqNILxBE)

## Minimum Device and Version etc

iOS:iPhone5s,iPod touch6,iOS9.3  
Android:version 5.1.1 (only FormsAppcompatActivity) / API22

## Nuget Installation

[https://www.nuget.org/packages/AiForms.SettingsView/](https://www.nuget.org/packages/AiForms.SettingsView/)

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

## How to write with xaml

```xml
<ContentPage 
	xmlns="http://xamarin.com/schemas/2014/forms" 
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
	xmlns:sv="clr-namespace:AiForms.Renderers;assembly=SettingsView"
	x:Class="Sample.Views.SettingsViewPage">
    
<sv:SettingsView HasUnevenRows="true">
    
    <sv:Section Title="Header1" FooterText="Footer1">
        <sv:CommandCell IconSource="icon.png" IconSize="60,60" IconRadius="30"
            Title="Xam Xamarin" Description="hoge@fuga.com"
            Command="{Binding ToProfileCommand}" CommandParameter="{Binding Parameter}"
            KeepSelectedUntilBack="true"            
        />
        <sv:ButtonCell Title="Toggle Section" TitleColor="{StaticResource TitleTextColor}"
             TitleAlignment="Center" Command="{Binding SectionToggleCommand}" />
        <sv:LabelCell Title="Label" ValueText="value" />
        <sv:SwitchCell Title="Switch" On="true" 
            Description="This is description." />
        <sv:CheckboxCell Title="Checkbox" Checked="true" />
    </sv:Section>

    <sv:Section Title="Header2" FooterText="Footer2" IsVisible="{Binding SctionIsVisible}">
        <sv:PickerCell Title="Favorites" ItemsSource="{Binding ItemsSource}" DisplayMember="Name" MaxSelectedNumber="3" 
        SelectedItems="{Binding SelectedItems}" KeepSelectedUntilBack="true" PageTitle="select 3 items" />
        <sv:NumberPickerCell Title="NumberPicker" Min="0" Max="99" Number="15" PickerTitle="Select number" />
        <sv:TimePickerCell Title="TimePicker" Format="HH:mm" Time="15:30" PickerTitle="Select time" />
        <sv:DatePickerCell Title="DatePicker" Format="yyyy/MM/dd (ddd)" Date="2017/11/11" MinimumDate="2015/1/1" MaximumDate="2018/12/15" TodayText="Today's date"/>
        <sv:EntryCell Title="EntryCell" ValueText="{Binding InputText.Value}" Placeholder="Input text" Keyboard="Email" TextAlignment="End" HintText="{Binding InputError.Value}" />
    </sv:Section>
    
</sv:SettingsView>
</ContentPage>
```

SettingsView properties settings may as well be witten in App.xaml. 
For example...

```xml
<Application xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:sv="clr-namespace:AiForms.Renderers;assembly=SettingsView"
             x:Class="Sample.App">
    <Application.Resources>
        <ResourceDictionary>
            <Color x:Key="AccentColor">#FFBF00</Color>
            <Color x:Key="DisabledColor">#E6DAB9</Color>
            <Color x:Key="TitleTextColor">#CC9900</Color>
            <Color x:Key="PaleBackColorPrimary">#F2EFE6</Color>
            <Color x:Key="PaleBackColorSecondary">#F2EDDA</Color>
            <Color x:Key="DeepTextColor">#555555</Color>
            <Color x:Key="NormalTextColor">#666666</Color>
            <Color x:Key="PaleTextColor">#999999</Color>
            <x:Double x:Key="BaseFontSize">12</x:Double>
            <x:Double x:Key="BaseFontSize+">14</x:Double>
            <x:Double x:Key="BaseFontSize++">17</x:Double>
            <x:Double x:Key="BaseFontSize-">11</x:Double>

            <Style TargetType="sv:SettingsView">
                <Setter Property="SeparatorColor" Value="{StaticResource DisabledColor}" />
                <Setter Property="BackgroundColor" Value="{StaticResource PaleBackColorPrimary}" />
                <Setter Property="HeaderBackgroundColor" Value="{StaticResource PaleBackColorPrimary}" />
                <Setter Property="CellBackgroundColor" Value="{StaticResource AppBackground}" />
                <Setter Property="CellTitleColor" Value="{StaticResource DeepTextColor}" />
                <Setter Property="CellValueTextColor" Value="{StaticResource NormalTextColor}" />
                <Setter Property="CellTitleFontSize" Value="{StaticResource BaseFontSize++}" />
                <Setter Property="CellValueTextFontSize" Value="{StaticResource BaseFontSize}" />
                <Setter Property="CellDescriptionColor" Value="{StaticResource NormalTextColor}" />
                <Setter Property="CellDescriptionFontSize" Value="{StaticResource BaseFontSize-}" />
                <Setter Property="CellAccentColor" Value="{StaticResource AccentColor}" />
                <Setter Property="SelectedColor" Value="#50FFBF00" />
                <Setter Property="HeaderTextColor" Value="{StaticResource TitleTextColor}" />
                <Setter Property="FooterFontSize" Value="{StaticResource BaseFontSize-}" />
                <Setter Property="FooterTextColor" Value="{StaticResource PaleTextColor}" />
            </Style>
        </ResourceDictionary>
    </Application.Resources>
</Application>

```
Whereby any SettingsView in App will become the same property setttings.

## SettingsView Properties

* BackgroundColor
	* A color of out of region and entire region. They contains header, footer and cell (in case android).
* SeparatorColor
    * Row separator color.
* SelectedColor
    * Backgraound color when row is selected.
* HeaderPadding
* HeaderTextColor
* HeaderFontSize
* HeaderTextVerticalAlign
* HeaderBackgroundColor
* HeaderHeight
    * They are section header options.
* FooterTextColor
* FooterFontSize
* FooterBackgroundColor
* FooterPadding
    * They are section footer options.
* RowHeight
	* If HasUnevenRows is false, this value apply to each row height;
	* Otherwise this value is used as minimum row height. 
* HasUnevenRows
	* Whether row height is fixed. Default false.(recomend true)
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
    * They are bulk cell options.
* UseDescriptionAsValue (for Android)
	* Whether description field  is used as value field. (like general android app)
    * Default false
* ShowSectionTopBottomBorder (for Android)
	* Whether a separator is shown at section top and bottom. (like general android app)
    * Default true
* ScrollToTop
* ScrollToBottom
	* When this property is set to true, the screen will be scrolled to first item position or last item position.
	* If scrolling has complete, it will be set to false automatically.

## SettingsView Methods

* ClearCache (static)
	* Clear all memory cache.

## Section Properties

* Title
	* Section header text. The same as Xamarin.Forms.TableSection.
* FooterText
	* Section footer text.
* IsVisible
	* Whether the section is visibled.
* ItemsSource
	* Specify the source of a DataTemplate.
* ItemTemplate
	 * Specify a DataTemplate.
* UseDragSort
	* Enable you to reorder cells in a section with drag and drop.
	* If iOS version is less than or equal to iOS10, the cells can be moved when grabbing the icon drawn three lines; Otherwise can be moved when doing long tap.

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

![cell layout](./images/cell_layout.png)

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
* IsEnabled
	* Whether a cell is enabled. If set to false, the entire cell color will turn translucent and the cell won't accept any operations.

### To use SVG image

You can use SVG image if SvgImageSource is installed.

https://github.com/muak/SvgImageSource  
https://www.nuget.org/packages/Xamarin.Forms.Svg/

```bash
Install-Package Xamain.Forms.Svg -pre
```


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

The others are the same as LabelText.

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
* SubDisplayMember
	 * Class member(property) name secondary displayed on the Picker. If this property is set, the cell will be two line and the first line will display DisplayMember and the second line will display SubDisplayMember.
* SelectedItems
    * IList where selected items is stored.
    * This have to assing a instance and must not null.
* SelectedItemsOrderKey
    * Class member(Property) name that becomes a order key when selected items is displayed  as text.
* SelectedCommand
	* A command invoked When finished being selected items. 
* MaxSelectedNumber
    * Selectable items number.
    * If zero, unlimited multi select mode. Else if One, single select mode. Otherwise limited multi select mode.
* KeepSelectedUntilBack
    * When moving next page or showing a dialog, whether keep the cell selected until being back to the page.
* AccentColor
    * Picker checkbox color.
* UseNaturalSort
	* Whether use NaturalSort as sort method. default false.
	* If true, for example, if  the order is normally  "1,10,2,3,4", is "1,2,3,4,10".
	* This option may not correctly work if not used Japanese language.
* UsePickToClose
	* Whether closing the Picker automatically if the number of selected items come to MaxSelectedNumber. 
* UseAutoValueText
	* Normally, selected items string is automatically displayed in the ValueText. If the value of this property is specified false, the auto display will be cleared and ValueText will be available as usual.

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

## Thanks 

* NaturalComparer
    * https://github.com/tomochan154/toy-box/blob/master/NaturalComparer.cs

## License

MIT Licensed.
