﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GMB_And_Selenium.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.8.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string USERNAME {
            get {
                return ((string)(this["USERNAME"]));
            }
            set {
                this["USERNAME"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string PASSWORD {
            get {
                return ((string)(this["PASSWORD"]));
            }
            set {
                this["PASSWORD"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("CHROME")]
        public string BROWSER {
            get {
                return ((string)(this["BROWSER"]));
            }
            set {
                this["BROWSER"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("//div[contains(text(),\'deliver goods\')]")]
        public string DELIVER_GOODS {
            get {
                return ((string)(this["DELIVER_GOODS"]));
            }
            set {
                this["DELIVER_GOODS"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("//div[@role=\'option\' and @aria-label=\'{0}\']/..")]
        public string STATE_PARENT {
            get {
                return ((string)(this["STATE_PARENT"]));
            }
            set {
                this["STATE_PARENT"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("//div[@role=\'option\' and @aria-label=\'{0}\']")]
        public string STATE_CHILD {
            get {
                return ((string)(this["STATE_CHILD"]));
            }
            set {
                this["STATE_CHILD"] = value;
            }
        }
    }
}
