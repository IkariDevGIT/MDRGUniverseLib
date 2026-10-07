#if CPP
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using UniverseLib.Runtime.Il2Cpp;
using System.Runtime.InteropServices;

#if INTEROP
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using IL2CPPType = Il2CppInterop.Runtime.Il2CppType;
#else
using UnhollowerRuntimeLib;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Attributes;
using IL2CPPType = UnhollowerRuntimeLib.Il2CppType;
#endif

#nullable enable

namespace UniverseLib;

/// <summary>
/// Replacement class for AssetBundles in case they were stripped by the game.
/// </summary>
public class AssetBundle : UnityEngine.Object
{
    static AssetBundle()
    {
        ClassInjector.RegisterTypeInIl2Cpp<AssetBundle>();
    }

    // ~~~~~~~~~~~~ Static ~~~~~~~~~~~~

    // AssetBundle.LoadFromFile(string path)

    internal delegate IntPtr d_LoadFromFile(IntPtr path, uint crc, ulong offset);
    private delegate IntPtr d_LoadFromFile_Injected(ref ManagedSpanWrapper path, uint crc, ulong offset);

    [HideFromIl2Cpp]
    public static AssetBundle? LoadFromFile(string path) => LoadFromFile(path, 0u, 0UL);

    [HideFromIl2Cpp]
    public static AssetBundle? LoadFromFile(string path, uint crc) => LoadFromFile(path, crc, 0UL);

    [HideFromIl2Cpp]
    public static AssetBundle? LoadFromFile(string path, uint crc, ulong offset)
    {
        IntPtr? ptr;
        if (ICallManager.TryGetICallUnreliable<d_LoadFromFile_Injected>(
                out var iCall,
                "UnityEngine.AssetBundle::LoadFromFile_Internal_Injecte",
                "UnityEngine.AssetBundle::LoadFromFile_Injecte"))
        {
            IntPtr gcHandle = ManagedSpanWrapper.Invoke(path, span => iCall!.Invoke(ref span, crc, offset));
            ptr = (gcHandle != IntPtr.Zero) ? gcHandle.GetTargetPtr() : null;
        }
        else
        {
            ptr = ICallManager.GetICallUnreliable<d_LoadFromFile>(
                "UnityEngine.AssetBundle::LoadFromFile_Internal",
                "UnityEngine.AssetBundle::LoadFromFile")
            ?.Invoke(IL2CPP.ManagedStringToIl2Cpp(path), crc, offset);
        }

        return ptr.HasValue && ptr.Value != IntPtr.Zero ? new AssetBundle(ptr.Value) : null;
    }

    // AssetBundle.LoadFromMemory(byte[] binary)

    private delegate IntPtr d_LoadFromMemory(IntPtr binary, uint crc);

    private delegate void d_ValidateLoadFromStream(IntPtr stream);
    private delegate IntPtr d_LoadFromStream(IntPtr stream, uint crc, uint managedReadBufferSize);

    [HideFromIl2Cpp]
    public static AssetBundle? LoadFromMemory(byte[] binary, uint crc = 0)
    {
        var il2cppArray = new Il2CppStructArray<byte>(binary);
        var loadFromMemory = ICallManager.GetICallUnreliable<d_LoadFromMemory>(
            "UnityEngine.AssetBundle::LoadFromMemory_Internal",
            "UnityEngine.AssetBundle::LoadFromMemory");
        IntPtr? ptr = loadFromMemory != null && IsNativeCode(Marshal.GetFunctionPointerForDelegate(loadFromMemory))
            ? loadFromMemory(il2cppArray.Pointer, crc)
            : null;

        if (ptr.HasValue && ptr.Value != IntPtr.Zero)
        {
            return new AssetBundle(ptr.Value);
        }

        Il2CppSystem.IO.MemoryStream il2CppStream = new();
        il2CppStream.Write(il2cppArray, 0, il2cppArray.Length);
        il2CppStream.Flush();

        IntPtr streamPtr = il2CppStream.ToIl2CppPointer();

        ICallManager.GetICallUnreliable<d_ValidateLoadFromStream>(
            "UnityEngine.AssetBundle::ValidateLoadFromStream"
            )?.Invoke(streamPtr);

        ptr = ICallManager.GetICallUnreliable<d_LoadFromStream>(
                "UnityEngine.AssetBundle::LoadFromStreamInternal",
                "UnityEngine.AssetBundle::LoadFromStream")
            ?.Invoke(streamPtr, crc, 0);

        return ptr.HasValue && ptr.Value != IntPtr.Zero ? new AssetBundle(ptr.Value) : null;
    }

    private static bool IsNativeCode(IntPtr address)
    {
        long target = address.ToInt64();
        foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
        {
            long start = module.BaseAddress.ToInt64();
            if (target >= start && target < start + module.ModuleMemorySize)
                return true;
        }

        return false;
    }

    // AssetBundle.GetAllLoadedAssetBundles()

    public delegate IntPtr d_GetAllLoadedAssetBundles_Native();

    [HideFromIl2Cpp]
    public static AssetBundle[] GetAllLoadedAssetBundles()
    {
        IntPtr? ptr = 
            ICallManager.GetICall<d_GetAllLoadedAssetBundles_Native>("UnityEngine.AssetBundle::GetAllLoadedAssetBundles_Native")?.Invoke();

        return ptr.HasValue && ptr.Value != IntPtr.Zero ? (AssetBundle[])new Il2CppReferenceArray<AssetBundle>(ptr.Value) : Array.Empty<AssetBundle>();
    }

    // ~~~~~~~~~~~~ Instance ~~~~~~~~~~~~

    public readonly IntPtr m_bundlePtr = IntPtr.Zero;
    public readonly IntPtr m_bundlePtr_Injected = IntPtr.Zero;

    public AssetBundle(IntPtr ptr) : base(ptr)
    {
        m_bundlePtr = ptr;
        m_bundlePtr_Injected = Marshal.ReadIntPtr(m_bundlePtr + 0x10); // Skip the Il2CppObject header + read the REAL object ptr from there.
    }

    // LoadAllAssets()

    internal delegate IntPtr d_LoadAssetWithSubAssets_Internal(IntPtr _this, IntPtr name, IntPtr type);
    private delegate IntPtr d_LoadAssetWithSubAssets_Internal_Injected(IntPtr _this, ref ManagedSpanWrapper name, IntPtr type);

    [HideFromIl2Cpp]
    public UnityEngine.Object[] LoadAllAssets()
    {
        IntPtr? ptr;
        if (ICallManager.TryGetICall<d_LoadAssetWithSubAssets_Internal_Injected>("UnityEngine.AssetBundle::LoadAssetWithSubAssets_Internal_Injected", out var icall))
        {
            IntPtr gcHandle = ManagedSpanWrapper.Invoke(string.Empty, span => icall!.Invoke(m_bundlePtr_Injected, ref span, IL2CPPType.Of<UnityEngine.Object>().Pointer));
            ptr = (gcHandle != IntPtr.Zero) ? Marshal.ReadIntPtr(gcHandle) : null;
        }
        else
        {

            ptr = ICallManager.GetICall<d_LoadAssetWithSubAssets_Internal>("UnityEngine.AssetBundle::LoadAssetWithSubAssets_Internal")
                ?.Invoke(m_bundlePtr, IL2CPP.ManagedStringToIl2Cpp(""), IL2CPPType.Of<UnityEngine.Object>().Pointer);
        }

        return ptr.HasValue && ptr.Value != IntPtr.Zero ? (UnityEngine.Object[])new Il2CppReferenceArray<UnityEngine.Object>(ptr.Value) : Array.Empty<UnityEngine.Object>();
    }

    // LoadAsset<T>(string name, Type type)

    internal delegate IntPtr d_LoadAsset_Internal(IntPtr _this, IntPtr name, IntPtr type);
    private delegate IntPtr d_LoadAsset_Internal_Injected(IntPtr _this, ref ManagedSpanWrapper name, IntPtr type);

    [HideFromIl2Cpp]
    public T? LoadAsset<T>(string name) where T : UnityEngine.Object
    {
        IntPtr? ptr;
        if (ICallManager.TryGetICall<d_LoadAsset_Internal_Injected>("UnityEngine.AssetBundle::LoadAsset_Internal_Injected", out var icall))
        {
            IntPtr gcHandle = ManagedSpanWrapper.Invoke(name, span => icall!.Invoke(m_bundlePtr_Injected, ref span, IL2CPPType.Of<T>().Pointer));
            ptr = (gcHandle != IntPtr.Zero) ? Marshal.ReadIntPtr(gcHandle) : null;
        }
        else
        {

            ptr = ICallManager.GetICall<d_LoadAsset_Internal>("UnityEngine.AssetBundle::LoadAsset_Internal")
                ?.Invoke(m_bundlePtr, IL2CPP.ManagedStringToIl2Cpp(name), IL2CPPType.Of<T>().Pointer);
        }

        return ptr.HasValue && ptr.Value != IntPtr.Zero ? new UnityEngine.Object(ptr.Value).TryCast<T>() : null;
    }

    // Unload(bool unloadAllLoadedObjects);

    internal delegate void d_Unload(IntPtr _this, bool unloadAllLoadedObjects);

    [HideFromIl2Cpp]
    public void Unload(bool unloadAllLoadedObjects)
    {
        var targetPtr = ICallManager.TryGetICall<d_LoadAsset_Internal_Injected>("UnityEngine.AssetBundle::LoadAsset_Internal_Injected", out var _) ?
            m_bundlePtr_Injected : m_bundlePtr;
        ICallManager.GetICall<d_Unload>("UnityEngine.AssetBundle::Unload")?.Invoke(targetPtr, unloadAllLoadedObjects);
    }
}
#endif