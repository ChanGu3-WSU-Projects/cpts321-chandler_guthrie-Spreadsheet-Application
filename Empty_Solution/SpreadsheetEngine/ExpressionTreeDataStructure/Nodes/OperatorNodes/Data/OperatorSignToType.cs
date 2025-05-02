// <copyright file="OperatorSignToType.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Data;
using System.Reflection;

namespace SpreadsheetEngine.ExpressionTreeDS
{
    /// <summary>
    /// type of operator nodes.
    /// </summary>
    internal static class OperatorSignToType
    {
        /// <summary>
        /// Initializes static members of the <see cref="OperatorSignToType"/> class.
        ///     Called once when any static member is accessed.
        /// </summary>
        static OperatorSignToType()
        {
            // getting assembly without loading to current AppDomain from a context.
            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string solutionDirectory = directory.Parent!.Parent!.Parent!.Parent!.FullName;
            Assembly.LoadFrom($"{solutionDirectory}\\OperatorLibrary\\bin\\Debug\\net8.0\\OperatorLibrary.dll");

            // add each op and type retreived from method into dictionary
            List<Assembly> currentContexAndDomaintAssemblyList = AppDomain.CurrentDomain.GetAssemblies().ToList();
            TraverseAvailableOperators(currentContexAndDomaintAssemblyList);
        }

        /// <summary>
        /// Gets characters matching to a Operator Node type.
        /// </summary>
        public static Dictionary<char, Type> Data
        {
            get;
        }

            = new Dictionary<char, Type>();

        /// <summary>
        /// Called To Initialize Static Members (Should Only Be Called Once).
        /// </summary>
        public static void Initialize()
        {
        }

        /// <summary>
        /// Traverses assembly files looking for specific classes that inherit from
        ///     operatorNode. then using retreived operator and operator node class
        ///     and use them as arguments for the OnOperator delegate.
        /// </summary>
        /// <param name="assemblies"> assemblies in use in current context. </param>
        /// <exception cref="Exception"> When there is a problem retreiving operator nodes. </exception>
        private static void TraverseAvailableOperators(List<Assembly> assemblies)
        {
            Type operatorNodeType = typeof(OperatorNode);

            IEnumerable<Type> operatorTypes;

            // sift through each assembly checking for inherited types of OperatorNode
            foreach (var assembly in assemblies)
            {
                operatorTypes = assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                // for every operator type found in specific assembly add to dictionary of operatortypes connected with operator
                foreach (Type operatorType in operatorTypes)
                {
                    FieldInfo? fieldInfo = null;
                    fieldInfo = operatorType.GetField("Op", BindingFlags.Static | BindingFlags.NonPublic);

                    if (fieldInfo is null)
                    {
                        throw new NoNullAllowedException("field for " + operatorType.Name + " Not found");
                    }

                    object? op = fieldInfo.GetValue(operatorType);

                    if (op is char)
                    {
                        // call method by using op field and type of operator retrieved as arguments.
                        if (OperatorSignToType.Data.TryAdd((char)op, operatorType) == false)
                        {
                            throw new ArgumentException("the operator already exists in operator dictionary");
                        }
                    }
                }
            }
        }
    }
}
