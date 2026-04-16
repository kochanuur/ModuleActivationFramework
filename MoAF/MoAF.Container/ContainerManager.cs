using MoAF.Abstractions.Container;
using MoAF.Abstractions.Modules;
using MoAF.Core.Application;
using MoAF.Core.Exception;
using System.Reflection;

namespace MoAF.Container
{
    internal class ContainerManager : IContainerManager
    {
        /// <summary>
        /// インスタンス（外部からインスタンス化させない）→ ContainerManager.Instance で取得してもらう
        /// </summary>
        internal static IContainerManager Instance { get; } = new ContainerManager();

        /// <summary>
        /// Register
        /// </summary>
        public IContainerRegistry Registry { get; private set; }
        /// <summary>
        /// Provider
        /// </summary>
        public IContainerProvider Provider { get; private set; }

        private List<ContainerStruct> containerStructs;

        private enum ConstructorType
        {
            WithoutArgument,
            WithArgument,
        }

        #region// コンストラクタ、Registry・Provider生成
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private ContainerManager()
        {
            containerStructs = new List<ContainerStruct>();

            SetContainerRegistry();
            SetContainerProvider();
            Registry = GetContainerRegistry();
            Provider = GetContainerProvider();

            SetEventHandler();

            MoAFApp.ResolveRegistry = () => Registry;
            MoAFApp.ResolveProvider = () => Provider;
        }
        private void SetContainerRegistry()
        {
            ContainerStruct containerStruct = new ContainerStruct()
            {
                LifeSpan = LifeSpan.Singleton,
                ServiceType = typeof(IContainerRegistry),
                ImplementType = typeof(ContainerRegistry),
                Key = nameof(ContainerRegistry),
                State = InstanceState.NotCreate
            };
            SetContainerStruct(containerStruct);
        }
        private void SetContainerProvider()
        {
            ContainerStruct containerStruct = new ContainerStruct()
            {
                LifeSpan = LifeSpan.Singleton,
                ServiceType = typeof(IContainerProvider),
                ImplementType = typeof(ContainerProvider),
                Key = nameof(ContainerProvider),
                State = InstanceState.NotCreate
            };
            SetContainerStruct(containerStruct);
        }
        private IContainerRegistry GetContainerRegistry()
        {
            ContainerResolveStruct resolveStruct = new ContainerResolveStruct()
            {
                ResolveType = typeof(IContainerRegistry),
                Key = nameof(ContainerRegistry),
            };
            GetContainerStruct(resolveStruct);
            return (resolveStruct.Instance as IContainerRegistry)!;
        }
        private IContainerProvider GetContainerProvider()
        {
            ContainerResolveStruct resolveStruct = new ContainerResolveStruct()
            {
                ResolveType = typeof(IContainerProvider),
                Key = nameof(ContainerProvider),
            };
            GetContainerStruct(resolveStruct);
            return (resolveStruct.Instance as IContainerProvider)!;
        }
        private void SetEventHandler()
        {
            ((ContainerRegistry)Registry).SetContainerStructHandler += SetContainerStruct;
            ((ContainerRegistry)Registry).RemoveContainerStructHandler += RemoveContainerStruct;
            ((ContainerProvider)Provider).GetContainerStructHandler += GetContainerStruct;
        }
        #endregion

        #region// Privateメソッド
        private void SetContainerStruct(ContainerStruct containerStruct)
        {
            switch (containerStruct.LifeSpan)
            {
                case LifeSpan.Singleton:
                    RegisterSingleton(containerStruct);
                    break;
                case LifeSpan.Transient:
                    break;
            }
        }
        private void GetContainerStruct(ContainerResolveStruct containerResolveStruct)
        {
            // 同じImplementedType で同じKeyのものがあるか
            List<ContainerStruct> sameImplementedType = GetSameImplementedType(containerStructs, containerResolveStruct.ResolveType);
            if (sameImplementedType.Any())
            {
                List<ContainerStruct> sameKey = GetSameKey(sameImplementedType, containerResolveStruct.Key);
                if (IsSameInstance(sameKey.Select(x => x.Instance).ToList()))
                {
                    containerResolveStruct.Instance = sameKey[0].Instance;
                    return;
                }
                else
                {
                    // 例外
                    // 同じImplementedTypeで同じKeyはありえないはずだけど一応
                }
            }
            // 同じServiceType で同じKeyのものがあるか
            else
            {
                List<ContainerStruct> sameServiceType = GetSameServiceType(containerStructs, containerResolveStruct.ResolveType);
                if (sameServiceType.Any())
                {
                    List<ContainerStruct> sameKey = GetSameKey(sameServiceType, containerResolveStruct.Key);
                    if (IsSameInstance(sameKey.Select(x => x.Instance).ToList()))
                    {
                        containerResolveStruct.Instance = sameKey[0].Instance;
                        return;
                    }
                    else
                    {
                        // 例外
                    }
                }
            }
        }
        /// <summary>
        /// 消せるのはSingleton登録のもののみ
        /// </summary>
        /// <param name="containerStruct"></param>
        private void RemoveContainerStruct(ContainerStruct containerStruct)
        {
            if (containerStruct.LifeSpan != LifeSpan.Singleton)
            {
                // 例外出す
            }

            List<ContainerStruct> sameImplementedType = GetSameImplementedType(containerStructs, containerStruct.ImplementType);
            foreach (ContainerStruct @struct in sameImplementedType)
            {
                if (IsSameContainerStruct(@struct, containerStruct))
                {
                    containerStructs.Remove(@struct);
                }
            }
        }
        #endregion

        #region// ContainerStruct への登録処理
        private void RegisterSingleton(ContainerStruct containerStruct)
        {
            // ImplementType が既に登録済みか否か
            if (containerStructs.Any(x => x.ImplementType.FullName == containerStruct.ImplementType.FullName))
            {
                SameImplementedType(containerStruct);
            }
            else
            {
                CreateNewInstance(containerStruct);
            }
        }
        private void SameImplementedType(ContainerStruct containerStruct)
        {
            List<ContainerStruct> sameImplementedType = containerStructs.Where(x => x.ImplementType.FullName == containerStruct.ImplementType.FullName).ToList();
            // ServiceType が既に登録済みか
            if (sameImplementedType.Any(x => x.ServiceType.FullName == containerStruct.ServiceType.FullName))
            {
                List<ContainerStruct> sameServiceType = sameImplementedType.Where(x => x.ServiceType.FullName == containerStruct.ServiceType.FullName).ToList();
                // Key が既に登録済みか
                if (sameServiceType.Any(x => x.Key == containerStruct.Key))
                {
                    // まったく同じ内容の登録なので登録しない
                    return;
                }
                // Key が登録されているものと空のものをどちらも持つ必要ある？
                containerStruct.Instance = sameServiceType[0].Instance;
                containerStruct.State = InstanceState.Created;
                containerStructs.Add(containerStruct);
                return;
            }
            containerStruct.Instance = sameImplementedType[0].Instance;
            containerStruct.State = InstanceState.Created;
            containerStructs.Add(containerStruct);
            return;
        }
        private void CreateNewInstance(ContainerStruct containerStruct)
        {
            // 手順
            // 1. `GetConstructorType` でインスタンス化に使用するコンストラクタを取得する
            // 2. 戻り値で引数有無を判定
            //  　→あり：`CreateInstanceWithArguments` で引数付きコンストラクタ実行
            //  　→なし：`CreateInstanceWithoutArguments` で引数なしコンストラクタ実行
            // 3. インスタンス作成に成功したら、`containerStructs` に `containerStruct` を追加する

            ConstructorType constructorType = GetConstructorType(containerStruct, out ConstructorInfo constructor);

            if (constructorType == ConstructorType.WithArgument)
            {
                CreateInstanceWithArguments(containerStruct, constructor);
            }
            else
            {
                CreateInstanceWithoutArguments(containerStruct);
            }

            containerStruct.State = InstanceState.Created;
            containerStructs.Add(containerStruct);
        }
        private ConstructorType GetConstructorType(ContainerStruct containerStruct, out ConstructorInfo constructorInfo)
        {
            // 手順
            // 1. `BindingFlags.Public | BindingFlags.Instance` の条件でコンストラクタを取得
            //  　→0：MoAFResultCode.NotExistPublicCtor
            // 2. `ModuleConstructor` 属性がついたコンストラクタの個数を確認
            //  　→2：MoAFResultCode.AttributeCtorMultiple
            //  　→1：手順3.へ
            //  　→0：手順4.へ
            // 3. 属性付きコンストラクタが引数有りか否か
            //  　→あり：WithArgument
            //  　→なし：WithoutArgument
            // 4. 1.で取得したコンストラクタの個数が2以上
            //  　→Yes：MoAFResultCode.NonAttributeCtorMultiple
            //  　→No：手順5.へ
            // 5. 唯一のコンストラクタが引数有りか否か
            //  　→あり：WithArgument
            //  　→なし：WithoutArgument

            ConstructorInfo[] constructors = containerStruct.ImplementType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 0)
            {
                string message = $"{GetErrorClassMessage(containerStruct)} で登録されたクラスが、Publicコンストラクタを実装していません。";
                throw new MoAFException(MoAFResultCode.NotExistPublicCtor, message);
            }

            ConstructorInfo[] attributeCtors = constructors.Where(x => x.IsDefined(typeof(ModuleConstructorAttribute), inherit: false)).ToArray();
            if (attributeCtors.Length > 1)
            {
                string message = $"{GetErrorClassMessage(containerStruct)} で登録されたクラスにおいて、ModuleConstructor属性が複数のコンストラクタに付与されています。";
                throw new MoAFException(MoAFResultCode.AttributeCtorMultiple, message);
            }
            else if (attributeCtors.Length == 1)
            {
                constructorInfo = attributeCtors[0];
                return HasArguments(constructorInfo) ? ConstructorType.WithArgument : ConstructorType.WithoutArgument;
            }

            if (constructors.Length > 1)
            {
                string message = $"{GetErrorClassMessage(containerStruct)} で登録されたクラスにおいて、どのコンストラクタにもModuleConstructor属性が付与されておらず、かつ複数コンストラクタが定義されています。";
                throw new MoAFException(MoAFResultCode.NonAttributeCtorMultiple, message);
            }

            constructorInfo = constructors[0];
            return HasArguments(constructorInfo) ? ConstructorType.WithArgument : ConstructorType.WithoutArgument;
        }
        private bool HasArguments(ConstructorInfo constructorInfo)
        {
            return constructorInfo != null && constructorInfo.GetParameters().Any();
        }
        //private void CreateInstanceWithArguments(ContainerStruct containerStruct)
        //{
        //    List<ConstructorInfo> constructors = containerStruct.ImplementType.GetConstructors().ToList();
        //    if (constructors.Count != 2)
        //    {
        //        // 例外
        //        // デフォルトコンストラクタの他にコンストラクタが2つ以上ある
        //    }

        //    foreach (ConstructorInfo constructor in constructors)
        //    {
        //        List<ParameterInfo> parameters = constructor.GetParameters().ToList();
        //        List<Type> types = parameters.Select(x => x.ParameterType).ToList();
        //        if (!types.Any())
        //        {
        //            continue;
        //        }

        //        List<object> objects = new List<object>();
        //        foreach (Type type in types)
        //        {
        //            List<ContainerStruct> structs = containerStructs.Where(x => x.ImplementType == type).ToList();
        //            if (structs.Any())
        //            {
        //                // インスタンスの中身が同じかを調べる必要がある
        //                if (IsSameInstance(structs.Select(x => x.Instance).ToList()))
        //                {
        //                    objects.Add(structs[0].Instance);
        //                }
        //                else
        //                {
        //                    // 例外（引数として該当するインスタンスが複数ある）
        //                }
        //            }
        //            else
        //            {
        //                // ServiceTypeの調査
        //                List<Type> baseTypes = GetInheritServiceType(type);
        //                if (baseTypes.Count == 1)
        //                {
        //                    objects.Add(containerStructs.First(x => x.ServiceType == baseTypes[0]).Instance);
        //                }
        //                else
        //                {
        //                    // 例外（引数として該当するインスタンスが複数ある）
        //                }
        //            }
        //        }
        //        containerStruct.Instance = constructor.Invoke(objects.ToArray());
        //    }
        //}
        private void CreateInstanceWithArguments(ContainerStruct containerStruct, ConstructorInfo constructorInfo)
        {
            ParameterInfo[] parameters = constructorInfo.GetParameters().ToArray();
            Type[] types = parameters.Select(x => x.ParameterType).ToArray();

            List<object> objects = new List<object>();
            foreach (Type type in types)
            {
                ArgumentNamedAttribute? attributeName = type.GetCustomAttribute<ArgumentNamedAttribute>();
                ContainerStruct[] sameImplemented = containerStructs.Where(x => x.ImplementType == type).ToArray();
                if (sameImplemented.Any())
                {
                    ContainerStruct[] sameKey = sameImplemented.Where(x => x.Key == attributeName?.Name).ToArray();

                    if (sameKey.Any())
                    {
                        if (IsSameInstance(sameKey.Select(x => x.Instance).ToList()))
                        {
                            objects.Add(sameKey[0].Instance);
                        }
                        else
                        {
                            // 例外（引数として該当するインスタンスが複数ある）
                        }
                    }
                    else
                    {
                        if (IsSameInstance(sameImplemented.Select(x => x.Instance).ToList()))
                        {
                            objects.Add(sameImplemented[0].Instance);
                        }
                        else
                        {
                            // 例外（引数として該当するインスタンスが複数ある）
                        }
                    }
                }
                else
                {   
                    // ServiceTypeの調査 (type が継承しているインターフェースの調査)
                    List<Type> baseTypes = GetInheritServiceType(type);
                    if (baseTypes.Count == 1)
                    {
                        ContainerStruct[] sameService = containerStructs.Where(x => x.ServiceType == baseTypes[0]).ToArray();
                        if (sameService.Any())
                        {
                            ContainerStruct[] sameKey = sameService.Where(x => x.Key == attributeName?.Name).ToArray();
                            if (sameKey.Any())
                            {
                                if (IsSameInstance(sameKey.Select(x => x.Instance).ToList()))
                                {
                                    objects.Add(sameKey[0].Instance);
                                }
                                else
                                {
                                    // 例外（引数として該当するインスタンスが複数ある）
                                }
                            }
                            else
                            {
                                if (IsSameInstance(sameService.Select(x => x.Instance).ToList()))
                                {
                                    objects.Add(sameService[0].Instance);
                                }
                                else
                                {
                                    // 例外（引数として該当するインスタンスが複数ある）
                                }

                            }
                        }
                    }
                    else
                    {
                        // 例外（引数として該当するインスタンスが複数ある）
                    }

                }
                // 今のところ、ImplementedType と type が一致するかと、type が実装しているインターフェースがあるか
                // のみを確認しているけど、ServiceType と type が一致するかも確認する必要があるのでは？
                // GetInheritServiceType において、ServiceTypeに `InterfaceA` が登録されているインスタンスがある場合、
                // type に `InterfaceA` を指定したら取得できるのか？
            }

            containerStruct.Instance = constructorInfo.Invoke(objects.ToArray());
        }
        private List<Type> GetInheritServiceType(Type type)
        {
            List<Type> baseTypes = new List<Type>();

            // type が ServiceTypeを継承しているか調査
            foreach (ContainerStruct containerStruct in containerStructs)
            {
                if (containerStruct.ServiceType.IsAssignableFrom(type))
                {
                    // type が ServiceType を継承している、またはServiceType インターフェースを実装している
                    baseTypes.Add(containerStruct.ServiceType);
                }
            }

            return baseTypes;
        }
        private bool IsSameInstance(List<object> instances)
        {
            if (instances.Count == 1)
            {
                return true;
            }

            for (int i = 1; i < instances.Count; i++)
            {
                if (!ReferenceEquals(instances[i], instances[0]))
                {
                    return false;
                }
            }
            return true;
        }
        private void CreateInstanceWithoutArguments(ContainerStruct containerStruct)
        {
            containerStruct.Instance = Activator.CreateInstance(containerStruct.ImplementType)!;
        }
        #endregion

        #region// ContainerStruct の取得処理
        private List<ContainerStruct> GetSameImplementedType(List<ContainerStruct> structs, Type type)
        {
            return structs.Where(x => x.ImplementType == type).ToList();
        }

        private List<ContainerStruct> GetSameServiceType(List<ContainerStruct> structs, Type type)
        {
            return structs.Where(x => x.ServiceType == type).ToList();
        }

        private List<ContainerStruct> GetSameKey(List<ContainerStruct> structs, string key)
        {
            return structs.Where(x => x.Key == key).ToList();
        }

        private bool IsExistSameKey(ContainerStruct containerStruct)
        {
            if (containerStruct.Key != string.Empty)
            {
                // Keyが同じものがあるか
                List<ContainerStruct> sameKeys = containerStructs.Where(x => x.Key == containerStruct.Key).ToList();
                if (sameKeys.Count > 0)
                {
                    foreach (ContainerStruct sameKey in sameKeys)
                    {
                        // ImplemenetdType が同じものか
                        if (sameKey.ImplementType == containerStruct.ImplementType)
                        {
                            containerStruct.Instance = sameKey.Instance;
                            return true;
                        }
                    }
                    // ServiceType が同じものの取得
                    List<ContainerStruct> sameServiceTypes = sameKeys.Where(x => x.ServiceType == containerStruct.ServiceType).ToList();
                }
            }

            return false;
        }

        #endregion

        #region// ContainerStruct からの削除処理
        private bool IsSameContainerStruct(ContainerStruct targetStruct, ContainerStruct removeInfoStruct)
        {
            if (targetStruct.ServiceType == removeInfoStruct.ServiceType
                && targetStruct.ImplementType == removeInfoStruct.ImplementType
                && targetStruct.Key == removeInfoStruct.Key)
            {
                return true;
            }
            return false;
        }
        #endregion

        private string GetErrorClassMessage(ContainerStruct containerStruct)
        {
            return $"ServiceType : {containerStruct.ServiceType.Name}、ImplementType : {containerStruct.ImplementType.Name}、Key : {containerStruct.Key}";
        }
    }
}
