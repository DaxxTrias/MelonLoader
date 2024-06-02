﻿#if NET35
using HarmonyLib;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
#endif

namespace MelonLoader.CompatibilityLayers;

internal static class Net20Compatibility
{
    internal static bool Active { get; private set; }

#if NET35
    public static void TryInstall()
    {
        if (Environment.Version.Major != 2)
            return;

        Active = true;

        MelonEvents.OnPreInitialization.Subscribe(OnPreInit, unsubscribeOnFirstInvocation: true);

        Core.HarmonyInstance.Patch(AccessTools.Constructor(typeof(Regex), [ typeof(string), typeof(RegexOptions) ]), new(typeof(Net20Compatibility), nameof(RegexCtor)));
    }

    private static void OnPreInit()
    {
        MelonLogger.MsgDirect(Color.Yellow, "The current game is running on .NET Framework 2.0, which is obsolete. Some universal Melons may run into unexpected errors.");
    }

    private static void RegexCtor([HarmonyArgument(1)] ref RegexOptions options)
    {
        // Compiled regex is not supported and results in an exception
        options &= ~RegexOptions.Compiled;
    }
#endif
}