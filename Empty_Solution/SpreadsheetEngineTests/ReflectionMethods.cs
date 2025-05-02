// <copyright file="ReflectionMethods.cs" company="Chandler_Guthrie-WSU_ID:011801740">
// Copyright (c) Chandler_Guthrie-WSU_ID:011801740. All rights reserved.
// </copyright>

using System.Reflection;

namespace SpreadsheetEngineTests
{
    /// <summary>
    /// methods that can be used in multiple cases for testing.
    ///     especially private methods.
    /// </summary>
    internal class ReflectionMethods
    {
        /// <summary>
        /// getting a private method from a class type.
        /// </summary>
        /// <param name="methodName"> name of the method that is private. </param>
        /// <param name="typeBeingTested"> the type under current testing. </param>
        /// <param name="bindingflags"> Bindingflags to apply to getting method. </param>
        /// <returns> method from class type. </returns>
        public static MethodInfo? GetMethod(string methodName, Type typeBeingTested, BindingFlags bindingflags = BindingFlags.Default)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Assert.Fail("methodName cannot be null or whitespace");
            }

            // get method only if it exists otherwise its null.
            var method = typeBeingTested.GetMethod(methodName, BindingFlags.NonPublic | bindingflags);
            if (method == null)
            {
                Assert.Fail(string.Format("{0} method not found", methodName));
            }

            return method;
        }

        /// <summary>
        /// get a private field of a class.
        /// </summary>
        /// <param name="nameOfField"> name of the field. </param>
        /// <param name="typeBeingTested"> the type under current testing. </param>
        /// <param name="bindingflags"> Bindingflags to apply to getting feild. </param>
        /// <returns> fields from class type. </returns>
        public static FieldInfo? GetField(string nameOfField, Type typeBeingTested, BindingFlags bindingflags = BindingFlags.Default)
        {
            var field = typeBeingTested.GetField(nameOfField, BindingFlags.NonPublic | bindingflags);
            if (field is null)
            {
                Assert.Fail("fields not found");
            }

            return field;
        }
    }
}
