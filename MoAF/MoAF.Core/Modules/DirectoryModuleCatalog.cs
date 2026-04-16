using MoAF.Abstractions.Modules;
using System.Reflection;

namespace MoAF.Core.Modules
{
    internal class DirectoryModuleCatalog : IModuleCatalog
    {
        public List<IModuleInfo> Modules { get; private set; }

        public ModuleCatalogStruct ModuleCatalogStruct { get; set; }

        public DirectoryModuleCatalog()
        {
            Modules = new List<IModuleInfo>();
        }

        public void Run()
        {
            Modules.Clear();
            string selfDll = $"{ModuleCatalogStruct.ModulePath}\\MoAF.dll";
            string interfaceDll = $"{ModuleCatalogStruct.ModulePath}\\{ModuleCatalogStruct.InterfaceDllName}";

            if (Directory.Exists(ModuleCatalogStruct.ModulePath))
            {
                List<string> dlls = Directory.GetFiles(ModuleCatalogStruct.ModulePath, "*.dll").ToList();
                if (dlls.Contains(selfDll))
                {
                    // MoAF.dll は読み込む必要がないため除外
                    dlls.Remove(selfDll);
                }
                if (dlls.Contains(interfaceDll))
                {
                    // Common（Interface類を実装しているdll）を先に読み込むと、Interface不整合が起きて後々castができない問題が発生する
                    dlls.Remove(interfaceDll);
                }

                foreach (string dllName in dlls)
                {
                    Assembly assembly = Assembly.LoadFrom(dllName);

                    // アセンブリで定義されている型を取得
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        // 非クラス型、非パブリック型、抽象クラスはスキップ
                        if (!type.IsClass || !type.IsPublic || type.IsAbstract)
                        {
                            continue;
                        }

                        // 型に実装されているインターフェースから IModule を取得
                        Type? t = type.GetInterfaces().FirstOrDefault(x => x == typeof(IModule));
                        // default(IModule) と等しい場合は未実装なのでスキップ
                        if (t == null || t == default(IModule))
                        {
                            continue;
                        }

                        // ModuleInfo に登録
                        AddModuleInfo(type);
                        break;
                    }
                }
            }
        }

        private void AddModuleInfo(Type type)
        {
            // これなくてもよい？
            // FullNameが同じということはまったく同じクラスだから、そんなことあり得るか？
            if (Modules.Any(x => x.ModuleName == type.FullName))
            {
                return;
            }

            Modules.Add(new ModuleInformation()
            {
                ModuleName = type.FullName!,
                ModuleType = type,
                ModuleState = ModuleState.NotStarted
            });
        }
    }
}
