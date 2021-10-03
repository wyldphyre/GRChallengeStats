using System;
using System.Windows;
using System.Windows.Input;

namespace MvvmLibrary.Behaviours
{
    public static class MouseDownBehaviour
    {
        public static readonly DependencyProperty MouseDownCommandProperty = DependencyProperty.RegisterAttached
        (
            "MouseDownCommand",
            typeof(ICommand),
            typeof(MouseDownBehaviour),
            new PropertyMetadata(MouseDownCommandPropertyChangedCallBack)
        );
        
        public static void SetMouseDownCommand(this UIElement inUiElement, ICommand inCommand)
        {
            if (inUiElement is null)
                throw new ArgumentNullException(nameof(inUiElement));

            inUiElement.SetValue(MouseDownCommandProperty, inCommand);
        }

        private static ICommand GetMouseDownCommand(UIElement inUIElement)
        {
            return (ICommand)inUIElement.GetValue(MouseDownCommandProperty);
        }
        
        private static void MouseDownCommandPropertyChangedCallBack(DependencyObject inDependencyObject, DependencyPropertyChangedEventArgs inEventArgs)
        {
            UIElement uiElement = inDependencyObject as UIElement;
            if (null == uiElement) return;

            uiElement.MouseDown += (sender, args) =>
            {
                GetMouseDownCommand(uiElement).Execute(args);
                args.Handled = true;
            };
        }
    }
}