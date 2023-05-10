using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

namespace ScanCode.WPF
{
    //public static class TextBlockExtensions
    //{
    //    // 定义一个名为CharacterSpacing的附加属性
    //    public static readonly DependencyProperty CharacterSpacingProperty =
    //        DependencyProperty.RegisterAttached("CharacterSpacing", typeof(int), typeof(TextBlockExtensions),
    //            new PropertyMetadata(0, OnCharacterSpacingChanged));

    //    public static int GetCharacterSpacing(DependencyObject obj)
    //    {
    //        return (int)obj.GetValue(CharacterSpacingProperty);
    //    }

    //    public static void SetCharacterSpacing(DependencyObject obj, int value)
    //    {
    //        obj.SetValue(CharacterSpacingProperty, value);
    //    }

    //    private static void OnCharacterSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var textBlock = d as TextBlock;
    //        if (textBlock == null)
    //            return;

    //        ApplyCharacterSpacing(textBlock, (int)e.NewValue);
    //    }

    //    private static void ApplyCharacterSpacing(TextBlock textBlock, int spacing)
    //    {
    //        // 将原始文本拆分为单独的字符
    //        var text = textBlock.Text;
    //        textBlock.Text = string.Empty;

    //        // 使用Run元素为每个字符应用字符间距
    //        for (int i = 0; i < text.Length; i++)
    //        {
    //            var run = new Run(text[i].ToString());
    //            if (i < text.Length - 1)
    //                run.Typography.Variants = FontVariants.Normal;
    //           // run.Typography.Tracking = spacing;

    //            textBlock.Inlines.Add(run);
    //        }
    //    }
    //}
}