namespace VTE.WPF.Services;

using System;
using System.Windows;

public static class ThemeService
{
    private static bool _isDark = false;

    public static bool IsDark => _isDark;

    public static void SetTheme(bool dark)
    {
        _isDark = dark;
        var dict = new ResourceDictionary();
        dict.Source = new Uri(dark
            ? "pack://application:,,,/Resources/DarkTheme.xaml"
            : "pack://application:,,,/Resources/LightTheme.xaml");

        // Replace the first merged dictionary (theme)
        var mergedDicts = Application.Current.Resources.MergedDictionaries;
        if (mergedDicts.Count > 0)
            mergedDicts[0] = dict;
        else
            mergedDicts.Insert(0, dict);
    }

    public static void Toggle()
    {
        SetTheme(!_isDark);
    }
}
