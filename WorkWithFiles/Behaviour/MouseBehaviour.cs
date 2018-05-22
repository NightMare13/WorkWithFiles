using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorkWithFiles.Behaviour
{
    public class MouseDoubleClick
    {
        public static readonly DependencyProperty MouseHandlerProperty = DependencyProperty.RegisterAttached(
           "MouseHandler",
           typeof(ICommand),
           typeof(MouseDoubleClick),
           new UIPropertyMetadata(OnMouseHandlerChanged));

        public static DependencyProperty MouseHandlerParameterProperty =
            DependencyProperty.RegisterAttached(
                "MouseHandlerParameter",
                typeof(object),
                typeof(MouseDoubleClick),
                new UIPropertyMetadata(null));

        public static void SetMouseHandler(DependencyObject target, ICommand value)
        {
            target.SetValue(MouseHandlerProperty, value);
        }

        public static void SetMouseHandlerParameter(DependencyObject target, object value)
        {
            target.SetValue(MouseHandlerParameterProperty, value);
        }
        public static object GetMouseHandlerParameter(DependencyObject target)
        {
            return target.GetValue(MouseHandlerParameterProperty);
        }

        private static void OnMouseHandlerChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var control = target as Control;
            if (control != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    control.MouseDoubleClick += OnMouseDoubleClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    control.MouseDoubleClick -= OnMouseDoubleClick;
                }
            }
        }

        private static void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;
            ICommand command = (ICommand)control.GetValue(MouseHandlerProperty);
            object commandParameter = control.GetValue(MouseHandlerParameterProperty);
            command.Execute(commandParameter);
        }

    }
}
