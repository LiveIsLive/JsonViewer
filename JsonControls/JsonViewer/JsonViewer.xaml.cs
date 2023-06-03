using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;

namespace JsonControls
{
	/// <summary>
	/// Interaction logic for JsonViewer.xaml
	/// </summary>
	public partial class JsonViewer : UserControl
	{
		private const GeneratorStatus Generated = GeneratorStatus.ContainersGenerated;
		private DispatcherTimer _timer;

		public JsonViewer()
		{
			InitializeComponent();
		}



		public DataType DataType
		{
			get { return (DataType)GetValue(DataTypeProperty); }
			set { SetValue(DataTypeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DataType.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.Register("DataType", typeof(DataType), typeof(JsonViewer), new PropertyMetadata(DataType.Json, DataChanged));



		public string Data
		{
			get { return (string)GetValue(DataProperty); }
			set { SetValue(DataProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Json.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register("Data", typeof(string), typeof(JsonViewer), new PropertyMetadata(null, DataChanged));

		public static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			JsonViewer me = (JsonViewer)d;
			TreeView JsonTreeView = ((JsonViewer)d).JsonTreeView;

			JsonTreeView.ItemsSource = null;
			JsonTreeView.Items.Clear();
			me.Error = null;

			string data = me.Data;
			if (string.IsNullOrWhiteSpace(data))
			{
				me.Error = "";
				return;
			}

			var children = new List<JToken>();

			try
			{
				JToken token = null;
				switch(me.DataType)
				{
					case DataType.Json:
						token = JToken.Parse(data);
						break;
					case DataType.Xml:
						System.Xml.XmlDocument document = new System.Xml.XmlDocument();
						document.LoadXml(data);
						token = JToken.Parse(Newtonsoft.Json.JsonConvert.SerializeXmlNode(document.DocumentElement));
						break;
				}

				if (token != null)
				{
					children.Add(token);
				}

				JsonTreeView.ItemsSource = children;
			}
			catch (Exception ex)
			{
				me.Error="Could not open the JSON string:\r\n" + ex.Message;
			}
		}



		public string Error
		{
			get { return (string)GetValue(ErrorProperty); }
			protected set { SetValue(ErrorProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Error.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ErrorProperty =
			DependencyProperty.Register("Error", typeof(string), typeof(JsonViewer), new PropertyMetadata(null));



		private void JValue_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount != 2) 
				return;
			
			var tb = sender as TextBlock;
			if (tb != null)
			{
				Clipboard.SetText(tb.Text); 
			}
		}

		private void ExpandAll(object sender, RoutedEventArgs e)
		{
			ToggleItems(true);
		}

		private void CollapseAll(object sender, RoutedEventArgs e)
		{
			ToggleItems(false);
		}

		private void ToggleItems(bool isExpanded)
		{
			if (JsonTreeView.Items.IsEmpty)
				return;

			var prevCursor = Cursor;
			//System.Windows.Controls.DockPanel.Opacity = 0.2;
			//System.Windows.Controls.DockPanel.IsEnabled = false;
			Cursor = Cursors.Wait;
			_timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Normal, delegate
			{
				ToggleItems(JsonTreeView, JsonTreeView.Items, isExpanded);
				//System.Windows.Controls.DockPanel.Opacity = 1.0;
				//System.Windows.Controls.DockPanel.IsEnabled = true;
				_timer.Stop();
				Cursor = prevCursor;
			}, Application.Current.Dispatcher);
			_timer.Start();
		}

		private void ToggleItems(ItemsControl parentContainer, ItemCollection items, bool isExpanded)
		{
			var itemGen = parentContainer.ItemContainerGenerator;
			if (itemGen.Status == Generated)
			{
				Recurse(items, isExpanded, itemGen);
			}
			else
			{
				itemGen.StatusChanged += delegate
				{
					Recurse(items, isExpanded, itemGen);
				};
			}
		}

		private void Recurse(ItemCollection items, bool isExpanded, ItemContainerGenerator itemGen)
		{
			if (itemGen.Status != Generated)
				return;

			foreach (var item in items)
			{
				var tvi = itemGen.ContainerFromItem(item) as TreeViewItem;
				tvi.IsExpanded = isExpanded;
				ToggleItems(tvi, tvi.Items, isExpanded);
			}
		}
	}
}
