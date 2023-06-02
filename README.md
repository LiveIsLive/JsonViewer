

This is a wpf control for displaying json data.

It is forked from：https://githu.com/catsgotmytongue/JsonControls-WPF/

How to Use？

First, add the package reference：

dotnet add package JsonControls.JsonViewer

And just set the json string to the "Json" property. For examlple：

```xml
<Window x:Class="JsonViewerDemo.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:jsonViewer="clr-namespace:JsonControls;assembly=JsonControls">
	<jsonViewer:JsonViewer Json="[1,2,3]" />
</Window>
```
