using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace GooseUnrunnableRuns
{
	public static class BindingHelper
	{
		public static void Bind(DependencyObject target, DependencyProperty dp, object source, string sourceProperty)
		{
			Bind(target, dp, source, sourceProperty, null);
		}

		public static void Bind(DependencyObject target, DependencyProperty dp, object source, string sourceProperty,
			IValueConverter converter)
		{
			Bind(target, dp, source, sourceProperty, converter, null);
		}

		public static void Bind(DependencyObject target, DependencyProperty dp, object source, string sourceProperty,
			IValueConverter converter, object converterParameter)
		{
			Binding binding = new Binding();
			binding.Source = source;
			binding.Mode = BindingMode.TwoWay;
			binding.Path = new PropertyPath(sourceProperty);
			binding.Converter = converter;
			binding.ConverterParameter = converterParameter;

			BindingOperations.SetBinding(target, dp, binding);
		}
	}
}
