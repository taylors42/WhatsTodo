﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WhatsTodo {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Commands {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Commands() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WhatsTodo.Commands", typeof(Commands).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /addtask,addtask,/add,add,new,/new,adicionar,/adicionar.
        /// </summary>
        public static string AddCommand {
            get {
                return ResourceManager.GetString("AddCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /deletetask,deletetask,/delete,delete,/del,del,remove,/remove,deletar,/deletar,rm,/rm.
        /// </summary>
        public static string DeleteCommand {
            get {
                return ResourceManager.GetString("DeleteCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /edittask,edittask,/edit,edit,editar,/editar,change,/change.
        /// </summary>
        public static string EditCommand {
            get {
                return ResourceManager.GetString("EditCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /ajuda,ajuda,/help,help.
        /// </summary>
        public static string HelpCommand {
            get {
                return ResourceManager.GetString("HelpCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /listtask,listtask,/list,list,mostrar,/mostrar.
        /// </summary>
        public static string ListCommand {
            get {
                return ResourceManager.GetString("ListCommand", resourceCulture);
            }
        }
    }
}
