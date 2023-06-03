This is a WPF TreeView control for displaying json or xml data.

It is forked from：https://githu.com/catsgotmytongue/JsonControls-WPF/

How to Use？

First, add the package reference：

dotnet add package JsonControls.JsonViewer

And just set the json string to the "Data" property. For examlple：

```xml
<Window x:Class="JsonViewerDemo.MainWindow"
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:jsonViewer="clr-namespace:JsonControls;assembly=JsonControls">
	<jsonViewer:JsonViewer Data="[1,2,3]" />
	<jsonViewer:JsonViewer DataType="Xml" Data="&lt;r&gt;1&lt;/r&gt;" />
</Window>
```
