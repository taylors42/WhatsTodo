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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WhatsTodo.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Você não possui tarefas pendentes..
        /// </summary>
        public static string DontHaveTask {
            get {
                return ResourceManager.GetString("DontHaveTask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERRO DE COMANDO- Use o formato válido para adicionar uma tarefa, espaçando entre os elementos:/addtask Titulo Descrição Horário (12:00).
        /// </summary>
        public static string ErrorAddtask {
            get {
                return ResourceManager.GetString("ErrorAddtask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERRO DE COMANDO- Use o formato válido para deletar uma tarefa, espaçando entre os elementos:/deletetask Titulo.
        /// </summary>
        public static string ErrorDeletetask {
            get {
                return ResourceManager.GetString("ErrorDeletetask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERRO DE COMANDO- Use o formato válido para editar uma tarefa, espaçando entre os elementos:/edittask NovoTitulo NovaDescrição NovoHorário(12:00).
        /// </summary>
        public static string ErrorEdittask {
            get {
                return ResourceManager.GetString("ErrorEdittask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Para solicitar ajuda digite /help.
        /// </summary>
        public static string FirstUserMessage {
            get {
                return ResourceManager.GetString("FirstUserMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Formato Invalido, digite /help para obter informações.
        /// </summary>
        public static string FormatInvalid {
            get {
                return ResourceManager.GetString("FormatInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to COMANDOS
        ///(Seguir os espaçamentos entre comandos, títulos, descrições e horários)
        ///
        ///ADICIONAR UMA TAREFA:
        ////addtask Titulo Descrição Horário(12:00)
        ///
        ///EDITAR UMA TAREFA EXISTENTE:
        ////edittask NovoTitulo NovaDescrição NovoHorario(13:00)
        ///
        ///LISTAR AS TAREFAS EXISTENTES:
        ////listtask
        ///
        ///DELETAR UMA TAREFA EXISTENTE:
        ////deletetask Titulo
        ///
        ///CONTATO DOS CRIADORES DO BOT:
        ////créditos
        ///.
        /// </summary>
        public static string HelpMessageText {
            get {
                return ResourceManager.GetString("HelpMessageText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tarefa não encontrada. Use /helppara mais informações..
        /// </summary>
        public static string TaskNotFound {
            get {
                return ResourceManager.GetString("TaskNotFound", resourceCulture);
            }
        }
    }
}
